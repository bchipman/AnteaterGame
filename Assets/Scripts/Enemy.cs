using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private int direction = 1;
    private float maxSpeed = 2f;
    public float moveForce = 10f;
    private SpriteRenderer spriteRenderer;
    public Vector3 spawnPoint;


    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    void FixedUpdate() {
        if (direction * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * moveForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        direction *= -1;
        if (direction > 0) {
            spriteRenderer.flipX = false;
            if (GetComponent<CircleCollider2D>() != null) {
                GetComponent<CircleCollider2D>().offset = new Vector2(0.02f, 0f);
            }
        } else if (direction < 0) {
            spriteRenderer.flipX = true;
            if (GetComponent<CircleCollider2D>() != null) {
                GetComponent<CircleCollider2D>().offset = new Vector2(-0.02f, 0f);
            }
        }
    }

}
