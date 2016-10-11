using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private float maxSpeedX = 10f;
    private float moveForce = 15f;
    private float bulletForce = 20f;
    private float shotDelay = 0.08f;
    private float jumpForce = 400f;
    public int score = 0;
    public GameObject scoreText;
    public GameObject bullet;
    public Vector2 velocity; // temporary, for debugging

    private bool jump = false;
    private bool grounded = false;
    private bool facingRight = true;
    private bool fireUp = false;
    private bool clickDraggingPlayer = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform groundCheck;
    private Transform clickCheck;
    private Vector3 spawnPoint;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        clickCheck = transform.Find("ClickCheck");
        spawnPoint = transform.position;
        StartCoroutine("ShootTimer");
    }

    private void Update() {

        // Can jump off of platforms, enemies, or objects
        bool grounded1 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("PlatformLayer"));
        bool grounded2 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ObjectLayer"));
        bool grounded3 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("EnemyLayer"));
        grounded = grounded1 || grounded2 || grounded3;
        animator.SetBool("Grounded", grounded);
        if (grounded && Input.GetButtonDown("Jump")) {
            jump = true;
        }

        // Respawn if fallen off the world
        if (transform.position.y <= -10) {
            transform.position = spawnPoint;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            facingRight = true;
        }

        // Just to set the public velocity variable to view in Unity inspector
        velocity = GetComponent<Rigidbody2D>().velocity;

        // Visualize ClickCheck box collider when clicked
        Bounds clickBoxBounds = clickCheck.GetComponent<BoxCollider2D>().bounds;
        if (clickBoxBounds.Contains(getMouseWorldPosition())) {
            clickCheck.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            clickCheck.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    private void FixedUpdate() {

        // Get horizontal input
        int h = (int) Input.GetAxisRaw("Horizontal");
        Move(h);

        // Player follows mouse if first click on player and continue holding down
        if (clickDraggingPlayer) {
            Vector3 diff = getMouseWorldPosition() - transform.position;
            Debug.Log("diff: " + diff);
            if      (diff.x > 0) { Move(1); }
            else if (diff.x < 0) { Move(-1); }
            else                 { Move(0); }
        }

        // Handle jumping
        if (jump) {
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    private void OnMouseDown() {
        clickDraggingPlayer = true;
        spriteRenderer.color = Color.red;
    }

    private void OnMouseUp() {
        clickDraggingPlayer = false;
        spriteRenderer.color = Color.white;
    }

    private void Move(int h) {

        // Set Speed animator parameter
//        animator.SetInteger("Speed", (int) Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        animator.SetInteger("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        }

        // Slows down player slightly faster
        if (h == 0) {
            GetComponent<Rigidbody2D>().velocity *= 0.975f;
        }

        // If the player's horizontal velocity is greater than the maxSpeedX...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Handle flipping sprite if change directions
        if (h > 0) {
            facingRight = true;
        } else if (h < 0) {
            facingRight = false;
        }
        spriteRenderer.flipX = !facingRight;
    }

    private IEnumerator ShootTimer() {
        while (true) {
            if (Input.GetButton("Fire1")) {
                fireUp = (int) Input.GetAxisRaw("Vertical") == 1;
                Fire();
                yield return new WaitForSeconds(shotDelay);
            } else {
                yield return null;
            }
        }
    }

    private void Fire() {
        // Create bullet instance near player's current position.
        Vector3 bulletOffset;
        Vector2 bulletVelocity;
        Quaternion bulletQuaternion;

        if (fireUp) {
            bulletOffset = new Vector3(0, 1f, 0);
            bulletVelocity = new Vector2(0, bulletForce);
            bulletQuaternion = Quaternion.identity;
        }
        else {
            if (facingRight) {
                bulletOffset = new Vector3(0.5f, 0, 0);
                bulletVelocity = new Vector2(bulletForce, 0);
                bulletQuaternion = Quaternion.AngleAxis(270, Vector3.forward);
            }
            else {
                bulletOffset = new Vector3(-0.5f, 0, 0);
                bulletVelocity = new Vector2(bulletForce * -1, 0);
                bulletQuaternion = Quaternion.AngleAxis(90, Vector3.forward);
            }
        }

        Vector3 newBulletPosition = transform.position + bulletOffset;
        GameObject bulletInstance = Instantiate(bullet, newBulletPosition, bulletQuaternion) as GameObject;
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bulletInstance.transform.SetParent(this.transform);
    }

    void OnCollisionEnter2D(Collision2D coll) {
//        Debug.Log("Player collided with " + coll.gameObject.name);
    }

    private Vector3 getMouseWorldPosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

}
