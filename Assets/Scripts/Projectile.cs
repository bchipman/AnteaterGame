using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

    public GameObject explosion;
    public EnemyHandler enemyHandler;

    public float projectileForce;
    public float shotDelay;
    public int angleOffset;


    private void Awake() {
        enemyHandler = GameObject.Find("EnemyHandler").GetComponent<EnemyHandler>();

        if (gameObject.name.StartsWith("Bullet")) {
            projectileForce = 20f;
            shotDelay = 0.08f;
            angleOffset = 270;
        } else if (gameObject.name.StartsWith("ZotBubble")) {
            projectileForce = 5f;
            shotDelay = 0.2f;
            angleOffset = 0;
        } else if (gameObject.name.StartsWith("Dirt")) {
            projectileForce = 5f;
            shotDelay = 0.2f;
            angleOffset = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("EnemyLayer") ||
            coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("ObjectLayer")) {
//            Debug.Log("Collided with " + coll.gameObject.name);
            enemyHandler.KillEnemy(coll.gameObject);


//            Destroy(coll.gameObject);
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            explosionInstance.SetActive(true);
            Destroy(explosionInstance, explosionInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 0f);
            Destroy(gameObject);
        }
    }

}
