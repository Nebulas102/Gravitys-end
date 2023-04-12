using System;
using System.Collections.Generic;
using System.Linq;
using StageGeneration.Rooms;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StageGeneration.Stage
{
    public class StageHelper : MonoBehaviour
    {
        [HideInInspector]
        public enum roomDirections
        {
            Top,
            Right,
            Bottom,
            Left,
            Undefined
        }

        public static StageHelper instance;

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
            gridZ = _gridZ;
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
            return (roomDirections)Random.Range(0, Enum.GetValues(typeof(roomDirections)).Length);
        }

        public static roomDirections RandomDirection(List<roomDirections> directions)
        {
            directions.Remove(roomDirections.Undefined);

            if (directions != null)
                return directions[Random.Range(0, directions.Count)];
            return roomDirections.Undefined;
        }

        public static roomDirections GetOppositeDirection(roomDirections direction)
        {
            //Cant be null so I gave just a value that will always be changed
            var oppositeDirection = roomDirections.Top;

            switch (direction)
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
            var doors = room.GetComponent<Room>().GetDoors().Where(d => d.activeSelf).ToList();

            foreach (var _door in doors)
            {
                var doorPos = _door.transform.position;
                var doorRot = _door.transform.rotation;

                Destroy(_door);

                var wall = Instantiate(room.GetComponent<Room>().GetDoorReplacement(), doorPos, doorRot);

                wall.transform.parent = room.transform;
            }
        }

        public static void NavMeshBaker()
        {
            // Find all game objects with the specified tag
            var taggedObjects = GameObject.FindGameObjectsWithTag("Room");

            // Create a NavMeshBuildSettings object
            var settings = NavMesh.GetSettingsByID(0);

            // Create an array to hold the NavMeshBuildSources
            var sources = new NavMeshBuildSource[0];

            // Iterate through all the tagged game objects and their children
            foreach (var obj in taggedObjects) AddSourcesFromObject(obj, ref sources);

            var centerCell = CalculateCenterStage();

            // Build the NavMesh
            var data = new NavMeshData();
            data = NavMeshBuilder.BuildNavMeshData(settings, sources.ToList(), new Bounds(
                centerCell.gameObject.transform.position,
                new Vector3(gridX * offset, 30, gridZ * offset)), Vector3.zero, Quaternion.identity);
            NavMesh.AddNavMeshData(data);
        }

        private static Cell CalculateCenterStage()
        {
            float calcX = 0;
            float calcZ = 0;

            if (gridX % 2 == 0)
                calcX = gridX / 2 - 1;
            else
                calcX = Mathf.RoundToInt(gridX / 2 - 1);

            if (gridZ % 2 == 0)
                calcZ = gridZ / 2 - 1;
            else
                calcZ = Mathf.RoundToInt(gridZ / 2 - 1);

            return cells.Where(c => c.x == calcX && c.z == calcZ).SingleOrDefault();
        }

        private static void AddSourcesFromObject(GameObject obj, ref NavMeshBuildSource[] sources)
        {
            var meshFilters = obj.GetComponentsInChildren<MeshFilter>();

            // Add a NavMeshBuildSource for each mesh filter
            foreach (var filter in meshFilters)
                if (obj.tag == "Floor" || obj.tag == "Wall" || obj.tag == "Door" && obj.activeSelf)
                {
                    var source = new NavMeshBuildSource
                    {
                        transform = filter.transform.localToWorldMatrix,
                        shape = NavMeshBuildSourceShape.Mesh,
                        sourceObject = filter.sharedMesh,
                        area = 0
                    };

                    // Add the NavMeshBuildSource to the sources array
                    ArrayUtility.Add(ref sources, source);
                }

            // Recursively add sources from all children
            foreach (Transform child in obj.transform) AddSourcesFromObject(child.gameObject, ref sources);
        }
    }
}
