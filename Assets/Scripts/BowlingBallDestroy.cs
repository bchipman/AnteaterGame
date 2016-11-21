using UnityEngine;
using System.Collections;

public class BowlingBallDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -9)Destroy (gameObject);
	}

}
