using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int score;
    private GameObject scoreText;
    private GameObject frameCountText;
    private GameObject deathText;

    private void Start () {
        scoreText = GameObject.Find("ScoreText");
        frameCountText = GameObject.Find("FrameCountText");
        deathText = GameObject.Find("DeathText");
        StartCoroutine(CheckProjectileTimer());
    }
	
	private void Update () {
        frameCountText.GetComponent<Text>().text = "Time: " + Time.realtimeSinceStartup.ToString("000.");
	}

    public void IncrementScore() {
        score++;
        scoreText.GetComponent<Text>().text = "Score: " + score.ToString("000");
    }

    public void DisplayDeathText() {
        StartCoroutine(DisplayDeathTextTimer());
    }

    private IEnumerator DisplayDeathTextTimer() {
        deathText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(3);
        deathText.GetComponent<Text>().enabled = false;
    }

    private IEnumerator CheckProjectileTimer() {
        Transform parentTransform = transform.Find("/Projectiles");
        while (true) {
            foreach (Transform child in parentTransform) {
                if (!child.gameObject.GetComponent<Renderer>().isVisible) {
                    Destroy(child.gameObject);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}
