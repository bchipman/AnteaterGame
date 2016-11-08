using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour {

	private void Start () {
        GetComponent<Button>().onClick.AddListener(Play);
    }
	
    private void Play() {
        SceneManager.LoadScene("Level1");
    }
}
