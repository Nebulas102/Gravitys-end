using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageHelper : MonoBehaviour
{
    public static StageHelper instance;

    [HideInInspector]
    public enum roomDirections { Top, Right, Bottom, Left };

    [HideInInspector]
    public static int gridX;
    [HideInInspector]
    public static int gridZ;
    [HideInInspector]
    public static int offset;
    [HideInInspector]
    public static List<Cell> cells;

    private void Awake() 
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
}
