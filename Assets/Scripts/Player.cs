using System;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 1f;
    public float maxSpeed = 5f;

    public float moveForce = 10f;
    public float jumpForce = 0.01f;

    public bool facingRight = true;
    public bool jump = false;
    public bool grounded = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;


    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update() {
//        if (grounded && Input.GetButtonDown("Jump"))
        if (grounded && Input.GetKey("space"))
            jump = true;
    }


    void FixedUpdate() {

        // Get horizontal input
        int h = (int) Input.GetAxisRaw("Horizontal");

        // Set Speed animator parameter
//        animator.SetInteger("Speed", Mathf.Abs(h));
        animator.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        }

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);


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

    private void OnCollisionEnter2D(Collision2D other) {
        // Player collided with something.  
        //  Do a raycast from player position straight downward.
        //  Check if raycast hit the same object
        //  If so, then you hit the top of a platform, so you can do the grounded stuff.
        //  Otherwise you hit the side or bottom of something.

        if (other.gameObject.layer == LayerMask.NameToLayer("BlockingLayer")) {
            String toPrint = "player hit " + other.collider.gameObject.name;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            if (hit) {
                toPrint += ", raycast hit " + hit.collider.gameObject.name;
                toPrint += ", " + other.collider.gameObject.name.Equals(hit.collider.gameObject.name);

                if (other.collider.gameObject.name.Equals(hit.collider.gameObject.name)) {
                    grounded = true;
                    animator.SetBool("Grounded", true);
                }

            }
            Debug.Log(toPrint);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {

        if (other.gameObject.layer == LayerMask.NameToLayer("BlockingLayer")) {
            String toPrint = "player exiting " + other.collider.gameObject.name;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            if (hit) {
                toPrint += ", raycast hit " + hit.collider.gameObject.name;
                toPrint += ", " + other.collider.gameObject.name.Equals(hit.collider.gameObject.name);

                if (other.collider.gameObject.name.Equals(hit.collider.gameObject.name)) {
                    grounded = false;
                    animator.SetBool("Grounded", false);
                }

            }
            Debug.Log(toPrint);
        }
    }
}
