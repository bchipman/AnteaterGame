using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour {

	private void Start () {
        GetComponent<Button>().onClick.AddListener(test);
    }
	
    private void test() {
        Debug.Log("Test!!");
        SceneManager.LoadScene("Level1");
    }
}
