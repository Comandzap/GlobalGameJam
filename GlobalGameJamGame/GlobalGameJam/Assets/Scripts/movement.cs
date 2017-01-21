using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {


	public Transform playerUI;
	bool jump;
	public float jumpPower;
	Rigidbody body;
	public float speed;
	public string Jumpbottum;
    public string FireButton;

    public bool fireDir;

    public GameObject missile;

	public float facing;
    

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

		

		if (Input.GetAxis("Horizontal")>0.0f)
		{
			facing = 1;
			fireDir = true;
		}
		else if (Input.GetAxis("Horizontal") < 0.0f)
		{
			facing = -1;
			fireDir = false;
		}

		playerUI.transform.position = new Vector2(transform.position.x+ facing, transform.position.y);
		playerUI.transform.localScale = new Vector3(facing, 1,1);


		if (Input.GetButtonDown(FireButton))
        {
			playerUI.gameObject.SetActive(true);
        }
		if(Input.GetButtonUp(FireButton))
		{
			playerUI.gameObject.SetActive(false);
			AudioInput audioClass = this.GetComponent<AudioInput>();
            (Instantiate(missile, playerUI.transform.position, Quaternion.identity) as GameObject).GetComponent<missile>().direction(fireDir, audioClass.GetDirectionVector(), audioClass.GetColorTemp());
		}

		
	}


	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ground")
		{
			jump = false;
		}
	}

}
