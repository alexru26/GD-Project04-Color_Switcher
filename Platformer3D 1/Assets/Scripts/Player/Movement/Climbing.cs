using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Rigidbody myRigidBody;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Walljump")]
    public float wallJumpUpForce;
    public float wallJumpBackForce;
    private bool canWallJump = true;
    public float wallJumpCooldown;

    public int wallJumps;
    private int wallJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    private void Update() 
    {
        WallCheck();
        StateMachine();

        if(climbing && !exitingWall) ClimbingMovement();
    }  

    private void StateMachine()
    {
        if(wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if(!climbing && climbTimer > 0) StartClimbing();

            if(climbTimer > 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }

        else if(exitingWall)
        {
            if(climbing) StopClimbing();

            if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if(exitWallTimer < 0) exitingWall = false;
        }

        else
        {
            if(climbing) StopClimbing();
        }

        if(wallFront && Input.GetKey(KeyCode.Space) && wallJumpsLeft > 0 && !pm.grounded && canWallJump) 
        {
            Walljump();
            Invoke(nameof(resetWalljump), wallJumpCooldown);
        }
    }

    void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            wallJumpsLeft = wallJumps;
        }
    }

    void StartClimbing()
    {
        climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    void ClimbingMovement()
    {
        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, climbSpeed, myRigidBody.velocity.z);
    }

    void StopClimbing()
    {
        climbing = false;
    }

    void Walljump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * wallJumpUpForce + frontWallHit.normal * wallJumpBackForce;

        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 0f, myRigidBody.velocity.z);
        myRigidBody.AddForce(forceToApply, ForceMode.Impulse);

        wallJumpsLeft -= 1; 

        canWallJump = false;
    }    

    void resetWalljump()
    {
        canWallJump = true;
    }

}
