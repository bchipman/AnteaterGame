using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coin : MonoBehaviour {

    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "Player") {
            Destroy(gameObject);
            gameManager.IncrementScore();
        }
    }

}
