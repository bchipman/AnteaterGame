using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private const float MaxSpeedX = 10f;
    private const float MoveForce = 15f;
    private const float JumpForce = 400f;

    private bool jump = false;
    private bool jumpedRecently = false;
    private bool grounded = false;
    private bool facingRight = true;
    private bool fireUp = false;
    private bool clickDraggingPlayer = false;
    private bool mouseInsideClickCheckBox = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform clickCheck;
    private Vector3 spawnPoint;
    public GameManager gameManager;
    private EdgeCollider2D bottomEdgeCollider;

    public Vector3 mousePositionWhenClickedPlayer;
    public Vector3 mousePositionNow;
    public Vector2 velocity; // temporary, for debugging
    public int xDirection = 0;
    public float yMouseVelocityInLastSec = 0;
    private Queue<List<float>> mousePositionQueue;
	private int currentHealth = 10;
	private int maxHealth = 10;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        clickCheck = transform.Find("ClickCheck");
        spawnPoint = transform.position;
        StartCoroutine(ShootTimer());
        mousePositionQueue = new Queue<List<float>>();
        bottomEdgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Check input in Update and set flags to be acted on in FixedUpdate
    private void Update() {

        // Save mouse position to be used in FixedUpdate
        mousePositionNow = Input.mousePosition;

        // Just setting some public variables to view in Unity inspector
        velocity = GetComponent<Rigidbody2D>().velocity;
        
        // Set grounded flag - can jump off of platforms, enemies, or objects
        bool grounded1 = bottomEdgeCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("PlatformLayer"));
        bool grounded2 = bottomEdgeCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("ObjectLayer"));
        bool grounded3 = bottomEdgeCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("EnemyLayer"));
        grounded = grounded1 || grounded2 || grounded3;

        // Set mouseInsideClickCheckBox flag
        mouseInsideClickCheckBox = clickCheck.GetComponent<BoxCollider2D>().bounds.Contains(GetMouseWorldPosition());
        
        // Display white clickCheck box around player when mouse is inside click check box
//        clickCheck.GetComponent<SpriteRenderer>().enabled = mouseInsideClickCheckBox;


        // Horizontal movement
        int xDirectionTemp1 = (int) Input.GetAxisRaw("Horizontal");
        int xDirectionTemp2 = 0;
        if (grounded && clickDraggingPlayer && !mouseInsideClickCheckBox) {
            Vector3 diff = GetMouseWorldPosition() - transform.position;
            if      (diff.x > 0) { xDirectionTemp2 = 1;  }
            else if (diff.x < 0) { xDirectionTemp2 = -1; }
            else                 { xDirectionTemp2 = 0;  }
        }
        xDirection = xDirectionTemp2 == 0 ? xDirectionTemp1 : xDirectionTemp2;  // use direction from kb input if direction not set from mouse


        // Jumping
        jump = grounded && Input.GetButton("Jump");  // set jump flag from kb input.  this may get overriden in FixedUpdate
    }

    private void FixedUpdate() {

        // Add data pt to mousePositionQueue, check to see how far mouse has moved in last 0.5sec, if above some threshold (125), set jump flag
        float time = Time.time;
        float yMousePosNow = mousePositionNow.y;
        if (time.ToString("F2").EndsWith("0")) {  // only save data pts every 100ms instead of every 20ms
            mousePositionQueue.Enqueue(new List<float> { time, yMousePosNow });
            if (mousePositionQueue.Count >= 5) {  // 5 = 500ms,  10 = 1sec
                yMouseVelocityInLastSec = yMousePosNow - mousePositionQueue.Dequeue()[1];
                if (grounded && clickDraggingPlayer && yMouseVelocityInLastSec > 125) {  // some arbitrary threshold
                        jump = true;
                }
            }
        }

        // Animation parameters
        animator.SetBool("Grounded", grounded);
        animator.SetInteger("Speed", Mathf.Abs(xDirection));

        // Horizontal movement
        Move(xDirection);

        // Jumping
        if (jump && !jumpedRecently) {
            StartCoroutine(JumpedRecentlyTimer());
            jump = false;
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, JumpForce));
//            Debug.Log("added jump force!  grounded:" + grounded + "  jump:" + jump + "  realtimeSinceStartup:" + Time.realtimeSinceStartup);
        }


        // Respawn if fallen off the world
        if (transform.position.y <= -10) {
            Respawn();
        }
    }

    private IEnumerator JumpedRecentlyTimer() {
        jumpedRecently = true;
        yield return new WaitForSeconds(0.25f);
        jumpedRecently = false;
    }

    private void OnMouseDown() {
        clickDraggingPlayer = true;
        spriteRenderer.color = Color.red;
        mousePositionWhenClickedPlayer = Input.mousePosition;
    }

    private void OnMouseUp() {
        clickDraggingPlayer = false;
        spriteRenderer.color = Color.white;
    }

    private void Move(int h) {

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < MaxSpeedX) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * MoveForce);
        }

        // Slows down player slightly faster
        if (h == 0) {
//            GetComponent<Rigidbody2D>().velocity *= 0.975f;
        }

        // If the player's horizontal velocity is greater than the maxSpeedX...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > MaxSpeedX) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * MaxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
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
            if (Input.GetMouseButton(0) && !clickDraggingPlayer) {
                GameObject projectileInstance = Instantiate(gameManager.GetCurrentProjectileType());
                if (projectileInstance.gameObject.name.StartsWith("Dirt")) {
                    FireTowardMouseInArc(projectileInstance);
                } else {
                    FireTowardMouse(projectileInstance);
                }
                yield return new WaitForSeconds(projectileInstance.GetComponent<Projectile>().shotDelay);
            } else if (Input.GetButton("Fire1")) {
                GameObject projectileInstance = Instantiate(gameManager.GetCurrentProjectileType());
                fireUp = (int) Input.GetAxisRaw("Vertical") == 1;
                Fire(projectileInstance);
                yield return new WaitForSeconds(projectileInstance.GetComponent<Projectile>().shotDelay);
            } else {
                yield return null;
            }
        }
    }

    private void Fire(GameObject bulletInstance) {
        Vector3 bulletOffset;

        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y, 0);
        Vector2 xyVectorScale = new Vector2(1, 1);
        float degs = 0;

        if (fireUp) {
            bulletOffset = new Vector3(0, 1f, 0);
            degs = 90;
            xyVectorScale = new Vector2(0, 1);
        }
        else {
            if (facingRight) {
                bulletOffset = new Vector3(0.5f, 0, 0);
                degs = 0;
                xyVectorScale = new Vector2(1, 0);
            }
            else {
                bulletOffset = new Vector3(-0.5f, 0, 0);
                degs = 180;
                xyVectorScale = new Vector2(-1, 0);
            }
        }

        float bulletForce = bulletInstance.GetComponent<Projectile>().projectileForce;
        int angleOffset = bulletInstance.GetComponent<Projectile>().angleOffset;
        Vector2 bulletVelocity = xyVectorScale * bulletForce;
        Quaternion bulletQuaternion = Quaternion.AngleAxis(angleOffset + degs, Vector3.forward);

        bulletInstance.transform.position = playerPos + bulletOffset;
        bulletInstance.transform.rotation = bulletQuaternion;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bulletInstance.transform.SetParent(transform.Find("/Projectiles"));
    }

    private void FireTowardMouse(GameObject bulletInstance) {
        Vector3 bulletOffset = Vector3.zero;

        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y, 0);
        Vector2 xyVectorScale = (GetMouseWorldPosition() - playerPos).normalized;
        float degs = Mathf.Atan(xyVectorScale.y / xyVectorScale.x) * Mathf.Rad2Deg;

        // Quadrant I
        if (xyVectorScale.x > 0 && xyVectorScale.y > 0) {
            // do nothing
        }

        // Quadrant II
        else if (xyVectorScale.x < 0 && xyVectorScale.y > 0) {
            degs = 180 + degs;
        }

        // Quadrant III
        else if (xyVectorScale.x < 0 && xyVectorScale.y < 0) {
            degs = 180 + degs;
        }

        // Quadrant IV
        else if (xyVectorScale.x > 0 && xyVectorScale.y < 0) {
            degs = 360 + degs;
        }

        float bulletForce = bulletInstance.GetComponent<Projectile>().projectileForce;
        int angleOffset = bulletInstance.GetComponent<Projectile>().angleOffset;
        Vector2 bulletVelocity = xyVectorScale * bulletForce;
        Quaternion bulletQuaternion = Quaternion.AngleAxis(angleOffset + degs, Vector3.forward);

        bulletInstance.transform.position = playerPos + bulletOffset;
        bulletInstance.transform.rotation = bulletQuaternion;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bulletInstance.transform.SetParent(transform.Find("/Projectiles"));
    }

    private void FireTowardMouseInArc(GameObject dirtInstance) {
        Vector3 originPos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y, 0);
        Vector3 targetPos = GetMouseWorldPosition();
        float flightTime = 1.5f;  // in seconds
        float g = Mathf.Abs(Physics2D.gravity.y);  // gravity
        float xVel = (targetPos.x - originPos.x) / flightTime;
        float yVel = (targetPos.y + 0.5f * g * flightTime * flightTime - originPos.y) / flightTime;

        dirtInstance.transform.position = originPos;
        dirtInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(xVel, yVel, 0f);
        dirtInstance.transform.SetParent(transform.Find("/Projectiles"));
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

	private void OnCollisionEnter2D(Collision2D coll) {
		if (currentHealth > 0) {
			if (coll.gameObject.layer == LayerMask.NameToLayer ("EnemyLayer")) {
				currentHealth--;
				transform.Find ("/Player/HealthBar/GreenHealthBarBox").localScale = new Vector3 ((float)currentHealth / maxHealth, 0.55f, 0);
			    if (currentHealth <= 0) {
                    Respawn();
			    }
			}
		}
	}

    private void Respawn() {
        gameManager.DisplayDeathText();
        transform.position = spawnPoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        facingRight = true;
        currentHealth = maxHealth;
		transform.Find("/Player/HealthBar/GreenHealthBarBox").localScale = new Vector3(0.868968f, 0.55f, 0);
		//transform.Find("HealthBar").localScale = new Vector3((float)maxHealth / maxHealth, 1f, 0);
    }

}
