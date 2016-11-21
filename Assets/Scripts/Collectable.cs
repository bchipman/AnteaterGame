using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collectable : MonoBehaviour {

    public GameManager gameManager;
	private AudioSource audioSource;
    private bool collected = false;


    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		audioSource = GetComponent<AudioSource> ();
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (!collected) {
            if (coll.name == "Player" || coll.name == "SideCollisionCheck") {
                if (gameObject.name.StartsWith("Coin") || gameObject.name.StartsWith("Book")) {
                    audioSource.Play();
                    gameManager.IncrementScore();
                    collected = true;
                }
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(DestroyTimer());
            }
        }
    }

	private IEnumerator DestroyTimer(){
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}

}
