using UnityEngine;
using System.Collections;

    //Changeable variables that manage movement
    [System.Serializable]
    public class MoveMentSettings
    {
        //Variables that relate to the movement of a character
        public float moveSpeed = 5.0f;
        public float jumpVelocity = 10.0f;
        public float dashSpeed = 1.0f;
        public float dashDist = 5.0f;
        public AnimationCurve dashAniCurve;
        public LayerMask ground;
    }

    //Changeable variales that manage what inputs to use
    [System.Serializable]
    public class InputSettings
    {
        //Variables that take axis input from the player
        public string HORIZONTAL_INPUT = "Horizontal";
        public string VERTICAL_INPUT = "Vertical";
        public string PROJECTILE_INPUT = "e";
        public string DASH_INPUT = "q";
        public string SUPER_INPUT = "x";
    }

    //Changeable variables that manage combat
    [System.Serializable]
    public class CombatSettings
    {
        //Character stun variables
        public float stunTime = 0.75f;
        public float myDamage = 10.0f;

        //Character Projectile times
        public GameObject projectile;
        public float projectileSpeed = 6.0f;
        public float projectileResetTime = 1.0f;

        //Dash Vars
        public float dashResetTime = 0.75f;    

        //Health and Super Values
        public float maxHealth = 100.0f;
        public float currHealth = 100.0f;

        public float maxSuper = 100.0f;
        public float currSuper = 0.0f;
    }

    [System.Serializable]
    public class audioClips
    {
        //Audioclips for *Insert verb here*
        public AudioClip jumped;
        public AudioClip projectiled;
        public AudioClip dashed;
        public AudioClip damaged;
        public AudioClip stunned;
        public AudioClip super;
    }
	
public class CharacterController : MonoBehaviour {

    //Various points in the scenes
    public GameObject anchorPoint;
    public GameObject otherPlayer; 
    public GameObject projectilePoint;

    //Public variable classes initialisation
    public MoveMentSettings moveSettings = new MoveMentSettings();
    public InputSettings inputSettings = new InputSettings();
    public CombatSettings combatSettings = new CombatSettings();
    public audioClips audioClip = new audioClips();

    //Bools that govern input and action
    private bool dashInput = false;
    private bool attackInput = false;

	public enum PlayerStates{
		psNeutral,
		psStunned,
		psDashing,
		psDashRecovery,
        psProjectileRecovery
	};
	
	public PlayerStates myState;
	
    private float distToGround = 0.1f;

    //Timer and input temporary variables
    private float stunTimer = 0.0f;
    private float dashTimer = 0.0f;
    private float projectileResetTimer = 0.0f;
    private float verticalInput = 0.0f;
    private float horizontalInput = 0.0f;
    
    //This object's rigidbody
    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        //Fetch rigidbody
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("This object does not contain a RigidBody!");

		myState = PlayerStates.psNeutral;
		
        combatSettings.currHealth = combatSettings.maxHealth;
        combatSettings.currSuper = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //Retrieve input
        GetInput();

        switch(myState){
            case PlayerStates.psStunned:
                StunRundown();
            break;

            case PlayerStates.psDashRecovery:
                DashRundownTimer();
            break;

            case PlayerStates.psProjectileRecovery:
                ProjectileRundownTimer();
            break;
        }

        //Update projectile throwing function
        if (myState != PlayerStates.psProjectileRecovery && myState != PlayerStates.psStunned)
            ThrowProjectile();
    }

    void FixedUpdate()
    {
        //Execute movement based on input
        //If player is not stunned, allow movement otherwise, increment timer and reset the variables
        if(CanMove())
            Movement();
    }

    //Take input here
    void GetInput()
    {
        //Take movement inputs from the player
        if(Grounded())
            horizontalInput = Input.GetAxisRaw(inputSettings.HORIZONTAL_INPUT);
        else
            horizontalInput = Input.GetAxis(inputSettings.HORIZONTAL_INPUT);

        verticalInput = Input.GetAxisRaw(inputSettings.VERTICAL_INPUT);

        //Take combat input here
        attackInput = Input.GetButtonDown(inputSettings.PROJECTILE_INPUT);
        dashInput = Input.GetButtonDown(inputSettings.DASH_INPUT);
    }

    //Move the player vertically and horizontally.
    void Movement()
    {
        //Play audio and jump if grounded + vertical input
        if (verticalInput > 0.5f && Grounded())
        {
            //audioCont.Play(audioClip.jumped);
            rigidBody.AddForce(Vector3.up * moveSettings.jumpVelocity);
        }

        if(dashInput && (myState != PlayerStates.psDashRecovery || myState != PlayerStates.psDashing))
        {
            Vector3 dashDir = (new Vector3(transform.position.x + horizontalInput, transform.position.y + verticalInput, transform.position.z) - transform.position); 
            StartCoroutine("dash",dashDir);
            //audioCont.Play(audioClip.dashed);
        }

        //Move the Object's X position here
        rigidBody.MovePosition(transform.position + (Camera.main.transform.right * (horizontalInput * moveSettings.moveSpeed * Time.deltaTime)));
    }

    void ThrowProjectile()
    {
        //On attack input, instantiate the projectile
        if (attackInput)
        {
			var infoPack = new ProjectileInfoPack();
			infoPack.thisObject = this.gameObject;
			infoPack.enemyObjectPos = otherPlayer.transform.position;
			infoPack.newSpeed = combatSettings.projectileSpeed;
			
            //Instantiate as Gameobject to alter projectile variables
            GameObject thisProj = Instantiate(combatSettings.projectile, projectilePoint.transform.position, projectilePoint.transform.rotation) as GameObject;
			thisProj.GetComponent<Projectile>().SetInformation(infoPack);
			thisProj.GetComponent<Projectile>().StartManual();
						
            myState = PlayerStates.psProjectileRecovery;
        }
    }

    //Check that the object is close to the ground
    bool Grounded()
    {
        //Return the result of a raycast casting from underneath the object to the layermask specified
        return Physics.Raycast(anchorPoint.transform.position, Vector3.down, distToGround, moveSettings.ground);
    }

    bool CanMove(){
        if((myState == PlayerStates.psNeutral) || (myState == PlayerStates.psDashing) || (myState == PlayerStates.psDashRecovery))
            return true;

            return false;
    }

    //All timers for bool resets
    void StunRundown()
    {
        //Increment stun timer until it is greater than stun time, reset timer and allow the player to move.
        if (stunTimer <= combatSettings.stunTime)
            stunTimer += Time.deltaTime;
        else {
            stunTimer = 0.0f;
            myState = PlayerStates.psNeutral;
        }
    }

    void ProjectileRundownTimer()
    {
        //Increment projectile reset timer until it is greater than stun time, reset timer and allow the player to throw another projectile
        if (projectileResetTimer <= combatSettings.projectileResetTime)
            projectileResetTimer += Time.deltaTime;
        else {
            projectileResetTimer = 0.0f;
            myState = PlayerStates.psNeutral;
        }
    }

    void DashRundownTimer()
    {
        //Increment dash reset timer until it is greater than dash time, reset timer and allow the player to dash.
        if (dashTimer <= combatSettings.dashResetTime)
            dashTimer += Time.deltaTime;
        else
        {
            dashTimer = 0.0f;
            myState = PlayerStates.psNeutral;
        }
    }

    //Take away health equal to the parameter given
    void TakeDamage(float damage)
    {
        combatSettings.currHealth -= damage;
    }

    void OnCollisionEnter(Collision other)
    {
        //On collision with another player, if the other player is dashing, take damage from them
        if(other.gameObject.tag == "Character")
        {
            if (other.gameObject.GetComponent<CharacterController>().myState == CharacterController.PlayerStates.psDashing)
                TakeDamage(other.gameObject.GetComponent<CharacterController>().combatSettings.myDamage);
        }
    }

    void OnCollisionStay(Collision other){
        if(myState == CharacterController.PlayerStates.psDashing && other.gameObject != otherPlayer)
            myState = PlayerStates.psDashRecovery;
    }

    IEnumerator dash(Vector3 direction)
    {
        myState = PlayerStates.psDashing;

        Vector3 startPos = transform.position;
        Vector3 tarPos = startPos + (direction * moveSettings.dashDist);

        int dashDuration = (int)(moveSettings.dashDist / moveSettings.dashSpeed);

        for (int f = 0; f < dashDuration; f++)
        {
            if(myState != CharacterController.PlayerStates.psDashing){
                yield break;
                myState = PlayerStates.psDashRecovery;
            }

            transform.position = Vector3.Lerp(startPos, tarPos, moveSettings.dashAniCurve.Evaluate((float)f / dashDuration));
            yield return null;
        }

        myState = PlayerStates.psDashRecovery;
    }
}
