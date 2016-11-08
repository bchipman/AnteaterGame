using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour {

    private void Start() {
        GetComponent<Button>().onClick.AddListener(Close);
    }

    private void Close() {
        SceneManager.LoadScene("TitleScreen");
    }
}
