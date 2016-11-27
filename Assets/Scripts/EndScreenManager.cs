using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour {

    private Button returnButton;

    private void Start () {
        returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(LoadTitleScreen);
    }

    private void LoadTitleScreen() {
        SceneManager.LoadScene("TitleScreen");
    }
}
