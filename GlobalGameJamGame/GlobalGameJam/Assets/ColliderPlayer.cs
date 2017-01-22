using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayer : MonoBehaviour {

    public Collider2D otherCollider;
    Collider2D ourCollider;

	// Use this for initialization
	void Start () {
        ourCollider = this.GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ourCollider.bounds.Intersects(otherCollider.bounds))
        {
            Debug.Log("Intersecting other player!");
        }
	}
}
