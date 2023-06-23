using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class EnemyGeneration : MonoBehaviour
    {
        [SerializeField]
        private List<EnemySpawnpoints> enemySpawnpoints;
        private int randomEnemy;

        IEnumerator EnemyDrop()
        {
            // Iterate through each EnemySpawnpoints object
            foreach (EnemySpawnpoints spawnpoint in enemySpawnpoints)
            {
                randomEnemy = UnityEngine.Random.Range(0, 3);
                // Generate a random number of enemies to spawn between minSpawn and maxSpawn
                int numEnemiesToSpawn = UnityEngine.Random.Range(spawnpoint.minSpawn, spawnpoint.maxSpawn + 1);

                // Spawn the enemies
                if (numEnemiesToSpawn == 1)
                {
                    Instantiate(spawnpoint.enemy[randomEnemy], spawnpoint.spawnpoint.position, spawnpoint.spawnpoint.rotation);
                }
                else if (numEnemiesToSpawn > 1) 
                {
                    for (int i = 0; i < numEnemiesToSpawn; i++)
                    {
                        float randomOffsetX = UnityEngine.Random.Range(-1f, 1f);
                        Vector3 spawnPosition = spawnpoint.spawnpoint.position + new Vector3(randomOffsetX, 0f, 0f);
                        Instantiate(spawnpoint.enemy[randomEnemy], spawnPosition, spawnpoint.spawnpoint.rotation);
                        yield return null;
                    }
                }
                
                yield return null;
            }
        }

        public void SpawnEnemy()
        {
            StartCoroutine(EnemyDrop());
        }

    }
}
