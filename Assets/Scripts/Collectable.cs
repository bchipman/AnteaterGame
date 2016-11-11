using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collectable : MonoBehaviour {

    public GameManager gameManager;

	public AudioClip Pickup_Coin3;
	public AudioClip Xylo_13;
	private AudioSource playerAS;

    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		playerAS = GetComponent<AudioSource> ();
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "Player" || coll.name == "SideCollisionCheck") {
			if (gameObject.name.StartsWith ("Coin")) {
				//playerAS.clip = Pickup_Coin3;
				//playerAS.PlayOneShot(Pickup_Coin3);
				playerAS.Play();
			}
			if (gameObject.name.StartsWith ("Book")) {
				playerAS.Play ();
			}
			GetComponent<SpriteRenderer>().enabled = false;
			StartCoroutine (DestroyTimer ());
            gameManager.IncrementScore();
        }
    }

	private IEnumerator DestroyTimer(){
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}

}
