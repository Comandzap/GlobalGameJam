using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInput : MonoBehaviour {
    public bool isWorking = true;
    private bool lastValueOfIsWorking;

    private bool RealtimeOutput;
    private bool lastValueOfRealtimeOutput;

    AudioSource src;

    //public static FFT Instance;

    private float lastVolume = 0.0f;

    void Start()
    {
        src = GetComponent<AudioSource>();

        if(isWorking)
        {
            WorkStart();
            Debug.Log("Work Started!");
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
			//src.GetSpectrumData();
        }
    }

    private void WorkStop()
    {
        Microphone.End(null);
        Debug.Log("Work Stopped!");
    }
}
