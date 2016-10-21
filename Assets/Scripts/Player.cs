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
    public GameManager gameManager;
    public GameObject bullet;
    public Vector2 velocity; // temporary, for debugging
    public float mouseAngleFromPlayer;

    private bool jump = false;
    private bool grounded = false;
    private bool facingRight = true;
    private bool fireUp = false;
    public bool clickDraggingPlayer = false;
    private bool mouseInsideClickCheckBox = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform groundCheck;
    private Transform clickCheck;
    private Vector3 spawnPoint;
    public Vector3 mousePositionWhenClickedPlayer;
    public Vector3 mousePositionNow;
    public float mousePositionDiffVertical;

    public int xDirection = 0;
    public int xDirectionTemp1 = 0;
    public int xDirectionTemp2 = 0;
    public int numFixedUpdateCalls = 0;

    public float yMouseVelocityInLastSec = 0;

    private Queue<List<float>> mousePositionQueue;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        clickCheck = transform.Find("ClickCheck");
        spawnPoint = transform.position;
        StartCoroutine("ShootTimer");
        mousePositionQueue = new Queue<List<float>>();
    }

    // Check input in Update and set flags to be acted on in FixedUpdate
    private void Update() {

        // Save mouse position to be used in FixedUpdate
        mousePositionNow = Input.mousePosition;

        // Just setting some public variables to view in Unity inspector
        velocity = GetComponent<Rigidbody2D>().velocity;
        
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
        mousePositionWhenClickedPlayer = Input.mousePosition;
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
            if (Input.GetMouseButton(0) && !clickDraggingPlayer) {
                FireNEW();
                yield return new WaitForSeconds(shotDelay);
            }
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

    private void FireNEW() {
        // Create bullet instance near player's current position.
        Vector3 bulletOffset;
        Vector2 bulletVelocity;
        Quaternion bulletQuaternion;
        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y, 0);

        float xDist = Mathf.Abs(playerPos.x - GetMouseWorldPosition().x);
        float yDist = Mathf.Abs(playerPos.y - GetMouseWorldPosition().y);
        float rads = Mathf.Atan(yDist / xDist);
        float degs = rads * Mathf.Rad2Deg;
        float xVectorScale = Mathf.Cos(rads);
        float yVectorScale = Mathf.Sin(rads);

        bool mouseLeftOfPlayer = GetMouseWorldPosition().x < playerPos.x;
        bool mouseBelowPlayer = GetMouseWorldPosition().y < playerPos.y;

        // Quadrant I
        if (!mouseLeftOfPlayer && !mouseBelowPlayer) {
            // do nothing
        }

        // Quadrant II
        else if (mouseLeftOfPlayer && !mouseBelowPlayer) {
            degs = 180 - degs;
            xVectorScale *= -1;
        }

        // Quadrant III
        else if (mouseLeftOfPlayer && mouseBelowPlayer) {
            degs = 180 + degs;
            xVectorScale *= -1;
            yVectorScale *= -1;
        }

        // Quadrant IV
        else if (!mouseLeftOfPlayer && mouseBelowPlayer) {
            degs = 360 - degs;
            yVectorScale *= -1;
        }

        bulletVelocity = new Vector2(xVectorScale * bulletForce, yVectorScale * bulletForce);
        bulletOffset = new Vector3(0.5f, 0, 0);
        bulletQuaternion = Quaternion.AngleAxis(270 + degs, Vector3.forward);

        Vector3 newBulletPosition = playerPos;
//        Vector3 newBulletPosition = playerPos + bulletOffset;
        GameObject bulletInstance = Instantiate(bullet, newBulletPosition, bulletQuaternion) as GameObject;
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bulletInstance.transform.SetParent(this.transform);
//        Debug.Log("FireNEW bulletVelocity: " + bulletVelocity + ",  degs: " + degs);
    }

    private void OnCollisionEnter2D(Collision2D coll) {
//        Debug.Log("Player collided with " + coll.gameObject.name);
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

}
