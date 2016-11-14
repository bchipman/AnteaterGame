using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour {

    public GameObject koopaPrefab;
    public GameObject shyGuyPrefab;
    public GameObject bookPrefab;
    private float respawnDelay;
    private List<GameObject> enemiesToDestroy;

	public AudioClip enemyDead;
	private AudioSource playerAS;

	void Start () {
	    respawnDelay = 5f;
        enemiesToDestroy = new List<GameObject>();
	}
	
	void Update () {
	}

    public void KillEnemy(GameObject enemy) {
//        if (enemy.name.StartsWith("Koopa")) {
//            StartCoroutine(NewRespawn(koopaPrefab, respawnDelay));
//        } else if (enemy.name.StartsWith("ShyGuy")) {
//            StartCoroutine(NewRespawn(shyGuyPrefab, respawnDelay));
//        }

		playerAS = GetComponent<AudioSource> ();
		playerAS.Play ();

        Destroy(enemy.GetComponent<BoxCollider2D>());
        Destroy(enemy.GetComponent<CircleCollider2D>());
        enemiesToDestroy.Add(enemy);
        GameObject book = Instantiate(bookPrefab, enemy.transform.position, Quaternion.identity) as GameObject;
        book.transform.SetParent(transform.Find("/Collectables"));
        StartCoroutine(DestroyEnemyTimer());
    }

    IEnumerator NewRespawn(GameObject toRespawn, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        GameObject newEnemy = Instantiate(toRespawn, randomLocation(), Quaternion.identity) as GameObject;

        newEnemy.transform.SetParent(transform.Find("/Enemies"));
    }

    private Vector2 randomLocation() {
        float xPos = Random.Range(-5f, 3f);
        float yPos = Random.Range(0f, 3f);
        return new Vector2(xPos, yPos);
    }

    private IEnumerator DestroyEnemyTimer() {
        yield return new WaitForSeconds(3);
        foreach (GameObject enemy in enemiesToDestroy) {
            Destroy(enemy);
        }
    }

}
