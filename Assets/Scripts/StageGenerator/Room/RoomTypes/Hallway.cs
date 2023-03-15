using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hallway : Room
{
    [SerializeField]
    private List<GameObject> doors;

    public List<GameObject> GetDoors()
    {
        return doors;
    }
}
