using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager: MonoBehaviour {

    private Button playButton;

	private void Start () {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(LoadLevel1);
    }
	
    private void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }
}
