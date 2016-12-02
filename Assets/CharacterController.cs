using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float moveSpeed = 5.0f;


    //MovementVariables
    private float horizontalInput = 0.0f;
    private float verticalInput = 0.0f;
    private float distToGround = 0.1f;

    bool isGrounded = false;

    public LayerMask ground;
    public GameObject anchorPoint;
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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void movement()
    {
        //Move the Object's X position here
        rigidBody.MovePosition(transform.position + (transform.right * (horizontalInput * moveSpeed * Time.deltaTime)));

        //move the object's Y position here
        if (verticalInput > 0 && grounded())
            rigidBody.AddForce(Vector3.up * 20);
    }

    bool grounded()
    {
        return Physics.Raycast(anchorPoint.transform.position, Vector3.down, distToGround, ground);
    }
}
