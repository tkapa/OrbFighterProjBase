using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [System.Serializable]
    public class ProjectileVariables {
        //Set up variables for projectile movement
        public float moveSpeed = 5.0f;
    }

    public ProjectileVariables projVars = new ProjectileVariables();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        movement();
    }

    void movement()
    {
        //Move the projectile's positiion
        transform.position += transform.up * projVars.moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            other.GetComponent<CharacterController>().combatSettings.isStunned = true;
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
