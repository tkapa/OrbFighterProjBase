using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [System.Serializable]
    public class movementVariables {
        //Set up variables for projectile movement
        public float moveSpeed = 5.0f;
    }

    public movementVariables moveVars = new movementVariables();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            other.GetComponent<CharacterController>().moveSettings.isStunned = true;
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
