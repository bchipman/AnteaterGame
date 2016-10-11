using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour {

    public GameObject koopaPrefab;
    public GameObject shyGuyPrefab;
    private float respawnDelay;

	void Start () {
	    respawnDelay = 5f;
	}
	
	void Update () {
	}

    public void KillEnemy(GameObject enemy) {
        if (enemy.name.StartsWith("Koopa")) {
            StartCoroutine(NewRespawn(koopaPrefab, respawnDelay));
        } else if (enemy.name.StartsWith("ShyGuy")) {
            StartCoroutine(NewRespawn(shyGuyPrefab, respawnDelay));
        }
        Destroy(enemy);
    }

    IEnumerator NewRespawn(GameObject toRespawn, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        GameObject newEnemy = Instantiate(toRespawn, Vector3.zero, Quaternion.identity) as GameObject;
    }


    
}
