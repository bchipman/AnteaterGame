using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed = 3f;
    public float maxSpeedX = 5f;
    public float moveForce = 10f;
    public float jumpForce = 400f;
    public int score = 0;
    public GameObject scoreText;
    public GameObject bullet;

    private bool jump = false;
    private bool grounded = false;
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
        // Can jump off of enemies, platforms, or objects
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
            spriteRenderer.flipX = false;
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


        Vector3 newBulletPosition = transform.position + new Vector3(0.5f, 0, 0);
        GameObject bulletInstance = Instantiate(bullet, newBulletPosition, Quaternion.AngleAxis(270, Vector3.forward)) as GameObject;
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(10f, 0f);
    }

    private void FixedUpdate() {

        // Get horizontal input
        int h = (int) Input.GetAxisRaw("Horizontal");

        // Set Speed animator parameter
        animator.SetInteger("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX) GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeedX...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX) GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);

        // Handle flipping sprite if change directions
        if (h > 0) { spriteRenderer.flipX = false; }
        else if (h < 0) { spriteRenderer.flipX = true; }

        if (jump) {
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

    }

}
