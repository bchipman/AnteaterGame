using UnityEngine;
using System.Collections;

public class Professor : MonoBehaviour {

	public GameManager gameManager;
    private bool isTouching; //in order to prevent multiple triggers of OnTriggerEnter2D/OnTriggerExit2D
    private Animator animator;


    private void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
    }
	
	private void OnTriggerEnter2D(Collider2D collider) {
		if (!isTouching) {
			isTouching = true;
			gameManager.CheckForGameWin();
            animator.SetBool("Talking", true);
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (isTouching) {
			isTouching = false;
            animator.SetBool("Talking", false);
		}
	}
}
