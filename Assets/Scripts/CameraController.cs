using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public List<GameObject> lines;
    private float minX = 0f;
    private float minY = 0f;
    private float xAdj = 2f;  // Adjustment from center; keeps player at ~40% of horizontal distance of screen instead of dead center

    private void Start() {
//        lines = new List<GameObject> {new GameObject(), new GameObject()};
    }

    void Update () {
        float x = player.transform.position.x + xAdj < minX ? minX : player.transform.position.x + xAdj;
        float y = player.transform.position.y < minY ? minY : player.transform.position.y;
        transform.position = new Vector3(x, y, -10f);

//        int w = Screen.width, h = Screen.height;
//        Vector3 bottomMiddle = Camera.main.ScreenToWorldPoint(new Vector3(0.50f * w, 0, 0f));
//        Vector3 topMiddle = Camera.main.ScreenToWorldPoint(new Vector3(0.50f * w, h, 0f));
//        bottomMiddle.z = 0f;
//        topMiddle.z = 0f;
//
//        Vector3 bottomMiddle2 = Camera.main.ScreenToWorldPoint(new Vector3(0.40f * w, 0, 0f));
//        Vector3 topMiddle2 = Camera.main.ScreenToWorldPoint(new Vector3(0.40f * w, h, 0f));
//        bottomMiddle2.z = 0f;
//        topMiddle2.z = 0f;
//
//        Color green = new Color(0, 255, 0);
//        Color red = new Color(255, 0, 0);
//        DrawLine(0, bottomMiddle, topMiddle, green);
//        DrawLine(1, bottomMiddle2, topMiddle2, red);
    }


    void DrawLine(int lineIndex, Vector3 start, Vector3 end, Color color) {
        GameObject line = lines[lineIndex];
        Destroy(line);
        line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.025f, 0.025f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lines[lineIndex] = line;
    }
}
