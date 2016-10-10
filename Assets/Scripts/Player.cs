using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private float maxSpeedX = 8f;
    private float moveForce = 15f;
    private float bulletForce = 20f;
    private float jumpForce = 300f;
    public int score = 0;
    public GameObject scoreText;
    public GameObject bullet;
    public Vector2 velocity;  // temporary, for debugging

    private bool jump = false;
    private bool grounded = false;
    private bool facingRight = true;
    private float shotDelay = 0.10f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform groundCheck;
    private Vector3 spawnPoint;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
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

        velocity = GetComponent<Rigidbody2D>().velocity;
    }


    private void FixedUpdate() {

        // Get horizontal input
        int h = (int) Input.GetAxisRaw("Horizontal");

        // Set Speed animator parameter
        animator.SetInteger("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        }

        if (h == 0) {
            GetComponent<Rigidbody2D>().velocity *= 0.975f;
        }

        // If the player's horizontal velocity is greater than the maxSpeedX...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
        }


        // Handle flipping sprite if change directions
        if (h > 0) { facingRight = true; }
        else if (h < 0) { facingRight = false; }
        spriteRenderer.flipX = !facingRight;


        // Handle jumping
        if (jump) {
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

    }


    private IEnumerator ShootTimer() {
        while (true) {
            if (Input.GetButton("Fire1")) {
                Fire();
                yield return new WaitForSeconds(shotDelay);
            }
            else {
                yield return null;
            }
        }
    }


    private void Fire() {
        // Create bullet instance near player's current position.
        Vector3 bulletOffset;
        Vector2 bulletVelocity;
        Quaternion bulletQuaternion;

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

        Vector3 newBulletPosition = transform.position + bulletOffset;
        GameObject bulletInstance = Instantiate(bullet, newBulletPosition, bulletQuaternion) as GameObject;
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bulletInstance.transform.SetParent(this.transform);
    }

}
