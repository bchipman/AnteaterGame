using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject explosion;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D coll) {

        if (coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("EnemyLayer")) {
            Debug.Log("Collided with " + coll.gameObject.name);
            Destroy(coll.gameObject);
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            explosionInstance.SetActive(true);
            Destroy(explosionInstance, explosionInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 0f);
            Destroy(gameObject);
        }
    }

}
