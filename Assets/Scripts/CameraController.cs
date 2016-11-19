//CameraController with junk debug line drawing code.

using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public List<GameObject> lines;
    private float minX = 0f;
    private float minY = 0f;
    private float xAdj = 2f;  // Adjustment from center; keeps player at ~40% of horizontal distance of screen instead of dead center

    private void Start() {
        GameObject debugLinesGameObject = new GameObject {name = "DebugLines"};
        debugLinesGameObject.transform.SetParent(transform.Find("/GameManager"));
        lines = new List<GameObject> { new GameObject(), new GameObject(), new GameObject(), new GameObject() };

        foreach (GameObject line in lines) {
            line.transform.SetParent(transform.Find("/GameManager/DebugLines"));
            line.transform.position = Vector3.zero;
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        }
    }

    void Update() {
        float x = player.transform.position.x + xAdj < minX ? minX : player.transform.position.x + xAdj;
        float y = player.transform.position.y < minY ? minY : player.transform.position.y;
        transform.position = new Vector3(x, y, -10f);
//
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
//        DrawLine(0, bottomMiddle, topMiddle, Color.green);
//        DrawLine(1, bottomMiddle2, topMiddle2, Color.red);
//
//        Vector3 playerPos = player.transform.position;
//
//        float distToGround = player.GetComponent<Collider2D>().bounds.extents.y + 0.1f;
//        float distToLeftRightSide = player.GetComponent<Collider2D>().bounds.extents.x - 0.25f;
//
//        Vector3 lineStartPos = new Vector3(playerPos.x - distToLeftRightSide, playerPos.y - distToGround, -1f);
//        Vector3 lineEndPos = new Vector3(playerPos.x + distToLeftRightSide, playerPos.y - distToGround, -1f);
//
//        DrawLine(2, lineStartPos, lineEndPos, Color.yellow);



        Vector3 jumpHeightLineStart = new Vector3(-100, player.GetComponent<Player>().yJumpTarget, -1f);
        Vector3 jumpHeightLineEnd = new Vector3(100, player.GetComponent<Player>().yJumpTarget, -1f);
        DrawLine(3, jumpHeightLineStart, jumpHeightLineEnd, Color.red);

    }


    void DrawLine(int lineIndex, Vector3 start, Vector3 end, Color color) {
        GameObject line = lines[lineIndex];
        line.transform.position = start;
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.SetColors(color, color);
        lr.SetWidth(0.05f, 0.05f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lines[lineIndex] = line;
    }
}
