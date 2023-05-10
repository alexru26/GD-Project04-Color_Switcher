using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    private float horizontalInput;
    private float verticalInput;

    [Header("Jump")]
    public float jumpForce;
    public LayerMask whatIsGround;

    private bool jumping = false;
    private bool grounded = true;
    
    Animator anim;

    [Header("References")]
    private Rigidbody2D rb;

    void Start()
    {
        // Start by setting up stuff
        rb = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input();
        respawn();
        ground();
        checkDirection();
    }

    void FixedUpdate() 
    {
        movement();
    }

    void input()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && grounded)
        {
            jump();
            grounded = false;
            jumping = true;
        }

        if(!grounded)
        {
            if(!Input.GetButton("Jump"))
            {
                rb.gravityScale = 8f;
                jumping = false;
            }

            if(-0.5f < rb.velocity.y && rb.velocity.y< 0.5f && jumping)
            {
                rb.gravityScale = 6.5f;
            }
        }
    }

    void movement()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.AddForce(-1 * rb.velocity.normalized * Mathf.Pow(rb.velocity.magnitude, 2) * 0.7f);
    }

    void ground()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, whatIsGround);

        if(grounded)
        {
            rb.gravityScale = 5f;
        }
    }

    void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void respawn()
    {
        if(transform.position.y < -20)
        {
            transform.position = new Vector2(-19f, 1f);
        }
    }

    void checkDirection()
    {
        anim.SetFloat("Direction", horizontalInput);
    }
}
