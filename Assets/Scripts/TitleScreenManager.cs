using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager: MonoBehaviour {

    private Button playButton;

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;

	private void Start () {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(LoadLevel1);
    }
	
    private void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}

	public void SetMasterVolume(float value) {

	}

	public void SetMusicVolume(float value) {

	}

	public void SetSfxVolume(float value) {

	}
}
