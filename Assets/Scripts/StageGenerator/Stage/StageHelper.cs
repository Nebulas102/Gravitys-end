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
}
