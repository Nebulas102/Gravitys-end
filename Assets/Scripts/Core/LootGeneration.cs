using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class LootGeneration : MonoBehaviour
    {
        [SerializeField]
        private List<Item> lootObjects;

        private Vector3 _spawnPointPos;

        public IEnumerator SpawnLoot(GameObject spawnRoom)
        {
            _spawnPointPos = gameObject.transform.position;
            // Spawns amount of loot objects based on the amount of loot objects in the list
            foreach (var item in lootObjects)
            {
                var xOffset = Random.Range(-12, 12);
                var zOffset = Random.Range(-12, 12);
                Instantiate(item.prefab, new Vector3(_spawnPointPos.x - xOffset, 0.5f, _spawnPointPos.z - zOffset),
                    Quaternion.identity);
                // Waits 0.1 seconds before spawning the next loot object because of performance
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
