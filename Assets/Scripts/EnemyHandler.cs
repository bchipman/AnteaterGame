using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour {

    public GameObject koopaPrefab;
    public GameObject shyGuyPrefab;

	void Start () {
	}
	
	void Update () {
	}

    public void KillEnemy(GameObject enemy) {
        if (enemy.name.StartsWith("Koopa")) {
            StartCoroutine(NewRespawn(koopaPrefab, 2f));
        } else if (enemy.name.StartsWith("ShyGuy")) {
            StartCoroutine(NewRespawn(shyGuyPrefab, 2f));
        }
        Destroy(enemy);
    }

    IEnumerator NewRespawn(GameObject toRespawn, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        GameObject newEnemy = Instantiate(toRespawn, Vector3.zero, Quaternion.identity) as GameObject;
    }


    
}
