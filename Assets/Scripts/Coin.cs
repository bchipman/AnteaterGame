using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coin : MonoBehaviour {

    public Player player;
    public GameObject scoreText;

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "Player") {
            Destroy(gameObject);
            player.score++;
            scoreText.GetComponent<Text>().text = player.score.ToString();
        }
    }

}
