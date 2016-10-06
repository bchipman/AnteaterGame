using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    public Player player;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
	void Start () {
	    spriteRenderer = GetComponent<SpriteRenderer>();
        float skyWidth = spriteRenderer.sprite.bounds.size.x;
	    float skyScale = transform.localScale.x;
//        Debug.Log("skyWidth: " + skyWidth);
//        Debug.Log("skyScale: " + skyScale);
//        Debug.Log("skyWidth * skyScale = " + skyWidth * skyScale);

//        for (int i = 0; i < 10; i++) {
//            GameObject instance = Instantiate(skySprite, new Vector3(i * skyWidth * skyScale, 0f, 0f), Quaternion.identity) as GameObject;
//            instance.transform.SetParent(this.transform);
//        }
	}

	// Update is called once per frame
	void Update () {
	    Bounds b = GetComponent<SpriteRenderer>().bounds;
//        Debug.Log("b.max.x: " + b.max.x);
	    if (player.transform.position.x > b.max.x) {
            GetComponent<SpriteRenderer>().transform.position = new Vector3(b.max.x, 0, 0);
	    } else if (player.transform.position.x < b.min.x) {
            GetComponent<SpriteRenderer>().transform.position = new Vector3(b.min.x, 0, 0);
	    }
	}

}
