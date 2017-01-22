using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayer : MonoBehaviour {

    Collider2D ourCollider;

	// Use this for initialization
	void Start () {
        ourCollider = this.GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Im colliding with: " + col.gameObject.name);
    }
}
