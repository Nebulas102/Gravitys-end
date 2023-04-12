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
            // Spawns certain amount of enemies, in this case 5 (can also be randomized)
            foreach (var item in lootObjects)
            {
                // Determines where enemies can spawn, will spawn anywhere within the given x and z range
                // Based on how map generation is handled this will of course change, just hard coded for testing purposes
                var xOffset = Random.Range(-12, 12);
                var zOffset = Random.Range(-12, 12);
                // Spawns enemy, enemy type can be selected in Unity
                Instantiate(item.prefab, new Vector3(_spawnPointPos.x - xOffset, 0.5f, _spawnPointPos.z - zOffset),
                    Quaternion.identity);
                // Waits a bit before spawning next enemy
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
