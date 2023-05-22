using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    private float horizontalInput;
    private float verticalInput;

    private float lastMove = 1;

    [Header("Jump")]
    public float jumpForce;
    public LayerMask whatIsGround;

    private bool jumping = false;
    private bool grounded = true;

    [Header("References")]
    private Rigidbody2D rb;

    public GameObject red_door_1;
    public GameObject red_door_2;

    public GameObject blue_door_1;
    public GameObject blue_door_2;

    private bool teleport = true;

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
        ground();
        checkDirection();

        if(transform.position.y < -20)
        {
            respawn();
        }
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
        transform.position = new Vector2(-19f, 1f);
    }

    void checkDirection()
    {
        transform.localScale = new Vector3(lastMove, 1, 1);
        if(horizontalInput != 0)
        {
            lastMove = horizontalInput;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.transform.parent.name == "spikes")
        {
            respawn();
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if(Input.GetButtonDown("Submit") && col.gameObject.CompareTag("black_door"))
        {
            SceneManager.LoadScene((int.Parse(SceneManager.GetActiveScene().name) + 1).ToString());
        }
        else if(col.gameObject.transform.parent.parent.name == "door" && Input.GetButton("Submit") && teleport)
        {
            teleport = false;
            Invoke(nameof(resetTeleport), 1f);
            if(col.gameObject.transform.parent.name == "red doors" && col.gameObject.name[9] == '1')
            {
                transform.position = red_door_2.transform.position;
            }
            else if(col.gameObject.transform.parent.name == "red doors")
            {
                transform.position = red_door_1.transform.position;
            }
            else if(col.gameObject.transform.parent.name == "blue doors" && col.gameObject.name[10] == '1')
            {
                transform.position = blue_door_2.transform.position;
            }
            else if(col.gameObject.transform.parent.name == "blue doors")
            {
                transform.position = blue_door_1.transform.position;
            }
        }
    }

    void resetTeleport()
    {
        teleport = true;
    }
}