using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInput : MonoBehaviour {
    public bool isWorking = true;
    private bool lastValueOfIsWorking;

    private bool RealtimeOutput;
    private bool lastValueOfRealtimeOutput;

	public GameObject missile;

    int qSamples = 1024;    // Array size
    float refValue = 0.1f;
    float threshold = 0.02f; //Minimum amplitude
    float rmsValue;     // sound level - RMS
    float dbValue;      // sound level - dB
    float pitchValue;   // sound pitch - HZ

    private float[] samples;    // Audio samples
    private float[] spectrum;   // Audio Spectrum
    float fSample;

    AudioSource src;
    

    private float lastVolume = 0.0f;

    void Start()
    {
        src = GetComponent<AudioSource>();

        if(isWorking)
        {
            WorkStart();
            Debug.Log("Work Started!");
        }

        samples = new float[qSamples];
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;
    }

    void AnalyzeSound()
    {
        src.GetOutputData(samples, 0); // Fill array with samples
        int i;
        float sum = 0.0f;

        for(i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i];
        }

        rmsValue = Mathf.Sqrt(sum / qSamples);
        dbValue = 20 * Mathf.Log10(rmsValue / refValue);

        if (dbValue < -160) dbValue = -160;

        src.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxV = 0.0f;
        int maxN = 0;

        for(i = 0; i < qSamples; i++)
        {
            if(spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i;
            }
        }

        float freqN = maxN;

        if(maxN > 0 && maxN < qSamples - 1)
        {
            float dL = spectrum[maxN - 1] / spectrum[maxN];
            float dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (fSample / 2) / qSamples;  // Convert index to freq
    }

    void FixedUpdate()
    {
        AnalyzeSound();
        //Debug.Log("RMS: " + rmsValue.ToString("F2") + " (" + dbValue.ToString("F1") + " dB)\n"
        //         + "Pitch: " + pitchValue.ToString("F0") + " Hz");



        if(pitchValue < 1000 && pitchValue > 0 && dbValue > -10)
        {
            Debug.Log("Jump: " + pitchValue.ToString("F0"));
        } else if(pitchValue > 400 && dbValue > -10)
        {
			Debug.Log(transform.position.x);
			Instantiate(missile,new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            //Debug.Log("Attack" + pitchValue.ToString("F0"));
        } else
        {
            Debug.Log("Do Nothing!");
        }
    }

    void Update()
    {
        CheckIfWorkingChanged();
        CheckIfRealtimeOutputChanged();
    }

    public void CheckIfWorkingChanged()
    {
        if(lastValueOfIsWorking != isWorking)
        {
            if(isWorking)
            {
                WorkStart();
            }
            else
            {
                WorkStop();
            }
        }

        lastValueOfIsWorking = isWorking;
    }

    public void CheckIfRealtimeOutputChanged()
    {
        if(lastValueOfRealtimeOutput != RealtimeOutput)
        {
            DisableSound(RealtimeOutput);
        }

        lastValueOfRealtimeOutput = RealtimeOutput;
    }

    public void DisableSound(bool SoundOn)
    {
        if(SoundOn)
        {
            if(lastVolume > 0)
            {
                src.volume = lastVolume;
            }
            else
            {
                src.volume = 1.0f;
            }
        }
        else
        {
            src.volume = 0.0f;
        }
    }

    private void WorkStart()
    {
        Debug.Log("Work Started!");
        src.clip = Microphone.Start(null, true, 10, 44100);
        src.loop = true;
        while(!(Microphone.GetPosition(null) > 0))
        {
            src.Play();
            
        }
    }

    private void WorkStop()
    {
        Microphone.End(null);
        Debug.Log("Work Stopped!");
    }

    private int GetMax(float[] Data)
    {
        float max = 0.0f;
        int max_index = 0;
        for(int i = 0; i < Data.Length; i++)
        {
            if(Data[i] > max)
            {
                max = Data[i];
                max_index = i;
            }
        }

        return max_index;
    }

    private float Average(float[] Data)
    {
        float sum = 0.0f;

        for(int i = 0; i < Data.Length; i++)
        {
            sum += Data[i];
        }

        return Mathf.Sqrt(sum);
    }
}
