using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private bool alreadySpawnedCollectable = false;
    private bool stopMoving = false;
    public int direction = 1;
    private bool alreadyHitFloor = false;
    private float maxSpeed = 2f;
    private float moveForce = 10f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Vector3 spawnPoint;
    private Player player;
    private AudioSource audioSource;
    private GameManager gameManager;
    private bool debugAnimationToggleOn = false;

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("enemyDead") as AudioClip;
        gameManager = transform.Find("/GameManager").gameObject.GetComponent<GameManager>();
        player = transform.Find("/Player").gameObject.GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
    }

    void Update() {
        if (gameObject.name.StartsWith("Bobcat") || gameObject.name.StartsWith("Bear")) {
            if (Input.GetKeyDown(KeyCode.F10)) {
                Vector3 currScale = gameObject.transform.localScale;
                gameObject.transform.localScale = new Vector3(-1*currScale.x, currScale.y, currScale.z);
            }

            if (Input.GetKeyDown(KeyCode.F11)) {
                stopMoving = !stopMoving;
            }

            if (Input.GetKeyDown(KeyCode.F12)) {
                debugAnimationToggleOn = !debugAnimationToggleOn;
                animator.SetBool("GettingEaten", debugAnimationToggleOn);
            }
        }
    }

    void FixedUpdate() {
        if (gameObject.name.StartsWith("Bear") || gameObject.name.StartsWith("Bobcat")) {
            float playerX = player.transform.position.x;
            float distToPlayer = Mathf.Abs(playerX - transform.position.x);
            animator.SetFloat("DistToPlayer", distToPlayer);

            float leftX = spriteRenderer.bounds.center.x - spriteRenderer.bounds.extents.x;
            float rightX = spriteRenderer.bounds.center.x + spriteRenderer.bounds.extents.x;

            // player is closer to left side of sprite
            bool closerToLeft = Mathf.Abs(playerX - leftX) < Mathf.Abs(playerX - rightX);

            bool bearCanSeePlayer;
            if (direction == -1 && closerToLeft) {
                bearCanSeePlayer = true;
            } else if (direction == -1 && !closerToLeft) {
                bearCanSeePlayer = false;
            } else if (direction == 1 && closerToLeft) {
                bearCanSeePlayer = false;
            } else {
                bearCanSeePlayer = true;
            }
            animator.SetBool("CanSeePlayer", bearCanSeePlayer);

        }
        if (direction * GetComponent<Rigidbody2D>().velocity.x < maxSpeed && !stopMoving) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * moveForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (alreadyHitFloor && (coll.gameObject.layer == LayerMask.NameToLayer("InvisibleWallLayer") || coll.gameObject.layer == LayerMask.NameToLayer("WallLayer") || coll.gameObject.name == "Player")) {
            direction *= -1;
            Vector3 currScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(-1 * currScale.x, currScale.y, currScale.z);
        }
        alreadyHitFloor = true;
    }

    public void TakeDamage() {
        Die();
    }

    public void Die() {
        audioSource.Play();
        stopMoving = true;
        if (!alreadySpawnedCollectable) {
            gameManager.SpawnBookCollectable(transform.position);
            alreadySpawnedCollectable = true;
        }
        if (gameObject.name.StartsWith("Bear") || gameObject.name.StartsWith("Bobcat")) {
            animator.SetBool("GettingEaten", true);
        }
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<CircleCollider2D>());
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(3);
        Destroy(this);
    }

}
