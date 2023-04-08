using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class StageHelper : MonoBehaviour
{
    public static StageHelper instance;

    [HideInInspector]
    public enum roomDirections { Top, Right, Bottom, Left , Undefined };

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

    public static roomDirections RandomDirection(List<roomDirections> directions)
    {
        directions.Remove(roomDirections.Undefined);

        if (directions != null)
        {
            return directions[Random.Range(0, directions.Count)];
        }
        else
        {
            return roomDirections.Undefined;
        }
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

    public static void NavMeshBaker()
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
            data = NavMeshBuilder.BuildNavMeshData(settings, sources.ToList(), new Bounds(Vector3.zero, new Vector3(1000, 20, 2000)), Vector3.zero, Quaternion.identity);
            NavMesh.AddNavMeshData(data);
            Debug.Log("NavMesh baked successfully");
        }

        private static void AddSourcesFromObject(GameObject obj, ref NavMeshBuildSource[] sources)
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
