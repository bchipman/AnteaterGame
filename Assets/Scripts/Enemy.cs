using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Vector3 spawnPoint;
    public int direction = 1;
    public bool dontMove = false;
    public bool flipOnStart = false;
    public bool forceAttackAnimation = false;

    private float maxSpeed = 2f;
    private float moveForce = 10f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Player player;
    private AudioSource audioSource;
    private GameManager gameManager;
    private bool alreadyHitFloor = false;
    private bool dying = false;
    private bool debugAnimationToggleOn = false;
    private float gettingEatenClipLength;

    private void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("enemyDead") as AudioClip;
        gameManager = transform.Find("/GameManager").gameObject.GetComponent<GameManager>();
        player = transform.Find("/Player").gameObject.GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
        SetAntsAnimClipLength();
        if (flipOnStart) { FlipX(); }
    }

    private void SetAntsAnimClipLength() {
        if (gameObject.name.StartsWith("Bear") || gameObject.name.StartsWith("Bobcat")) {
            AnimationClip[] animClips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in animClips) {
                if (clip.name.Contains("Eaten")) {
                    gettingEatenClipLength = clip.length / animator.GetFloat("GettingEatenSpeed");
                    break;
                }
            }
        }
    }

    private void Update() {
        if (gameObject.name.StartsWith("Bobcat") || gameObject.name.StartsWith("Bear")) {
            if (Input.GetKeyDown(KeyCode.F10)) {
                FlipY();
                RemoveColliders();
            }

            if (Input.GetKeyDown(KeyCode.F11)) {
                dontMove = !dontMove;
            }

            if (Input.GetKeyDown(KeyCode.F12)) {
                debugAnimationToggleOn = !debugAnimationToggleOn;
                animator.SetBool("GettingEaten", debugAnimationToggleOn);
            }
        }
    }

    private void FixedUpdate() {
        if (gameObject.name.StartsWith("Bear") || gameObject.name.StartsWith("Bobcat")) {
            float playerX = player.transform.position.x;
            float distToPlayer = Mathf.Abs(playerX - transform.position.x);
            animator.SetFloat("DistToPlayer", distToPlayer);
            
            bool attackAnimationOn = forceAttackAnimation ? forceAttackAnimation : CanSeePlayer();
            animator.SetBool("CanSeePlayer", attackAnimationOn);
        }
        if (direction * GetComponent<Rigidbody2D>().velocity.x < maxSpeed && !dontMove) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * moveForce);
        }
    }

    private bool CanSeePlayer() {
        float playerX = player.transform.position.x;
        float leftX = spriteRenderer.bounds.center.x - spriteRenderer.bounds.extents.x;
        float rightX = spriteRenderer.bounds.center.x + spriteRenderer.bounds.extents.x;
        bool closerToLeft = Mathf.Abs(playerX - leftX) < Mathf.Abs(playerX - rightX);
        bool canSeePlayer;
        if (direction == -1 && closerToLeft) {
            canSeePlayer = true;
        }
        else if (direction == -1 && !closerToLeft) {
            canSeePlayer = false;
        }
        else if (direction == 1 && closerToLeft) {
            canSeePlayer = false;
        }
        else {
            canSeePlayer = true;
        }
        return canSeePlayer;
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (alreadyHitFloor && (coll.gameObject.layer == LayerMask.NameToLayer("InvisibleWallLayer") || coll.gameObject.layer == LayerMask.NameToLayer("WallLayer"))) {
            direction *= -1;
            FlipX();
        }
        alreadyHitFloor = true;
    }

    private void FlipX() {
        // Better than flipping the SpriteRenderer since colliders will not need adjusting
        Vector3 currScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(-1 * currScale.x, currScale.y, currScale.z);
    }

    private void FlipY() {
        // Better than flipping the SpriteRenderer since colliders will not need adjusting
        Vector3 currScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(currScale.x, -1 * currScale.y, currScale.z);
    }

    public void TakeDamage() {
        Die();
    }

    public void Die() {
        if (dying) { return; }  // this method should only be called once
        dying = true;
        dontMove = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        audioSource.Play();
        gameManager.SpawnBookCollectable(transform.position);
        if (gameObject.name.StartsWith("Bear") || gameObject.name.StartsWith("Bobcat")) {
            animator.SetBool("GettingEaten", true);
            if (!CanSeePlayer()) { FlipX(); }
            StartCoroutine(RemoveCollidersTimer());
        } else {
            RemoveColliders();
            StartCoroutine(DestroyTimer());
        }
    }

    private void RemoveColliders() {
        var allcolliders = GetComponentsInChildren<Collider2D>();
        foreach (var childCollider in allcolliders)
            Destroy(childCollider);
    }

    private IEnumerator RemoveCollidersTimer() {
        yield return new WaitForSeconds(gettingEatenClipLength * 3);
        FlipY();
        RemoveColliders();
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
