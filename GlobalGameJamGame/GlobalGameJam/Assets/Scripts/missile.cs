using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

	Rigidbody body;
	bool right;



	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(right)
		{
			body.AddForce(Vector3.right * 200);
		}
		else
		{
			body.AddForce(Vector3.left * 200);
		}
	}

	public void direction(bool SetRight)
	{
		right = SetRight;
	}


}
