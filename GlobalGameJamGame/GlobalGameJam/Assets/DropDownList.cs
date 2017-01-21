using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownList : MonoBehaviour {

    string[] devices;
    int numDevices = 0;

    public AudioInput input;

    Dropdown list;

    // Use this for initialization
    void Start () {
        list = this.GetComponent<Dropdown>();
        devices = Microphone.devices;
        numDevices = devices.Length;
        UpdateList();
	}
	
	// Update is called once per frame
	void Update () {
		if(Microphone.devices.Length != numDevices)
        {
            UpdateList();
        }
	}

    void UpdateList()
    {
        
        list.ClearOptions();
        devices = Microphone.devices;

        List<string> options = new List<string>();

        foreach(string device in devices)
        {
            if(device == "")
            {
                options.Add("Built in Microphone");
            } else
            {
                options.Add(device);
            }
            
        }

        list.AddOptions(options);
    }

    
}
