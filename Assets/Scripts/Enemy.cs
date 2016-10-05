using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private int direction = 1;
    private float speed = 1f;
    private float maxSpeed = 2f;
    public float moveForce = 10f;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {

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
        } else if (direction < 0) {
            spriteRenderer.flipX = true;
        }
    }
}
