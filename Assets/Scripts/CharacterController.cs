﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    //Anchorpoint of character
    public GameObject anchorPoint;
    public GameObject projectilePoint;
    GameObject otherPlayer;

    //Changeable variables that manage movement
    [System.Serializable]
    public class MoveMentSettings
    {
        //Variables that relate to the movement of a character
        public float moveSpeed = 5.0f;
        public float gravity = 9.8f;
        public float jumpVelocity = 10.0f;
        public float dashSpeed = 1.0f;
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

        //Character Projectile times
        public GameObject projectile;
        public float projectileSpeed = 6.0f;
        public float projectileResetTime = 1.0f;

        //Character dashing variables
        public float dashTime = 0.75f;
    }

    //Public variable classes initialisation
    public MoveMentSettings moveSettings = new MoveMentSettings();
    public InputSettings inputSettings = new InputSettings();
    public CombatSettings combatSettings = new CombatSettings();

    //Input Temp Variables
    private float horizontalInput = 0.0f;
    private float verticalInput = 0.0f;
    private bool dashInput = false;
    private bool attackInput = false;
    private bool canAttack = true;
    private bool canDash = true;

    private bool facingRight;

    private float distToGround = 0.1f;
    private float stunTimer = 0.0f;
    private float projectileResetTimer = 0.0f;
    private float dashTimer = 0.0f;

    //This object's rigidbody
    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        //Fetch rigidbody
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("This object does not contain a RigidBody!");

        otherPlayer = GameObject.FindGameObjectWithTag("Character");
	}
	
	// Update is called once per frame
	void Update () {
        //Retrieve input
        getInput();

        if (otherPlayer.transform.position.x > transform.position.x)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
        //Update projectile throwing function
        if (canAttack && !combatSettings.isStunned)
            throwProjectile();
        else if (projectileResetTimer <= combatSettings.projectileResetTime)
            projectileResetTimer += Time.deltaTime;
        else {
            projectileResetTimer = 0.0f;
            canAttack = true;
        }
    }

    void FixedUpdate()
    {
        //Execute movement based on input
        //If player is not stunned, allow movement otherwise, increment timer and reset the variables
        if (!combatSettings.isStunned)
            movement();
        else if (stunTimer <= combatSettings.stunTime)
            stunTimer += Time.deltaTime;
        else {
            stunTimer = 0.0f;
            combatSettings.isStunned = false;
        }
    }

    void getInput()
    {
        //Take movement input here
        if(grounded())
            horizontalInput = Input.GetAxisRaw(inputSettings.HORIZONTAL_INPUT);
        else
            horizontalInput = Input.GetAxis(inputSettings.HORIZONTAL_INPUT);

        verticalInput = Input.GetAxisRaw(inputSettings.VERTICAL_INPUT);

        //Take combat input here
        attackInput = Input.GetKeyDown(inputSettings.PROJECTILE_INPUT);
        dashInput = Input.GetKeyDown(inputSettings.DASH_INPUT);
    }

    void movement()
    {
        if (verticalInput > 0 && grounded())
            rigidBody.AddForce(Vector3.up * moveSettings.jumpVelocity);

        //Move the Object's X position here
        rigidBody.MovePosition(transform.position + (Camera.main.transform.right * (horizontalInput * moveSettings.moveSpeed * Time.deltaTime)));
    }

    void throwProjectile()
    {
        //On attack input, instantiate the projectile
        if (attackInput)
        {
            GameObject thisProj = Instantiate(combatSettings.projectile, projectilePoint.transform.position, projectilePoint.transform.rotation) as GameObject;

            if (!facingRight)
                thisProj.GetComponent<Projectile>().projVars.moveSpeed *= -combatSettings.projectileSpeed;
            else
                thisProj.GetComponent<Projectile>().projVars.moveSpeed *= combatSettings.projectileSpeed;
            canAttack = false;
        }
    }

    bool grounded()
    {
        //Return the result of a raycast casting from underneath the object to the layermask specified
        return Physics.Raycast(anchorPoint.transform.position, Vector3.down, distToGround, moveSettings.ground);
    }
}
