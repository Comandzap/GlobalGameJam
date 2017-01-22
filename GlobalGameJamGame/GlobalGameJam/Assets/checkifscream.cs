using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkifscream : MonoBehaviour {
    public GameObject Player;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.transform.parent.gameObject.GetComponent<AudioInput>().shouting);
		if(this.transform.parent.gameObject.GetComponent<AudioInput>().shouting)
        {
            this.GetComponent<Animator>().SetBool("Screaming", true);
            this.GetComponent<SpriteRenderer>().color = this.transform.parent.gameObject.GetComponent<AudioInput>().fireColor;
        } else
        {
            this.GetComponent<Animator>().SetBool("Screaming", false);
        }
	}
}
