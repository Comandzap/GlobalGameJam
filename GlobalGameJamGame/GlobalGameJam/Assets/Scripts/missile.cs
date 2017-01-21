using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

	Rigidbody body;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		body.AddForce(Vector3.right * 40);
	}


}
