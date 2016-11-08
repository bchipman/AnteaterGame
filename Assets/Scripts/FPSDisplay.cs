// Source: http://wiki.unity3d.com/index.php/FramesPerSecond

using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

    private float deltaTime = 0.0f;
    private Text fpsText;

    private void Start () {
        fpsText = GetComponent<Text>();
    }

    private void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI() {
        int w = Screen.width, h = Screen.height;
        fpsText.fontSize = h * 5 / 100;  // 5% of screen height

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

//        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = string.Format("FPS: {0:0.}", fps);
    }


}
