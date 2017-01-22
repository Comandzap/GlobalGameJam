using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Transform playerUI;
    bool jump = false;
    public float jumpPower;
    Rigidbody body;
    public float speed;
    public string Jumpbottum;
    public string FireButton;

    public bool fireDir;

    public GameObject missile;

    public float facing;

    public string Axis;

    public string playerHorizontal;
    public string playerJump;

    public GameObject health0;
    public GameObject health1;
    public GameObject health2;

    public BoxCollider2D groundFinder;


    public GameObject OtherPlayer;

    GameObject[] healthBar;

    public int health = 3;

    public bool controller = false;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();

        healthBar = new GameObject[health];

        healthBar[0] = health0;
        healthBar[1] = health1;
        healthBar[2] = health2;

        OtherPlayer = GetComponent<AudioInput>().OtherPlayer;
    }

    void updateMovement()
    {
        Vector2 force = new Vector2(0, 0);
        float horizontal = Input.GetAxis(playerHorizontal);

        if (Input.GetButtonDown(playerJump))
        {
            if (Mathf.Abs(body.velocity.y) < 100)
            {
                force = Vector2.up * jumpPower;
            }
        }
        force += Time.deltaTime * Vector2.right * speed * horizontal;

        body.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.magnitude > 14.0f)
        {
            body.velocity = 14.0f * body.velocity.normalized;
        }

//        Debug.Lasog(body.velocity.magnitude);

        updateMovement();
//        oldMovement();

        playerUI.transform.position = new Vector2(transform.position.x + facing * 2, transform.position.y);
        playerUI.transform.localScale = new Vector3(facing, 1, 1);
    }

    private void oldMovement()
    {
        if (!controller)
        {
            Vector2 force = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                if (!jump)
                {
                    force = Vector2.up * jumpPower;
                    jump = true;
                }
            }

            if (jump)
            {
                if (Input.GetKey(KeyCode.S))
                {
                }

                if (Input.GetKey(KeyCode.A))
                {
                    force += -Time.deltaTime * Vector2.right * speed;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    force += Time.deltaTime * Vector2.right * speed;
                }
            }

            body.AddForce(force);
        }
        else
        {
            Vector2 force = new Vector2(0, 0);

            if (Input.GetButton("Jump"))
            {
                if (!jump)
                {
                    force += Vector2.up * jumpPower;
                    jump = true;
                }
            }

            if (Input.GetButton("Fire1"))
            {
                (Instantiate(missile, playerUI.transform.position, Quaternion.identity) as GameObject)
                    .GetComponent<missile>()
                    .direction(fireDir, OtherPlayer.transform.position, new Color(1, 1, 1, 1));
            }

            if (jump)
                force += Input.GetAxis("Horizontal") * Vector2.right * speed * Time.deltaTime;

            body.AddForce(force);
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Player")
        {
            jump = false;
        }
        else
        {
            jump = true;
        }


        if (col.gameObject.tag == "Projectile")
        {
            switch (health)
            {
                case 3:
                    healthBar[2].GetComponent<Animator>().SetTrigger("Explode");
                    Destroy(col.gameObject);
                    health--;
                    break;
                case 2:
                    healthBar[1].GetComponent<Animator>().SetTrigger("Explode");
                    Destroy(col.gameObject);
                    health--;
                    break;
                case 1:
                    healthBar[0].GetComponent<Animator>().SetTrigger("Explode");
                    Destroy(col.gameObject);
                    health--;
                    break;
                default:
                    Debug.Log("You are dead!");
                    break;
            }
        }
    }
}