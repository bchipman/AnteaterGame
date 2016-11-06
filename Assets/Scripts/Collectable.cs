using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collectable : MonoBehaviour {

    public GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "Player") {
            Destroy(gameObject);
            gameManager.IncrementScore();
        }
    }

}
