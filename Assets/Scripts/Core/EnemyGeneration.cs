using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class EnemyGeneration : MonoBehaviour
    {
        public GameObject enemy;
        private int xOffset;
        private int zOffset;
        public Vector3 spawnpointPos;
        public int enemyCount;

        void Start()
        {
            spawnpointPos = gameObject.transform.position;
            Debug.Log(spawnpointPos.x);
            StartCoroutine(EnemyDrop());
        }

        // Spawns enemies at random x and z coordinates
        IEnumerator EnemyDrop()
        {
            // Spawns certain amount of enemies, in this case 5 (can also be randomized)
            while (enemyCount < 5)
            {
                // Determines where enemies can spawn, will spawn anywhere within the given x and z range
                // Based on how map generation is handled this will of course change, just hard coded for testing purposes

                xOffset = Random.Range(-12, 12);
                zOffset = Random.Range(-12, 12);
                // Spawns enemy, enemy type can be selected in Unity
                Instantiate(enemy, new Vector3(spawnpointPos.x-xOffset, 0, spawnpointPos.z-zOffset), Quaternion.identity);
                // Waits a bit before spawning next enemy
                yield return new WaitForSeconds(0.1f);
                enemyCount += 1;
            }
        }
    }
}
