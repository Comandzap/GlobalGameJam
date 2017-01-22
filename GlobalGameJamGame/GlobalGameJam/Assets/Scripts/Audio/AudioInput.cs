using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
class Peak
{
    public float amplitude;
    public int index;

    public Peak()
    {
        amplitude = 0f;
        index = -1;
    }

    public Peak(float _frequency, int _index)
    {
        amplitude = _frequency;
        index = _index;
    }
}

class AmpComparer : IComparer<Peak>
{
    public int Compare(Peak a, Peak b)
    {
        return 0 - a.amplitude.CompareTo(b.amplitude);
    }
}

class IndexComparer : IComparer<Peak>
{
    public int Compare(Peak a, Peak b)
    {
        return a.index.CompareTo(b.index);
    }
}

public class AudioInput : MonoBehaviour
{
    public Image DecibelMeter;
    public GameObject missile;
    public bool fireDir;

    public RectTransform PlayerUI;

    public Animator charAnimator;

    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    public int qSamples = 1024;

    public int binSize =
            8192
        ; // you can change this up, I originally used 8192 for better resolution, but I stuck with 1024 because it was slow-performing on the phone

    public float refValue = 0.1f;
    public float threshold = 0.01f;

    public GameObject OtherPlayer;


    private List<Peak> peaks = new List<Peak>();
    float[] samples;
    float[] spectrum;
    int samplerate;

    public Text display; // drag a Text object here to display values
    public bool mute = true;
    public AudioMixer masterMixer; // drag an Audio Mixer here in the inspector

    public int channelId = 0;

    public int micNum = 0;

    public Vector3 fireVector = new Vector3(1, 0, 0);
    public Color fireColor = new Color(255, 0, 0);

    public Gradient g;

    public float channel;

    void Start()
    {
        foreach (string device in Microphone.devices)
        {
            Debug.Log(device);
        }

        samples = new float[qSamples];
        spectrum = new float[binSize];
        samplerate = AudioSettings.outputSampleRate;

        // starts the Microphone and attaches it to the AudioSource
        GetComponent<AudioSource>().clip = Microphone.Start(Microphone.devices[micNum], true, 1, samplerate);
        GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
        while (!(Microphone.GetPosition(Microphone.devices[micNum]) > 0))
        {
        } // Wait until the recording has started
        GetComponent<AudioSource>().Play();

        // Mutes the mixer. You have to expose the Volume element of your mixer for this to work. I named mine "masterVolume".
        masterMixer.SetFloat("masterVolume", -80f);
    }

    void AnalyzeSound(int channel)
    {
        GetComponent<AudioSource>().GetOutputData(samples, channel); // fill array with samples
        int i = 0;
        float sum = 0f;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min

        // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(spectrum, channel, FFTWindow.BlackmanHarris);
        float maxV = 0f;
        for (i = 0; i < binSize; i++)
        {
            // find max
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                peaks.Add(new Peak(spectrum[i], i));
                if (peaks.Count > 5)
                {
                    // get the 5 peaks in the sample with the highest amplitudes
                    peaks.Sort(new AmpComparer()); // sort peak amplitudes from highest to lowest
                    //peaks.Remove (peaks [5]); // remove peak with the lowest amplitude
                }
            }
        }
        float freqN = 0f;
        if (peaks.Count > 0)
        {
            peaks.Sort(new IndexComparer()); // sort indices in ascending order
            maxV = peaks[0].amplitude;
            int maxN = peaks[0].index;
            freqN = maxN; // pass the index to a float variable
            if (maxN > 0 && maxN < binSize - 1)
            {
                // interpolate index using neighbours
                var dL = spectrum[maxN - 1] / spectrum[maxN];
                var dR = spectrum[maxN + 1] / spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }
        }
        pitchValue = freqN * (samplerate / 2f) / binSize; // convert index to frequency
        peaks.Clear();
    }

    float timer = 0.1f;

    public float lowestDb = -15.0f;
    public float highestDb = 10.0f;

    float[] Samples_1 = new float[5];
    float[] Freq_Array1 = new float[5];
    int sampleCounter = 0;

    void FixedUpdate()
    {
        AnalyzeSound(channelId);
        if (sampleCounter > Samples_1.Length - 1)
        {
            sampleCounter = 0;
        }


        Freq_Array1[sampleCounter] = pitchValue;
        Samples_1[sampleCounter++] = dbValue;

        Update_Player();
    }

    bool projectileCooldown = false;

    float maxTime = 1.0f;
    float protimer = 0.0f;

    void Update_Player()
    {
        float K = (Mathf.Sqrt(2) / 2) / (highestDb + Mathf.Abs(lowestDb));
        float Average = GetAverage(Samples_1);

        lowestDb = Mathf.Min(Samples_1);
        highestDb = Mathf.Max(Samples_1);


        charAnimator.SetBool("SCREAMING", Average > -4);
        Debug.Log(Average > -4);
//            Debug.Log("Average: " + Average + " lowest: " + lowestDb + " higest: " + highestDb);


        if (Average < lowestDb)
        {
            Average = lowestDb;
        }
        if (Average > highestDb)
        {
            Average = highestDb;
            // Fire a projectile if its not on cooldown

            if (!projectileCooldown)
            {
                (Instantiate(missile,
                        this.transform.position + (OtherPlayer.transform.position - OtherPlayer.transform.position)
                        .normalized * 5, Quaternion.identity) as GameObject).GetComponent<missile>()
                    .direction(fireDir, OtherPlayer.transform.position, GetColorTemp());
                projectileCooldown = true;
            }
            else
            {
                protimer += Time.fixedDeltaTime;
            }

            Debug.Log(protimer);
        }

        //highestDb - lowestDb;
        float DBMax = 0.0f;
        DecibelMeter.fillAmount = (Average + Mathf.Abs(lowestDb)) / (Mathf.Abs(highestDb) + Mathf.Abs(lowestDb));

        fireVector = new Vector3(Mathf.Cos(Average * K), Mathf.Sin(Average * K));

        float c = pitchValue / 800;

        if (pitchValue > 0 && pitchValue < 800)
        {
            fireColor = g.Evaluate(c);
        }
        else if (pitchValue >= 1000)
        {
            fireColor = g.Evaluate(1.0f);
        }
        else
        {
            fireColor = g.Evaluate(0.0f);
        }
    }

    void Update()
    {
        if (protimer >= maxTime)
        {
            projectileCooldown = false;
            protimer = 0.0f;
        }
    }

    float GetAverage(float[] Data)
    {
        float sum = 0.0f;

        for (int i = 0; i < Data.Length; i++)
        {
            sum += Data[i];
        }

        return sum / Data.Length;
    }


    public Vector3 GetDirectionVector()
    {
        return fireVector;
    }

    public Color GetColorTemp()
    {
        return fireColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}