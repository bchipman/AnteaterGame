using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int score;
    private GameObject scoreText;
    private GameObject frameCountText;

    private void Start () {
        scoreText = GameObject.Find("ScoreText");
        frameCountText = GameObject.Find("FrameCountText");
    }
	
	private void Update () {
        frameCountText.GetComponent<Text>().text = Time.realtimeSinceStartup.ToString("F3");
    }

    public void IncrementScore() {
        score++;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
