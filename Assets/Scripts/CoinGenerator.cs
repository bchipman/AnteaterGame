using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinGenerator : MonoBehaviour {

    public GameObject coin;
    private Vector2 initialPos = new Vector2(-8f, -1.5f);
    private int xSep = 1;
    private int ySep = 1;

    private void Start () {
        // Create some coins
        for (int col = 0; col < 10; col++) {
	        for (int row = 0; row < 5; row++) {
	            GameObject instance = Instantiate(coin, new Vector3(col * xSep + initialPos.x, row * ySep +initialPos.y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(transform.Find("/Collectables"));
            }
        }
	}
	
}
