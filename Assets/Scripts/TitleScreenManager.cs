using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager: MonoBehaviour {

    private Button playButton;
	private bool isMuted = false;
	private Toggle muted;

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;

	private void Start () {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(LoadLevel1);

		//AudioListener.volume = 0; //works to turn off sound automatically
    }
	
    private void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);

		muted = GameObject.Find ("/Canvas/OptionsMenu/MuteToggle").GetComponent<Toggle> ();
		muted.enabled = true;
		muted.onValueChanged.AddListener (ToggleSound);
	}

	public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}

	public void ToggleSound(bool value) {
		//isMuted = GameObject.Find ("MuteToggle").GetComponent<Toggle> ();
		if (value)
			AudioListener.volume = 0;
		else
			AudioListener.volume = 1;
	}

	/*
	public void SetMasterVolume(float value) {

	}*/

	public void SetMusicVolume(float value) {

	}

	public void SetSfxVolume(float value) {

	}
}
