using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int score;
    private float deltaTime;
    private Text scoreText;
    private Text timeText;
    private Text deathText;
    private Text fpsText;
    private Button closeButton;
    private Dropdown projectileTypeDropdown;
    public GameObject bulletPrefab;
    public GameObject zotBubblePrefab;
    public GameObject dirtPrefab;


    private void Start () {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        deathText = GameObject.Find("DeathText").GetComponent<Text>();
        fpsText = GameObject.Find("FPSText").GetComponent<Text>();
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        projectileTypeDropdown = GameObject.Find("ProjectileTypeDropdown").GetComponent<Dropdown>();
        closeButton.onClick.AddListener(LoadTitleScreen);
        StartCoroutine(CheckProjectileTimer());
    }
	
	private void Update () {
        timeText.text = "Time: " + Time.realtimeSinceStartup.ToString("000.");
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI() {
        int w = Screen.width, h = Screen.height;
        fpsText.fontSize = h * 5 / 100;  // 5% of screen height
        scoreText.fontSize = h * 5 / 100;  // 5% of screen height
        timeText.fontSize = h * 5 / 100;  // 5% of screen height

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        //        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = string.Format("FPS: {0:0.}", fps);
    }

    public void IncrementScore() {
        score++;
        scoreText.text = "Score: " + score.ToString("000");
    }

    public void DisplayDeathText() {
        StartCoroutine(DisplayDeathTextTimer());
    }

    private IEnumerator DisplayDeathTextTimer() {
        deathText.enabled = true;
        yield return new WaitForSeconds(3);
        deathText.enabled = false;
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

    private void LoadTitleScreen() {
        SceneManager.LoadScene("TitleScreen");
    }

    public GameObject GetCurrentProjectileType() {
        switch (projectileTypeDropdown.value) {
            case 0: return bulletPrefab;
            case 1: return zotBubblePrefab;
            default: return dirtPrefab;
        }
    }
}
