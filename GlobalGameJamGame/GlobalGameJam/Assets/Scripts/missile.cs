using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

	Rigidbody body;
	bool right;
    Vector3 directionVector = new Vector3(1, 0, 0);
    public SpriteRenderer renderer;
	float time = 0;

    bool firstTime = true;

    Vector3 target= new Vector3(0, 0, 0);

    // Use this for initialization
    void Start ()
	{
		body = GetComponent<Rigidbody>();

        this.tag = "Projectile";
    }
	
	// Update is called once per frame
	void Update ()
	{
        // Autotracking!
        if (target.magnitude > 0)
        {
            Vector3 ourPos = transform.position;
            Vector3 dir = target - ourPos;

            if (firstTime)
            {
                firstTime = false;

                transform.position += dir.normalized * 2;
            }

            dir.Normalize();

            body.AddForce(2000 * dir * Time.deltaTime);

            Debug.Log(target);
        }

		time += Time.deltaTime;
		if(time > 2)
		{
			Destroy(this.gameObject);
		}
	}

	public void direction(bool SetRight, Vector3 position,  Color color)
	{
		right = SetRight;
        renderer = GetComponent<SpriteRenderer>();
        target = position;
        renderer.color = color;
	}
}
