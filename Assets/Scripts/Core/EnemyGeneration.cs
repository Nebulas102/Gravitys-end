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
            for (int i = 0; i < enemySpawnPoints.Length; i++)
            {
                groupSize = Random.Range(minGroupSize, maxGroupSize + 1);
                for (int _ = 0; _ < groupSize; _++)
                {
                    spawnPosition = enemySpawnPoints[i].position;
                    spawnPosition.x += Random.Range(-1f, 1f); // Slightly randomize the X position so enemies dont collide upon spawning
                    Instantiate(enemy, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        public void SpawnEnemy()
        {
            StartCoroutine(EnemyDrop());
        }

    }
}
