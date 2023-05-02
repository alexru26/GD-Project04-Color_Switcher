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

    private bool jumped = false;
    private bool grounded = true;

    [Header("References")]
    private Rigidbody2D rb;

    void Start()
    {
        // Start by setting up stuff
        rb = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        input();
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
            jumped = true;
        }
        else
        {
            ground();
        }

        if(!grounded && jumped)
        {
            if(!Input.GetButton("Jump"))
            {
                rb.gravityScale = 5f;
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
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, whatIsGround);

        if(grounded)
        {
            rb.gravityScale = 2f;
        }
    }

    void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
