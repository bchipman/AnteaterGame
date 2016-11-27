using UnityEngine;
using System.Collections;

public class Professor : MonoBehaviour {
	bool isTouching; //in order to prevent multiple triggers of OnTriggerEnter2D/OnTriggerExit2D
	public GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log (gameManager);
		if (isTouching != true) {
			gameManager.CheckForGameWin ();
			isTouching = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (isTouching != false) {
			isTouching = false;
		}
	}
}
