using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public List<Cell> cells;

    public float sizeX;
    public float sizeY;
    public float sizeZ;
}
