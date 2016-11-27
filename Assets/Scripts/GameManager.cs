using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
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
    private RectTransform[] bookCirclesRectTransLi;
    public GameObject bulletPrefab;
    public GameObject zotBubblePrefab;
    public GameObject dirtPrefab;
    public GameObject bookPrefab;

    private void Start () {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        deathText = GameObject.Find("DeathText").GetComponent<Text>();
//        fpsText = GameObject.Find("FPSText").GetComponent<Text>();
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
//        projectileTypeDropdown = GameObject.Find("ProjectileTypeDropdown").GetComponent<Dropdown>();
        bookCirclesRectTransLi = GameObject.Find("/Canvas/Circles").GetComponentsInChildren<RectTransform>();
        bookCirclesRectTransLi = bookCirclesRectTransLi.ToList().Where(r => new Regex(@"Circle\d").IsMatch(r.name)).ToArray();
        closeButton.onClick.AddListener(LoadTitleScreen);
        StartCoroutine(CheckProjectileTimer());
    }

	private void Update () {
        timeText.text = "Time: " + Time.realtimeSinceStartup.ToString("000.");
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI() {
//        int w = Screen.width, h = Screen.height;
//        float smallSize = 0.07f;
//        float largeSize = 0.2f;
//
//        fpsText.fontSize = Mathf.RoundToInt(h * smallSize);
//        scoreText.fontSize = Mathf.RoundToInt(h * smallSize);
//        timeText.fontSize = Mathf.RoundToInt(h * smallSize);
//        deathText.fontSize = Mathf.RoundToInt(h * largeSize);
//        
//        closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(h * 0.12f, h * 0.12f);
//        closeButton.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 1f);
//        closeButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
//        Vector2 size = closeButton.GetComponent<RectTransform>().sizeDelta;
//        Vector2 halfSize = new Vector2(-1*size.x/2, -1*size.y/2);
//        closeButton.GetComponent<RectTransform>().anchoredPosition = halfSize;
//
//        Text closeButtonText = GameObject.Find("CloseButton/Text").GetComponent<Text>();
//        closeButtonText.fontSize = Mathf.RoundToInt(h * 0.1f);
//
//        float fps = 1.0f / deltaTime;
//        fpsText.text = string.Format("FPS: {0:0.}", fps);
//
//        for (int i = 0; i < bookCirclesRectTransLi.Length; i++) {
//            var rectTrans = bookCirclesRectTransLi[i];
//            rectTrans.sizeDelta = new Vector2(h * 0.1f, h * 0.1f); // radius = 10% of screen height
//            rectTrans.anchoredPosition = new Vector2(w * 0.7f + (i * rectTrans.sizeDelta.x) + (i * 0.002f * h), h * -0.05f - 0.005f * h); // 70% to right, 0.5% from top
//            rectTrans.transform.FindChild("Book").GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.025f, w * 0.025f * 1.7f);
//        }

    }

    public void CollectedBook() {
        foreach (var rectTrans in bookCirclesRectTransLi) {
            Transform bookTrans = rectTrans.transform.FindChild("Book");
            if (!bookTrans.gameObject.activeSelf) {
                bookTrans.gameObject.SetActive(true);
                break;
            }
        }
    }

    public int NumberOfBooksCollected() {
        int numCollected = 0;
        foreach (var rectTrans in bookCirclesRectTransLi) {
            Transform bookTrans = rectTrans.transform.FindChild("Book");
            if (bookTrans.gameObject.activeSelf) {
                numCollected++;
            }
        }
        return numCollected;
    }

    public void LostAllBooks() {
        foreach (var rectTrans in bookCirclesRectTransLi) {
            Transform bookTrans = rectTrans.transform.FindChild("Book");
            bookTrans.gameObject.SetActive(false);
        }
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
//        switch (projectileTypeDropdown.value) {
//            case 0: return bulletPrefab;
//            case 1: return zotBubblePrefab;
//            default: return dirtPrefab;
//        }
        return dirtPrefab;
    }

    public void SpawnBookCollectable(Vector3 position) {
        GameObject book = Instantiate(bookPrefab, position, Quaternion.identity) as GameObject;
        book.transform.SetParent(transform.Find("/Collectables"));
    }

	public void CheckForGameWin() {
		Debug.Log ("checking for game win");
		SceneManager.LoadSceneAsync ("EndScreen");
	}

}
