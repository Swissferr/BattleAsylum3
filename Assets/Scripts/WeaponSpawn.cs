using UnityEngine;
using System.Collections;

public class WeaponSpawn : MonoBehaviour {

    public Vector2 valeursSpawn;
    public float startWait;
    public float waveWait; 
    public GameObject[] armes;

	void Start () {
        StartCoroutine(SpawnWave(armes));
	}
	
	IEnumerator SpawnWave(GameObject[] armes) {
        yield return new WaitForSeconds(startWait);
        while (true) {
            Vector2 spawnPosition = new Vector2(Random.Range(-valeursSpawn.x,valeursSpawn.x), valeursSpawn.y);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(armes[Random.Range(0, armes.Length)], spawnPosition, spawnRotation);
            yield return new WaitForSeconds(waveWait);
        }
    }

}
