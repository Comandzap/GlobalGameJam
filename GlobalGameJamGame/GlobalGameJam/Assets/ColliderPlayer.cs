using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayer : MonoBehaviour {

    public GameObject otherPlayer;
    Collider ourCollider;

    GameObject ourParent;

	// Use this for initialization
	void Start () {
        ourCollider = this.GetComponent<Collider>();
        ourParent = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (ourCollider.bounds.Intersects(otherPlayer.GetComponent<Collider>().bounds))
        {
            if(ourParent.GetComponent<AudioInput>().shouting)
            {
                // Damage other player.
                if(otherPlayer)
                    otherPlayer.GetComponent<movement>().TakeDamage();
            }
            
            //Debug.Log("Intersecting other player!");
        }
	}
}
