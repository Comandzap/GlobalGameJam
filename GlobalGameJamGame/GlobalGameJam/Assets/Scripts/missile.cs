using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

	Rigidbody body;
	bool right;
    Vector3 directionVector = new Vector3(1, 0, 0);
    SpriteRenderer renderer;
	float time = 0;

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
		
		time += Time.deltaTime;
		if(time > 2)
		{
			Destroy(this.gameObject);
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
