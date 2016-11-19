using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private bool alreadySpawnedCollectable = false;
    private bool stopMoving = false;
    private int direction = 1;
    private bool alreadyHitFloor = false;
    private float maxSpeed = 2f;
    private float moveForce = 10f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Vector3 spawnPoint;
    private Player player;
    private AudioSource audioSource;
    private GameManager gameManager;

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("enemyDead") as AudioClip;
        gameManager = transform.Find("/GameManager").gameObject.GetComponent<GameManager>();
        player = transform.Find("/Player").gameObject.GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
        if (gameObject.name.StartsWith("Bear")) {
            direction *= -1;
        }
    }

    void FixedUpdate() {
        if (gameObject.name.StartsWith("Bear")) {
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
        if (coll.gameObject.layer != LayerMask.NameToLayer("EnemyLayer") && alreadyHitFloor) {
            direction *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            if (direction > 0) {
                if (GetComponent<CircleCollider2D>() != null) {
                    GetComponent<CircleCollider2D>().offset = new Vector2(0.02f, 0f);
                }
            } else if (direction < 0) {
                if (GetComponent<CircleCollider2D>() != null) {
                    GetComponent<CircleCollider2D>().offset = new Vector2(-0.02f, 0f);
                }
            }
        }
        alreadyHitFloor = true;
    }

    public void TakeDamage() {
        Die();
    }

    public void Die() {
        audioSource.Play();
        if (!alreadySpawnedCollectable) {
            gameManager.SpawnBookCollectable(transform.position);
            alreadySpawnedCollectable = true;
        }
        if (gameObject.name.StartsWith("Bear")) {
            animator.SetBool("GettingEaten", true);
            stopMoving = true;
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
