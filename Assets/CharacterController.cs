using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    //Anchorpoint of character
    public GameObject anchorPoint;

    [System.Serializable]
    public class MoveMentSettings
    {
        //Variables that relate to the movement of a character
        public float moveSpeed = 5.0f;
        public float gravity = 9.8f;
        public float jumpVelocity = 10.0f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class InputSettings
    {
        //Variables that take input from the player
        public string HORIZONTAL_INPUT = "Horizontal";
        public string VERTICAL_INPUT = "Vertical";
    }

    public MoveMentSettings moveSettings = new MoveMentSettings();
    public InputSettings inputSettings = new InputSettings();

    //Input Temp Variables
    private float horizontalInput = 0.0f;
    private float verticalInput = 0.0f;


    private float Yvelocity = 0.0f;
    private float Ydisplacement = 0.0f;
    private float distToGround = 0.1f;
    
    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        //Fetch rigidbody
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("This object does not contain a RigidBody!");
	}
	
	// Update is called once per frame
	void Update () {
        //Retrieve input
        getInput();
	}

    void FixedUpdate()
    {
        //Execute movement based on input
        movement();
    }

    void getInput()
    {
        //Take movement input here
        if(grounded())
            horizontalInput = Input.GetAxisRaw(inputSettings.HORIZONTAL_INPUT);
        else
            horizontalInput = Input.GetAxis(inputSettings.HORIZONTAL_INPUT);

        verticalInput = Input.GetAxisRaw(inputSettings.VERTICAL_INPUT);
    }

    void movement()
    {
        if (verticalInput > 0 && grounded())
            rigidBody.AddForce(Vector3.up * moveSettings.jumpVelocity);

        //Move the Object's X position here
        rigidBody.MovePosition(transform.position + (transform.right * (horizontalInput * moveSettings.moveSpeed * Time.deltaTime)));
    }

    bool grounded()
    {
        //Return the result of a raycast casting from underneath the object to the layermask specified
        return Physics.Raycast(anchorPoint.transform.position, Vector3.down, distToGround, moveSettings.ground);
    }
}
