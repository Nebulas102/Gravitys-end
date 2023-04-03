using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageHelper : MonoBehaviour
{
    public static StageHelper instance;

    [HideInInspector]
    public enum roomDirections { Top, Right, Bottom, Left };

    private static int gridX;
    private static int gridZ;
    private static int offset;
    private static List<Cell> cells;
    private static List<GameObject> rooms;

    private void Awake() 
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public static int GetGridX()
    {
        return gridX;
    }

    public static void SetGridX(int _gridX)
    {
        gridX = _gridX;
    }

    public static int GetGridZ()
    {
        return gridZ;
    }

    public static void SetGridZ(int _gridZ)
    {
        gridX = _gridZ;
    }

    public static int GetOffset()
    {
        return offset;
    }

    public static void SetOffset(int _offset)
    {
        offset = _offset;
    }

    public static List<Cell> GetCells()
    {
        return cells;
    }

    public static void SetCells(List<Cell> _cells)
    {
        cells = _cells;
    }

    public static List<GameObject> GetRooms()
    {
        return rooms;
    }

    public static void SetRooms(List<GameObject> _rooms)
    {
        rooms = _rooms;
    }

    public static roomDirections RandomDirection()
    {
        return (roomDirections)Random.Range(0, System.Enum.GetValues(typeof(roomDirections)).Length);
    }

    public static roomDirections GetOppositeDirection(roomDirections direction)
    {
        //Cant be null so I gave just a value that will always be changed
        roomDirections oppositeDirection = roomDirections.Top;

        switch(direction)
        {
            case roomDirections.Top:
                oppositeDirection = roomDirections.Bottom;
            break;
            case roomDirections.Right:
                oppositeDirection = roomDirections.Left;
            break;  
            case roomDirections.Bottom:
                oppositeDirection = roomDirections.Top;
            break;  
            case roomDirections.Left:
                oppositeDirection = roomDirections.Right;
            break;  
        }

        return oppositeDirection;
    }

    public static void ReplaceAllDoors(GameObject room)
    {
        var doors = room.GetComponent<Room>().GetDoors().Where(d => d.activeSelf == true).ToList();

        foreach (GameObject _door in doors)
        {
            Vector3 doorPos = _door.transform.position;
            Quaternion doorRot = _door.transform.rotation;

            Destroy(_door);

            GameObject wall = Instantiate(room.GetComponent<Room>().GetDoorReplacement(), doorPos, doorRot);

            wall.transform.parent = room.transform;
        }
    }
}
