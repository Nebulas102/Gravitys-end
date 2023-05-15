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
        public GameObject enemy;
        [SerializeField]
        private Transform[] enemySpawnPoints;
        private Vector3 spawnPosition;
        private int groupSize;
        [SerializeField]
        private int minGroupSize;
        [SerializeField]
        private int maxGroupSize;
        

        void Start()
        {

        }

        IEnumerator EnemyDrop()
        {
            List<Transform> availableSpawnPoints = new List<Transform>(enemySpawnPoints);
            int spawnPointCount = availableSpawnPoints.Count;

            for (int i = 0; i < 3 && spawnPointCount > 0; i++)
            {
                int randomIndex = Random.Range(0, spawnPointCount);
                groupSize = Random.Range(minGroupSize, maxGroupSize + 1);
                spawnPosition = availableSpawnPoints[randomIndex].position;

                for (int _ = 0; _ < groupSize; _++)
                {
                    spawnPosition.x += Random.Range(-1f, 1f);
                    Instantiate(enemy, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(0.1f);
                }

                availableSpawnPoints.RemoveAt(randomIndex);
                spawnPointCount--;
            }
        }


        public void SpawnEnemy()
        {
            StartCoroutine(EnemyDrop());
        }

    }
}
