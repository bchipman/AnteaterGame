using UnityEngine;
using System.Collections;

public class RotatingPlatform : MonoBehaviour {

	private bool stopMoving = false;
	public int direction = 1;
	private float maxSpeed = 6f;
	private float moveForce = 100f;

	void Start() {

		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
	}

	void Update() {

	}

	void FixedUpdate() {
		if (direction * GetComponent<Rigidbody2D>().angularVelocity < maxSpeed && !stopMoving) {
			GetComponent<Rigidbody2D>().AddTorque(moveForce);
		}
	}

	private void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer("InvisibleWallLayer") || coll.gameObject.layer == LayerMask.NameToLayer("WallLayer")) {
			direction *= -1;
		}
	}
}