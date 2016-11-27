using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager: MonoBehaviour {

    private TitleScreenState titleScreenState;
    public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;


	private void Start () {
	    titleScreenState = GameObject.Find("TitleScreenState").GetComponent<TitleScreenState>();
	}

//    private void OnGUI() {
//        int w = Screen.width;
//        int h = Screen.height;
//        float smallFontSize = 0.06f;
//
////        GameObject mainMenuImageObj = GameObject.Find("MainMenuImage");
////        if (mainMenuImageObj != null) {
////            RectTransform mainMenuImageRectTransform = mainMenuImageObj.GetComponent<RectTransform>();
////            mainMenuImageRectTransform.sizeDelta = new Vector2(w, h);
////            Vector2 mainMenuImageSize = mainMenuImageRectTransform.sizeDelta;
////            Vector2 mainMenuImageHalfSize = new Vector2(-1 * mainMenuImageSize.x / 2, mainMenuImageSize.y / 2);
////            mainMenuImageRectTransform.anchoredPosition = mainMenuImageHalfSize;
////        }
////
////        GameObject optionsMenuImageObj = GameObject.Find("OptionsMenuImage");
////        if (optionsMenuImageObj != null) {
////            RectTransform optionsImageRectTransform = optionsMenuImageObj.GetComponent<RectTransform>();
////            optionsImageRectTransform.sizeDelta = new Vector2(w, h);
////            Vector2 optionsMenuImageSize = optionsImageRectTransform.sizeDelta;
////            Vector2 optionsMenuImageHalfSize = new Vector2(-1 * optionsMenuImageSize.x / 2, optionsMenuImageSize.y / 2);
////            optionsImageRectTransform.anchoredPosition = optionsMenuImageHalfSize;
////        }
//
//        GameObject playButtonObj = GameObject.Find("PlayButton");
//        if (playButtonObj != null) {
//            Button playButton = playButtonObj.GetComponent<Button>();
//            playButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.40f, h * 0.10f);
//            playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.65f);
//            playButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallFontSize);
//        }
//
//        GameObject optionsButtonObj = GameObject.Find("OptionsButton");
//        if (optionsButtonObj != null) {
//            Button optionsButton = optionsButtonObj.GetComponent<Button>();
//            optionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.40f, h * 0.10f);
//            optionsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.8f);
//            optionsButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallFontSize);
//        }
//
//        GameObject backButtonObj = GameObject.Find("BackButton");
//        if (backButtonObj != null) {
//            Button backButton = backButtonObj.GetComponent<Button>();
//            backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.40f, h * 0.10f);
//            backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.8f);
//            backButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallFontSize);
//        }
//
////        GameObject muteToggleObj = GameObject.Find("MuteToggle");
////        if (muteToggleObj != null) {
////            GameObject muteToggleCheckmarkObj = GameObject.Find("MuteToggle/Background/Checkmark");
////            Debug.Log(muteToggleCheckmarkObj);
////            RectTransform muteToggleCheckmarkRectTransform = muteToggleCheckmarkObj.GetComponent<RectTransform>();
//////            muteToggleCheckmarkRectTransform.sizeDelta = new Vector2(w*0.40f, h * 0.10f);
////            muteToggleCheckmarkRectTransform.sizeDelta = new Vector2(w, h);
////
//////            RectTransform muteToggleRectTransform = muteToggleObj.GetComponent<RectTransform>();
//////            muteToggleRectTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 0.40f, h * 0.10f);
//////            muteToggleRectTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * h * 0.8f);
//////            backButtonObj.GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt(h * smallFontSize);
////        }
//    }

    public void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);

		Toggle mutedToggle = GameObject.Find ("/Canvas/OptionsMenu/MuteToggle").GetComponent<Toggle> ();
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
