using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager: MonoBehaviour {

    private Toggle mutedToggle;
    private Text titleText;
    private RectTransform anteaterImageRectTransform;
    private TitleScreenState titleScreenState;

    public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;

	private void Start () {
        titleText = GameObject.Find("TitleText").GetComponent<Text>();
	    anteaterImageRectTransform = GameObject.Find("AnteaterImage").GetComponent<RectTransform>();
	    titleScreenState = GameObject.Find("TitleScreenState").GetComponent<TitleScreenState>();
	}

    private void OnGUI() {
        int w = Screen.width, h = Screen.height;
        float smallSize = 0.12f;
        float largeSize = 0.15f;

        titleText.fontSize = Mathf.RoundToInt(h * largeSize);
        titleText.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h * 0.2f);
        Vector2 titleTextSize = titleText.GetComponent<RectTransform>().sizeDelta;
        Vector2 titleTextHalfSize = new Vector2(0, -1 * titleTextSize.y / 2);
        titleText.GetComponent<RectTransform>().anchoredPosition = titleTextHalfSize;
        titleText.GetComponent<RectTransform>().offsetMin = new Vector2(0, titleText.GetComponent<RectTransform>().offsetMin.y);
        titleText.GetComponent<RectTransform>().offsetMax = new Vector2(0, titleText.GetComponent<RectTransform>().offsetMax.y);

        anteaterImageRectTransform.localScale = new Vector2(1, 1);
        anteaterImageRectTransform.sizeDelta = new Vector2(w, h * 0.9f);

        GameObject playButtonObj = GameObject.Find("PlayButton");
        if (playButtonObj != null) {
            Button playButton = playButtonObj.GetComponent<Button>();
            playButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.65f, h * 0.15f);
            playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.6f);
            playButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallSize);
        }

        GameObject optionsButtonObj = GameObject.Find("OptionsButton");
        if (optionsButtonObj != null) {
            Button optionsButton = optionsButtonObj.GetComponent<Button>();
            optionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.65f, h * 0.15f);
            optionsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.8f);
            optionsButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallSize);
        }

        GameObject backButtonObj = GameObject.Find("BackButton");
        if (backButtonObj != null) {
            Button backButton = backButtonObj.GetComponent<Button>();
            backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.65f, h * 0.15f);
            backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.8f);
            backButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallSize);
        }
    }


    public void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);

		mutedToggle = GameObject.Find ("/Canvas/OptionsMenu/MuteToggle").GetComponent<Toggle> ();
		mutedToggle.enabled = true;
	    mutedToggle.isOn = titleScreenState.isMuted;
		mutedToggle.onValueChanged.AddListener (ToggleSound);
	}

	public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}

	public void ToggleSound(bool value) {
	    if (value) {
	        AudioListener.volume = 0;
	        titleScreenState.isMuted = true;
	    } else {
			AudioListener.volume = 1;
	        titleScreenState.isMuted = false;
	    }
	}

	/*
	public void SetMasterVolume(float value) {

	}*/

	public void SetMusicVolume(float value) {

	}

	public void SetSfxVolume(float value) {

	}
}
