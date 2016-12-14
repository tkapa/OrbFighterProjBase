using UnityEngine;
using System.Collections;

    [System.Serializable]
    public class stunVariables
    {
        public float minStunTime = 0.0f;
        public float maxStunTime = 0.0f;

        public float minDistance = 0.0f;
        public float maxDistance = 15.0f;

        public AnimationCurve stunTimeCurve;
    }
	
public class Projectile : MonoBehaviour {

    //Stop the parent of the object from hitting itself
    GameObject myParent;
	Vector3 enemyPos;
	float moveSpeed = 1.0f;
	
    private Vector3 direction;
    private float travelDistance = 0.0f;
    private float alteredStunTime = 0.0f;
    
    public stunVariables stunVars = new stunVariables();

	// Use this for delayed initialization
	public void StartManual () {
        Debug.Log("Go");
        direction = (enemyPos - this.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Movement();
    }
	
	public void SetInformation(ProjectileInfoPack infoPacket){
        //Take the information provided to this object to alter current null variables
        myParent = infoPacket.thisObject;
        enemyPos = infoPacket.enemyObjectPos;
        moveSpeed = infoPacket.newSpeed;	
	}

    void Movement()
    { 
        //Move the projectile's positiion
        this.transform.position += direction * moveSpeed * Time.deltaTime;
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

            var target = other.GetComponent<TwoDimensionalCharacterController>();

            //Change the stun time and stun the player
            target.combatSettings.stunTime = alteredStunTime;
            target.myState = TwoDimensionalCharacterController.PlayerStates.psStunned;

            //Destroy this object after hit
            Destroy(this.gameObject);
        }
        else if(other.gameObject != myParent)
            Destroy(this.gameObject);
    }
}
