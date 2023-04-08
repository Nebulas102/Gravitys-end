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
        private int xOffset;
        private int zOffset;
        public Vector3 spawnpointPos;
        public int enemyCount;

        void Start()
        {
            // Create walkable surface for enemies
            StartCoroutine(NavMeshBaker());
            // Spawn enemies
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

        IEnumerator NavMeshBaker()
        {
            // Find all game objects with the specified tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Room");

            // Create a NavMeshBuildSettings object
            NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);

            // Create an array to hold the NavMeshBuildSources
            NavMeshBuildSource[] sources = new NavMeshBuildSource[0];

            // Iterate through all the tagged game objects and their children
            foreach (GameObject obj in taggedObjects)
            {
                AddSourcesFromObject(obj, ref sources);
            }

            // Build the NavMesh
            NavMeshData data = new NavMeshData();
            data = NavMeshBuilder.BuildNavMeshData(settings, sources.ToList(), new Bounds(Vector3.zero, new Vector3(1000, 20, 1000)), Vector3.zero, Quaternion.identity);
            NavMesh.AddNavMeshData(data);
            Debug.Log("NavMesh baked successfully");

            yield return null;
        }

        private void AddSourcesFromObject(GameObject obj, ref NavMeshBuildSource[] sources)
        {
            MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();

            // Add a NavMeshBuildSource for each mesh filter
            foreach (MeshFilter filter in meshFilters)
            {
                if (obj.tag == "Floor" || obj.tag == "Wall" || obj.tag == "Door")
                {
                    NavMeshBuildSource source = new NavMeshBuildSource()
                    {
                        transform = filter.transform.localToWorldMatrix,
                        shape = NavMeshBuildSourceShape.Mesh,
                        sourceObject = filter.sharedMesh,
                        area = 0
                    };

                    // Add the NavMeshBuildSource to the sources array
                    ArrayUtility.Add(ref sources, source);
                }
            }

            // Recursively add sources from all children
            foreach (Transform child in obj.transform)
            {
                AddSourcesFromObject(child.gameObject, ref sources);
            }
        }
    }
}
