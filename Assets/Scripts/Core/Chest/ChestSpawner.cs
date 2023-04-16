using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{

    [Range(0f, 1f)]
    public float chestSpawnChance = 0.15f;

    [SerializeField]
    public Transform[] chestSpawnPoints;

    private Transform roomPos;

    [SerializeField]
    public GameObject chest;

    private bool chestWillSpawn = false;

    private Vector3 spawnPointPos;


    void DetermineChestSpawn()
    {
        float randValue = Random.value;

        // It will spawn a chest X% of time which is set in the inspector
        if (randValue < chestSpawnChance)
        {
            chestWillSpawn = true;
        }
        else
        {
            chestWillSpawn = false;
        }
    }

    public void SpawnChest () {
        DetermineChestSpawn();

        if (chestWillSpawn) {
            int chestSpawnPointKey = Random.Range(0, chestSpawnPoints.Length);

            roomPos = gameObject.transform; // 0 0 0

            spawnPointPos = chestSpawnPoints[chestSpawnPointKey].position;
            Debug.Log(spawnPointPos);

            Instantiate(chest, roomPos.TransformPoint(spawnPointPos), Quaternion.identity);
            Debug.LogWarning("Chest spawned");
        } else {
            Debug.Log("Chest Not spawned");
        }
    }
}
