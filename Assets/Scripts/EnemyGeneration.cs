using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    // Spawns enmies at random x and z coordinates
    IEnumerator EnemyDrop()
    {
        // Spawns certain amount of enemies, in this case 3
        while (enemyCount < 3)
        {
            // Determines where enemies can spawn, will spawn anywhere within the given x and z range
            // Based on how map generation is handled this will of course change, just hard coded for testing purposes
            xPos = Random.Range(-6, 6);
            zPos = Random.Range(1, 8);
            // Spawns enemy, enemy type can be selected in Unity
            Instantiate(enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            // Waits a bit before spawning next enemy
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

}
