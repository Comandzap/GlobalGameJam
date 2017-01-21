using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

	Rigidbody body;
	bool right;
    Vector3 directionVector = new Vector3(1, 0, 0);
    public SpriteRenderer renderer;


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
			body.AddForce(directionVector * 200);
		}
		else
		{
            directionVector.x *= -1;
			body.AddForce(directionVector * 200);
		}
	}

	public void direction(bool SetRight, Vector3 vector, Color color)
	{
		right = SetRight;
        directionVector = vector;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = color;
	}


}
