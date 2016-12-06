﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    //Stop the parent of the object from hitting itself
    public GameObject myParent;
    public Vector3 enemyPos;
    public Vector3 direction;

    [System.Serializable]
    public class ProjectileVariables {
        //Set up variables for projectile movement
        public float moveSpeed = 1.0f;
    }

    [System.Serializable]
    public class stunVariables
    {
        public float minStunTime = 0.0f;
        public float maxStunTime = 0.0f;

        public float minDistance = 0.0f;
        public float maxDistance = 15.0f;

        public AnimationCurve stunTimeCurve;
    }

    private float travelDistance = 0.0f;
    private float alteredStunTime = 0.0f;
    
    public stunVariables stunVars = new stunVariables();
    public ProjectileVariables projVars = new ProjectileVariables();


	// Use this for initialization
	void Start () {
        direction = (enemyPos - this.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        movement();
    }

    void movement()
    { 
        //Move the projectile's positiion
        this.transform.position += direction * projVars.moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        //Of the collider's tag is character, and it isn't the spawner, stun the player
        if (other.tag == "Character" && other.gameObject != myParent)
        {
            //Find the distance percentage of the projectile from the parent. Make it a % of the min - max distance.
            travelDistance = (Vector3.Distance(myParent.transform.position, this.gameObject.transform.position) - stunVars.minDistance) / stunVars.maxDistance;

            //Grab the percentage from the graph
            alteredStunTime = (stunVars.stunTimeCurve.Evaluate(travelDistance));

            //Find the stun time for the enemy player
            alteredStunTime *= (stunVars.maxStunTime - stunVars.minStunTime) + stunVars.minStunTime;

            //Change the stun time and stun the player
            other.GetComponent<CharacterController>().combatSettings.stunTime = alteredStunTime;
            other.GetComponent<CharacterController>().combatSettings.isStunned = true;

            //Destroy this object after hit
            Destroy(this.gameObject);
        }
        else if(other.gameObject != myParent)
            Destroy(this.gameObject);
    }
}
