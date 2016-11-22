using UnityEngine;

public class TitleScreenState : MonoBehaviour {

    public static TitleScreenState instance;
    public bool isMuted;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

}
