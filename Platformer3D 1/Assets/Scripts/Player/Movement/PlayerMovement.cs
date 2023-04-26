using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;   

    private bool running;
    
    public float walkingGroundDrag;
    public float runningGroundDrag;

    public float walkFOV;
    public float runFOV;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("References")]
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody myRigidBody;

    public Camera cam;

    private Vector3 spawn;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.freezeRotation = true; 
    }

    void Update()
    {
        input();
        ground();

        if(transform.position.y < -40)
        {
            transform.position = spawn;
        }

    }

    void FixedUpdate()
    {
        movement();
    }

    void input()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetButton("Run"))
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }

    void movement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            if(!running)
            {
                myRigidBody.AddForce(moveDirection.normalized * walkSpeed * 10f);
            }
            else if(running)
            {
                myRigidBody.AddForce(moveDirection.normalized * runSpeed * 10f);
            }
        }
        else if(!grounded)
        {
            if(!running)
            {
                myRigidBody.AddForce(moveDirection.normalized * walkSpeed * 10f * airMultiplier, ForceMode.Force);
            }
            else if(running)
            {
                myRigidBody.AddForce(moveDirection.normalized * runSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        if(running)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runFOV, 0.5f);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, walkFOV, 0.5f);
        }
    }

    void speedControl()
    {
        Vector3 flatVel = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);
        if(flatVel.magnitude > runSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            myRigidBody.velocity = new Vector3(limitedVel.x, myRigidBody.velocity.y, limitedVel.z);
        }
    }

    void ground()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if(grounded)
        {
            if(!running)
            {
                myRigidBody.drag = walkingGroundDrag;
            }
            else if(running)
            {
                myRigidBody.drag = runningGroundDrag;
            }
            
        }
        else
        {
            myRigidBody.drag = 0;
        }
    }

    void Jump()
    {
        //Reset y velocity
        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);

        myRigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "checkpoint")
        {
            spawn = other.gameObject.transform.position;
        }
    }

}
