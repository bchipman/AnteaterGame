using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private float minX = 0f;
    private float minY = 0f;

    void Update () {
        float x = player.transform.position.x < minX ? minX : player.transform.position.x;
        float y = player.transform.position.y < minY ? minY : player.transform.position.y;
        transform.position = new Vector3(x, y, -10f);
    }
}
