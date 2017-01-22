using System;
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
    public Animator charAnimatior;

    public float eeeviiil = 0;


    public GameObject OtherPlayer;

    GameObject[] healthBar;

    public int health = 3;

    public bool controller = false;

    float damageTimer = 0.0f;
    float cooldown = 5.0f;

    public AudioClip Jump1;
    public AudioClip Jump2;
    public AudioClip Jump3;
    public AudioClip Jump4;

    public AudioClip[] JumpSounds = new AudioClip[4];
    public AudioClip[] TakeDamageSounds = new AudioClip[4];

    public AudioClip Land;

    public AudioClip takeDamage1;
    public AudioClip takeDamage2;
    public AudioClip takeDamage3;
    public AudioClip takeDamage4;

    public GameObject AudioGo;


    bool isCooldown = true;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();

        healthBar = new GameObject[health];

        healthBar[0] = health0;
        healthBar[1] = health1;
        healthBar[2] = health2;

        OtherPlayer = GetComponent<AudioInput>().OtherPlayer;

        JumpSounds[0] = Jump1;
        JumpSounds[1] = Jump2;
        JumpSounds[2] = Jump3;
        JumpSounds[3] = Jump4;

        TakeDamageSounds[0] = takeDamage1;
        TakeDamageSounds[1] = takeDamage2;
        TakeDamageSounds[2] = takeDamage3;
        TakeDamageSounds[3] = takeDamage4;
    }

    void updateMovement()
    {
        charAnimatior.SetFloat("Evil", eeeviiil);
        Vector2 force = new Vector2(0, 0);
        float horizontal = Input.GetAxis(playerHorizontal);


        if (body.velocity.x != 0)
        {
            if (body.velocity.x > 0)
            {
                Vector3 s = transform.localScale;
                s.x = -Mathf.Abs(s.x);
                transform.localScale = s;
            }
            else
            {
                Vector3 s = transform.localScale;
                s.x = Mathf.Abs(s.x);
                transform.localScale = s;
            }
        }
        if (Mathf.Abs(horizontal) > 0.05f)
        {
            if (!jump)
            {
                charAnimatior.SetBool("Moving", true);
            }
            if (horizontal > 0)
            {
                
                Vector3 s = transform.localScale;
                s.x = -Mathf.Abs(s.x);
                transform.localScale = s;
            }
            else
            {
                Vector3 s = transform.localScale;
                s.x = Mathf.Abs(s.x);
                transform.localScale = s;
            }
        } else
        {
            charAnimatior.SetBool("Moving", false);
        }

        if (Input.GetButtonDown(playerJump))
        {
            
            if (Mathf.Abs(body.velocity.y) < 100)
            {
                Debug.Log((int)UnityEngine.Random.Range(0.0f, 4.0f));
                AudioGo.GetComponent<AudioSource>().clip = JumpSounds[(int)UnityEngine.Random.Range(0.0f, 4.0f)];
                AudioGo.GetComponent<AudioSource>().loop = false;
                AudioGo.GetComponent<AudioSource>().Play();

                force = Vector2.up * jumpPower;
                jump = true;
            }
        }
        force += Time.deltaTime * Vector2.right * speed * horizontal;



        if (body.velocity.y > 1)
        {
            charAnimatior.SetBool("InAir", jump);
        }
        else
        {
            charAnimatior.SetBool("InAir", jump);
        }

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

        damageTimer += Time.deltaTime;
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
    }

    public void TakeDamage()
    {

        if (isCooldown)
        {
            if (damageTimer > cooldown)
            {
                isCooldown = false;
            }
        }

        if(!isCooldown)
        {
            damageTimer = 0.0f;
            Debug.Log((int)UnityEngine.Random.Range(0.0f, 4.0f));
            AudioGo.GetComponent<AudioSource>().clip = TakeDamageSounds[(int)UnityEngine.Random.Range(0.0f, 4.0f)];
            AudioGo.GetComponent<AudioSource>().loop = false;
            AudioGo.GetComponent<AudioSource>().Play();
            switch (health)
            {
                case 3:
                    healthBar[2].GetComponent<Animator>().SetTrigger("Explode");
                    //Destroy(healthBar[2]);
                    health--;
                    break;
                case 2:
                    healthBar[1].GetComponent<Animator>().SetTrigger("Explode");
                    //Destroy(healthBar[2]);
                    health--;
                    break;
                case 1:
                    healthBar[0].GetComponent<Animator>().SetTrigger("Explode");
                    //Destroy(col.gameObject);
                    health--;
                    break;
                default:
                    Debug.Log("You are dead!");
                    //Destroy();
                    break;
            }
            isCooldown = true;
        }
    }
}