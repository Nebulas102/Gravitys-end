using System.Collections.Generic;
using UnityEngine;

namespace Core.Chest
{
    public class ChestSpawner : MonoBehaviour
    {
        [Range(0f, 1f)] public float chestSpawnChance = 0.15f;

        [SerializeField] public Transform[] chestSpawnPoints;

        [SerializeField] public GameObject chest;

        [SerializeField] List<GameObject> possibleLoot;

        private bool _chestWillSpawn;

        private Transform _spawnPointPos;

        private void DetermineChestSpawn()
        {
            var randValue = Random.value;

            // It will spawn a chest X% of time which is set in the inspector
            _chestWillSpawn = randValue < chestSpawnChance;
        }

        public void SpawnChest()
        {
            DetermineChestSpawn();

            if (_chestWillSpawn)
            {
                var chestSpawnPointKey = Random.Range(0, chestSpawnPoints.Length - 1);

                _spawnPointPos = chestSpawnPoints[chestSpawnPointKey];

                GameObject spawnedChest = Instantiate(chest, _spawnPointPos);
                spawnedChest.GetComponent<Chest>().SetLootObjects(possibleLoot);
            }
        }
    }
}