using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 initialOffset;
    private float minX = 0f;
    private float minY = 0f;

	void Start () {
//	    initialOffset = transform.position - player.transform.position;
        Debug.Log("Camera initialOffset: " + initialOffset);
	}


    void Update () {
        float x = player.transform.position.x < minX ? minX : player.transform.position.x;
        float y = player.transform.position.y < minY ? minY : player.transform.position.y;
//        transform.position = new Vector3(x, y, 0f) + initialOffset;
        transform.position = new Vector3(x, y, -10f);
    }
}
