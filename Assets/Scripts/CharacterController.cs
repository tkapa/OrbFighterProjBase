﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    //Various points in the scenes
    public GameObject anchorPoint;
    public GameObject otherPlayer; 
    public GameObject projectilePoint;

    //Changeable variables that manage movement
    [System.Serializable]
    public class MoveMentSettings
    {
        //Variables that relate to the movement of a character
        public float moveSpeed = 5.0f;
        public float gravity = 9.8f;
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
    }

    //Changeable variables that manage combat
    [System.Serializable]
    public class CombatSettings
    {
        //Character stun variables
        public bool isStunned = false;
        public float stunTime = 0.75f;
        public float myDamage = 10.0f;

        //Character Projectile times
        public GameObject projectile;
        public float projectileSpeed = 6.0f;
        public float projectileResetTime = 1.0f;

        //Character dashing variables
        public float dashTime = 0.75f;

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

    //Public variable classes initialisation
    public MoveMentSettings moveSettings = new MoveMentSettings();
    public InputSettings inputSettings = new InputSettings();
    public CombatSettings combatSettings = new CombatSettings();
    public audioClips audioClip = new audioClips();

    //Bools that govern input and action
    private bool dashInput = false;
    private bool attackInput = false;
    private bool facingRight;
    private bool canDash = true;
    private bool canAttack = true;
    private bool isDashing = false;

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

        combatSettings.currHealth = combatSettings.maxHealth;
        combatSettings.currSuper = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //Retrieve input
        getInput();

        //Update projectile throwing function
        if (canAttack && !combatSettings.isStunned)
            throwProjectile();
        else
            projectileRundownTimer();

        if (!canDash && !isDashing)
            dashRundownTimer();        
    }

    void FixedUpdate()
    {
        //Execute movement based on input
        //If player is not stunned, allow movement otherwise, increment timer and reset the variables
        if (!combatSettings.isStunned)
            movement();
        else
            stunRundown();
    }

    //Take input here
    void getInput()
    {
        //Take movement inputs from the player
        if(grounded())
            horizontalInput = Input.GetAxisRaw(inputSettings.HORIZONTAL_INPUT);
        else
            horizontalInput = Input.GetAxis(inputSettings.HORIZONTAL_INPUT);

        verticalInput = Input.GetAxisRaw(inputSettings.VERTICAL_INPUT);

        //Take combat input here
        attackInput = Input.GetButtonDown(inputSettings.PROJECTILE_INPUT);
        dashInput = Input.GetButtonDown(inputSettings.DASH_INPUT);
    }

    //Move the player vertically and horizontally.
    void movement()
    {
        //Play audio and jump if grounded + vertical input
        if (verticalInput > 0.5f && grounded())
        {
            //audioCont.Play(audioClip.jumped);
            rigidBody.AddForce(Vector3.up * moveSettings.jumpVelocity);
        }

        if(dashInput && canDash)
        {
            isDashing = true;
            Vector3 dashDir = (new Vector3(transform.position.x + horizontalInput, transform.position.y + verticalInput, transform.position.z) - transform.position); 
            StartCoroutine("dash",dashDir);
            //audioCont.Play(audioClip.dashed);
        }

        //Move the Object's X position here
        rigidBody.MovePosition(transform.position + (Camera.main.transform.right * (horizontalInput * moveSettings.moveSpeed * Time.deltaTime)));
    }

    void throwProjectile()
    {
        //On attack input, instantiate the projectile
        if (attackInput)
        {
            //Instantiate as Gameobject to alter projectile variables
            GameObject thisProj = Instantiate(combatSettings.projectile, projectilePoint.transform.position, projectilePoint.transform.rotation) as GameObject;

            //Ensure the projectile can't hit this object
            thisProj.GetComponent<Projectile>().myParent = this.gameObject;
            thisProj.GetComponent<Projectile>().enemyPos = otherPlayer.transform.position;

            //Play audio clip
            //audioCont.Play(audioClip.projectiled);

            canAttack = false;
        }
    }

    bool grounded()
    {
        //Return the result of a raycast casting from underneath the object to the layermask specified
        return Physics.Raycast(anchorPoint.transform.position, Vector3.down, distToGround, moveSettings.ground);
    }

    void stunRundown()
    {
        //Increment stun timer until it is greater than stun time, reset timer and allow the player to move.
        if (stunTimer <= combatSettings.stunTime)
            stunTimer += Time.deltaTime;
        else {
            stunTimer = 0.0f;
            combatSettings.isStunned = false;
        }
    }

    void projectileRundownTimer()
    {
        //Increment projectile reset timer until it is greater than stun time, reset timer and allow the player to throw another projectile
        if (projectileResetTimer <= combatSettings.projectileResetTime)
            projectileResetTimer += Time.deltaTime;
        else {
            projectileResetTimer = 0.0f;
            canAttack = true;
        }
    }

    void dashRundownTimer()
    {
        //Increment dash reset timer until it is greater than dash time, reset timer and allow the player to dash.
        if (dashTimer <= combatSettings.dashTime)
            dashTimer += Time.deltaTime;
        else
        {
            dashTimer = 0.0f;
            canDash = true;
        }
    }

    void takeDamage(float damage)
    {
        combatSettings.currHealth -= damage;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Character")
        {
            if (other.gameObject.GetComponent<CharacterController>().isDashing)
                takeDamage(other.gameObject.GetComponent<CharacterController>().combatSettings.myDamage);
            else if (isDashing)
                other.gameObject.GetComponent<CharacterController>().takeDamage(combatSettings.myDamage);
        }
    }

    IEnumerator dash(Vector3 direction)
    {
        canDash = false;
        Vector3 startPos = transform.position;
        Vector3 tarPos = startPos + (direction * moveSettings.dashDist);

        int dashDuration = (int)(moveSettings.dashDist / moveSettings.dashSpeed);

        for (int f = 0; f < dashDuration; f++)
        {
            transform.position = Vector3.Lerp(startPos, tarPos, moveSettings.dashAniCurve.Evaluate((float)f / dashDuration));
            yield return null;
        }

        isDashing = false;
    }
}
