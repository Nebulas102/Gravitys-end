using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomPosXOffset;
    public int roomPosZOffset;

    public Cell cell;

    public bool hasNeighbour = false;

    [SerializeField]
    private StageHelper.roomDirections direction;

    public StageHelper.roomDirections GetDirection()
    {
        return direction;
    }
}
