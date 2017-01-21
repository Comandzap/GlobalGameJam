﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

	bool jump;
	public float jumpPower;
	Rigidbody body;
	public float speed;
	public string Jumpbottum;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(Jumpbottum) && jump == false)
		{
			jump = true;
			body.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
		}

		//Debug.Log(Input.GetAxis("Horizontal"));
		float horizontalForce = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		body.AddForce(Vector3.right * horizontalForce);
	}


	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ground")
		{
			jump = false;
		}
	}

}
