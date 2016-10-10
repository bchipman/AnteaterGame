using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject explosion;


    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("EnemyLayer") ||
            coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("ObjectLayer")) {
            Debug.Log("Collided with " + coll.gameObject.name);
            Destroy(coll.gameObject);
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            explosionInstance.SetActive(true);
            Destroy(explosionInstance, explosionInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 0f);
            Destroy(gameObject);
        }
    }

}
