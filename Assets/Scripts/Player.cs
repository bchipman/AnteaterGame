using System;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 1f;
    public float maxSpeedX = 5f;

    public float moveForce = 10f;
    public float jumpForce = 0.01f;

    public bool facingRight = true;
    public bool jump = false;
    public bool grounded = false;
    public Vector2 velocity;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform groundCheck;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
    }


    void Update() {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("BlockingLayer"));
        animator.SetBool("Grounded", grounded);

        velocity = GetComponent<Rigidbody2D>().velocity;

        if (grounded && Input.GetButtonDown("Jump"))
//        if (grounded && Input.GetKey("space"))
            jump = true;
    }


    void FixedUpdate() {

        // Get horizontal input
        int h = (int) Input.GetAxisRaw("Horizontal");

        // Set Speed animator parameter
        animator.SetInteger("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        }

        // If the player's horizontal velocity is greater than the maxSpeedX...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX) {
            // ... set the player's velocity to the maxSpeedX in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Handle flipping sprite if change directions
        if (h > 0) {
            spriteRenderer.flipX = false;
        }
        else if (h < 0) {
            spriteRenderer.flipX = true;
        }

        if (jump) {
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

    }
}
