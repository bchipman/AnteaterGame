using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private float maxSpeedX = 10f;
    private float moveForce = 15f;
    private float bulletForce = 20f;
    private float shotDelay = 0.08f;
    private float jumpForce = 400f;
    public int score = 0;
    public GameObject scoreText;
    public GameObject frameCount;
    public GameObject bullet;
    public Vector2 velocity; // temporary, for debugging
    public float mouseAngleFromPlayer;

    private bool jump = false;
    private bool grounded = false;
    private bool facingRight = true;
    private bool fireUp = false;
    public bool clickDraggingPlayer = false;
    private bool mouseInsideClickCheckBox = false;
    public float timeSinceClickedPlayer = 0;

    private float mouseAngleFromPlayerJumpThresholdLow = 90 - 40;
    private float mouseAngleFromPlayerJumpThresholdHigh = 90 + 40;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform groundCheck;
    private Transform clickCheck;
    private Vector3 spawnPoint;

    public int xDirection = 0;
    public int xDirectionTemp1 = 0;
    public int xDirectionTemp2 = 0;

    public int numUpdateCalls = 0;
    public int numFixedUpdateCalls = 0;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        clickCheck = transform.Find("ClickCheck");
        spawnPoint = transform.position;
        StartCoroutine("ShootTimer");
    }

    // Check input in Update and set flags to be acted on in FixedUpdate
    private void Update() {
        numUpdateCalls++;

        // Just setting some public variables to view in Unity inspector
        velocity = GetComponent<Rigidbody2D>().velocity;
        mouseAngleFromPlayer = GetMouseAngleFromPlayer();
        
        // Update timers
        timeSinceClickedPlayer += Time.deltaTime;
        frameCount.GetComponent<Text>().text = Time.realtimeSinceStartup.ToString("F3");


        // Set grounded flag - can jump off of platforms, enemies, or objects
        bool grounded1 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("PlatformLayer"));
        bool grounded2 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ObjectLayer"));
        bool grounded3 = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("EnemyLayer"));
        grounded = grounded1 || grounded2 || grounded3;

        // Set mouseInsideClickCheckBox flag
        mouseInsideClickCheckBox = clickCheck.GetComponent<BoxCollider2D>().bounds.Contains(GetMouseWorldPosition());
        
        // Display white clickCheck box around player when mouse is inside click check box
        clickCheck.GetComponent<SpriteRenderer>().enabled = mouseInsideClickCheckBox;


        // Horizontal movement
        xDirectionTemp1 = (int) Input.GetAxisRaw("Horizontal");
        xDirectionTemp2 = 0;
        if (grounded && clickDraggingPlayer && !mouseInsideClickCheckBox) {
            if (!MouseAngleFromPlayerWithinJumpThreshold()) {
                Vector3 diff = GetMouseWorldPosition() - transform.position;
                if      (diff.x > 0) { xDirectionTemp2 = 1;  }
                else if (diff.x < 0) { xDirectionTemp2 = -1; }
                else                 { xDirectionTemp2 = 0;  }
            }
        }
        xDirection = xDirectionTemp2 == 0 ? xDirectionTemp1 : xDirectionTemp2;  // use direction from kb input if direction not set from mouse


        // Jumping
        bool tempJump1 = grounded && Input.GetButtonDown("Jump");
        bool tempJump2 = false;
        if (grounded && clickDraggingPlayer && !mouseInsideClickCheckBox) {
            if (MouseAngleFromPlayerWithinJumpThreshold()) {
                tempJump2 = true;
//                Debug.Log("setting jump to true  grounded:" + grounded + "  jump:" + jump + "  realtimeSinceStartup:" + Time.realtimeSinceStartup);
            }
        }
        jump = tempJump1 || tempJump2;  // set jump flag if detect jump from either kb or from mouse position
        
    }

    private void FixedUpdate() {
        numFixedUpdateCalls++;

        // Animation parameters
        animator.SetBool("Grounded", grounded);
        animator.SetInteger("Speed", Mathf.Abs(xDirection));

        // Horizontal movement
        Move(xDirection);

        // Jumping
        if (jump) {
            jump = false;
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
//            Debug.Log("added jump force!  grounded:" + grounded + "  jump:" + jump + "  realtimeSinceStartup:" + Time.realtimeSinceStartup);
        }


        // Respawn if fallen off the world
        if (transform.position.y <= -10) {
            transform.position = spawnPoint;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            facingRight = true;
        }
    }

    private void OnMouseDown() {
        clickDraggingPlayer = true;
        spriteRenderer.color = Color.red;
        timeSinceClickedPlayer = 0;  // reset this timer
    }

    private void OnMouseUp() {
        clickDraggingPlayer = false;
        spriteRenderer.color = Color.white;
    }

    private void Move(int h) {

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeedX yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        }

        // Slows down player slightly faster
        if (h == 0) {
//            GetComponent<Rigidbody2D>().velocity *= 0.975f;
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

    private void OnCollisionEnter2D(Collision2D coll) {
//        Debug.Log("Player collided with " + coll.gameObject.name);
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

    private float GetMouseAngleFromPlayer() {
        Vector2 topCenter = new Vector2(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y);
        float yDist = Mathf.Abs(topCenter.y - GetMouseWorldPosition().y);
        float xDist = Mathf.Abs(topCenter.x - GetMouseWorldPosition().x);
        float rads = Mathf.Atan(yDist / xDist);
        float degs = rads * Mathf.Rad2Deg;

        bool mouseLeftOfPlayer = GetMouseWorldPosition().x < topCenter.x;
        bool mouseBelowPlayer = GetMouseWorldPosition().y < topCenter.y;

        // Quadrant I
        if (!mouseLeftOfPlayer && !mouseBelowPlayer) {
            // do nothing
        }

        // Qudrant II
        else if (mouseLeftOfPlayer && !mouseBelowPlayer) {
            degs = 180 - degs;
        }

        // Quadrant III
        else if (mouseLeftOfPlayer && mouseBelowPlayer) {
            degs = 180 + degs;
        }

        // Quadrant IV
        else if (!mouseLeftOfPlayer && mouseBelowPlayer) {
            degs = 360 - degs;
        }
        
        return degs;
    }

    private bool MouseAngleFromPlayerWithinJumpThreshold() {
        return (GetMouseAngleFromPlayer() > mouseAngleFromPlayerJumpThresholdLow && 
                GetMouseAngleFromPlayer() < mouseAngleFromPlayerJumpThresholdHigh);
    }

}
