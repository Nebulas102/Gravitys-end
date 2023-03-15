using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    [HideInInspector]
    public List<Cell> cells;

    [SerializeField]
    private int spawnChance;

    public int GetSpawnChance()
    {
        return spawnChance;
    }
}
