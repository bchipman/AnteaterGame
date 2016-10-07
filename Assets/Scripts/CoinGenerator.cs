using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinGenerator : MonoBehaviour {

    private Vector2 initialPos = new Vector2(-8f, -1.5f);
    public GameObject coinSprite;
    private int xSep = 1;
    private int ySep = 1;
	
	void Start () {
	    for (int i = 0; i < 10; i++) {
	        for (int j = 0; j < 5; j++) {
	            GameObject instance = Instantiate(coinSprite, new Vector3(i * xSep + initialPos.x, j * ySep +initialPos.y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(this.transform);
            }
        }
	    
	}
	
	void Update () {
	
	}
}
