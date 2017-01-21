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
    public GameObject missile;
    public bool fireDir;

    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    public int qSamples = 1024;
    public int binSize = 8192; // you can change this up, I originally used 8192 for better resolution, but I stuck with 1024 because it was slow-performing on the phone
    public float refValue = 0.1f;
    public float threshold = 0.01f;


    private List<Peak> peaks = new List<Peak>();
    float[] samples;
    float[] spectrum;
    int samplerate;

    public Text display; // drag a Text object here to display values
    public bool mute = true;
    public AudioMixer masterMixer; // drag an Audio Mixer here in the inspector

    public int micNum = 0;

    public Vector3 fireVector = new Vector3(1, 0, 0);
    public Color fireColor = new Color(255, 0, 0);

    public Gradient g;

    void Start()
    {
        samples = new float[qSamples];
        spectrum = new float[binSize];
        samplerate = AudioSettings.outputSampleRate;

        // starts the Microphone and attaches it to the AudioSource
        GetComponent<AudioSource>().clip = Microphone.Start(Microphone.devices[micNum], true, 1, samplerate);
        GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
        while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
        GetComponent<AudioSource>().Play();

        // Mutes the mixer. You have to expose the Volume element of your mixer for this to work. I named mine "masterVolume".
        masterMixer.SetFloat("masterVolume", -80f);


        //GradientColorKey[] gck;
        //GradientAlphaKey[] gak;
        //g = new Gradient();
        //gck = new GradientColorKey[2];
        //gck[0].color = Color.red;
        //gck[0].time = 0.0F;
        //gck[1].color = Color.blue;
        //gck[1].time = 1.0F;
        //gak = new GradientAlphaKey[2];
        //gak[0].alpha = 1.0F;
        //gak[0].time = 0.0F;
        //gak[1].alpha = 0.0F;
        //gak[1].time = 1.0F;
        //g.SetKeys(gck, gak);
    }

    void AnalyzeSound()
    {
        GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
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
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0f;
        for (i = 0; i < binSize; i++)
        { // find max
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                peaks.Add(new Peak(spectrum[i], i));
                if (peaks.Count > 5)
                { // get the 5 peaks in the sample with the highest amplitudes
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
            { // interpolate index using neighbours
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

    float[] Samples = new float[25];
    int sampleCounter = 0;

    void FixedUpdate()
    {
        AnalyzeSound();
        //Debug.Log("RMS: " + rmsValue.ToString("F2") + " (" + dbValue.ToString("F1") + " dB)\n"
        //      + "Pitch: " + pitchValue.ToString("F0") + " Hz");

        if(sampleCounter > Samples.Length-1)
        {
            sampleCounter = 0;
        }

        Samples[sampleCounter++] = dbValue;
    }

    void Update()
    {
        //Mathf.Sin(lowestDb * K) = 0;
        //lowestDb = 0;

        //(highestDb + Mathf.Abs(lowestDb)) * k = Mathf.Sqrt(2) / 2;
        float K = (Mathf.Sqrt(2) / 2) / (highestDb + Mathf.Abs(highestDb));


        //Debug.Log(Mathf.Sin(GetAverage() * K));

        float Average = GetAverage();

        if (Average < lowestDb)
            Average = lowestDb;
        if (Average > highestDb)
            Average = highestDb;


        fireVector = new Vector3(Mathf.Cos(Average * K), Mathf.Sin(Average * K));

        float c = pitchValue/800;

        if(pitchValue > 0 && pitchValue < 800)
        {
            Debug.Log(c);
            fireColor = g.Evaluate(c);
        } else if(pitchValue >= 1000)
        {
            fireColor = g.Evaluate(1.0f);
        } else
        {
            fireColor = g.Evaluate(0.0f);
        }

        //fireColor = new Color(color_r, color_g, color_b);

        //Debug.DrawLine(this.GetComponent<Transform>().position, new Vector3(Mathf.Cos(Average * K), Mathf.Sin(Average * K), 0) * 100, Color.red, 1);
    }

    float GetAverage()
    {
        float sum = 0.0f;

        for(int i = 0; i < Samples.Length; i++)
        {
            sum += Samples[i];
        }

        return sum / Samples.Length;
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