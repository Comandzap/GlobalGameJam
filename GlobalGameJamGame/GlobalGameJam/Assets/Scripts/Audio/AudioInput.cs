using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioInput : MonoBehaviour
{
    public GameObject missile;
    public bool fireDir;

    public Vector3 fireVector = new Vector3(1, 0, 0);
    public Color fireColor = new Color(255, 0, 0);

    public Gradient g;

    public float lowestLoudness;
    public float highestLoudness;

    float[] Samples;
    float[] Samples_Freq;
    int sampleCounter = 0;

    MicControl micController;

    void Start()
    {
        Samples = new float[25];
        Samples_Freq = new float[25];

        micController = this.GetComponent<MicControl>();
    }


    void FixedUpdate()
    {
        //Use this in your script to call loudnes data from selected controller 

        if (sampleCounter > Samples.Length - 1)
            sampleCounter = 0;

        Samples[sampleCounter++] = micController.loudness;
    }

    void Update()
    {
        float K = (Mathf.Sqrt(2) / 2) / (lowestLoudness + highestLoudness);

        float Average = GetAverage();

        if (Average < lowestLoudness)
            Average = lowestLoudness;
        if (Average > highestLoudness)
            Average = highestLoudness;


        fireVector = new Vector3(Mathf.Cos(Average * K), Mathf.Sin(Average * K));

        //float c = pitchValue/800;

        //if(pitchValue > 0 && pitchValue < 800)
        //{
        //fireColor = g.Evaluate(c);
        //} else if(pitchValue >= 1000)
        //{
        //fireColor = g.Evaluate(1.0f);
        //} else
        //{
        //fireColor = g.Evaluate(0.0f);
        //}

        Debug.DrawLine(this.GetComponent<Transform>().position, fireVector * 10);
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
}