using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    private int gridX;
    [SerializeField]
    private int gridZ;
    [SerializeField]
    private int offset;
    [SerializeField]
    private GameObject gridCell;

    [Header("Hallway settings")]
    [SerializeField]
    private GameObject hallway;
    [SerializeField]
    private List<GameObject> hallways = new List<GameObject>();

    [Header("Room Settings")]
    [SerializeField]
    private GameObject spawnRoom;
    [SerializeField]
    private GameObject bossRoom;
    [SerializeField]
    private List<GameObject> rooms;


    [Header("Grid Cell Materials")]
    [SerializeField]
    private Material occupiedCellMaterial;
    [SerializeField]
    private Material doorCellMaterial;

    [HideInInspector]
    public List<Cell> cells;

    private GameObject stageParent;

    private List<Hallway> mapHallways = new List<Hallway>();
    private List<Room> mapRooms = new List<Room>();

    private void Start()
    {
        //Create empty gameobject called Stage to store the cells in
        stageParent = new GameObject("Stage");

        //Loop through the X and Z to create the grid
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                SpawnCell(i, j);
            }
        }

        InitializeRoomSizes();

        //Start making the finite hallway
        StartCoroutine(HallwayGenerator());
    }

    //Spawn cell in the grid on the calculated location
    private void SpawnCell(int x, int z)
    {
        //Create the position of the cell to place in scene
        Vector3 cellPos = new Vector3(x * offset, -4f, z * offset);

        //Place the gameobject in the scene
        GameObject newCell = Instantiate(gridCell, cellPos, Quaternion.identity);

        //Set the cell X and Z position of the grid
        newCell.GetComponent<Cell>().x = x;
        newCell.GetComponent<Cell>().z = z;

        //Set the cell in the Stage gameobject and rename the cell to its position in the grid
        newCell.transform.parent = stageParent.transform;
        newCell.name = "Cell: X = " + x + ", Z = " + z;

        //Add the cell to the cells list
        cells.Add(newCell.GetComponent<Cell>());
    }

    //Generate the finite hallway in the game
    private IEnumerator HallwayGenerator()
    {
        int hallwayX = (int)hallway.GetComponent<Room>().x;
        int hallwayZ = (int)hallway.GetComponent<Room>().z;

        //First hallway (function as spawn for now)
        GameObject initialHallway = InitialHallway();

        GameObject latestHallway = initialHallway;

        int maxHallways = Mathf.FloorToInt(gridZ - hallwayZ / offset) / (hallwayZ / offset);

        for (int i = 0; i < maxHallways; i++)
        {
            if (i == 0)
            {
                latestHallway = initialHallway;
            }

            int posX = (int)initialHallway.transform.position.x;
            int posZ = (int)latestHallway.transform.position.z + hallwayZ;

            latestHallway = Instantiate(hallway, new Vector3(posX, 0, posZ), Quaternion.identity);
            List<Cell> roomCells = SetRoomCells(latestHallway, posX, posZ);

            latestHallway.GetComponent<Hallway>().cells = roomCells;

            mapHallways.Add(latestHallway.GetComponent<Hallway>());
        }

        yield return StartCoroutine(RoomPlacement());
    }

    private GameObject InitialHallway()
    {
        int hallwayX = (int)hallway.GetComponent<Room>().x;
        int hallwayZ = (int)hallway.GetComponent<Room>().z;

        int posX = gridX * offset / 2;
        int posZ = hallwayZ / 2;

        if (hallwayX % 2 == 0)
        {
            posX = posX - 5;
        }

        if (hallwayX % 2 == 0)
        {
            posZ = posZ - 5;
        }

        GameObject _hallway = Instantiate(hallway, new Vector3(posX, 0, posZ), Quaternion.identity);
        List<Cell> roomCells = SetRoomCells(_hallway, posX, posZ);

        _hallway.GetComponent<Hallway>().cells = roomCells;
        _hallway.name = "Initial hallway (temp spawn)";

        mapHallways.Add(_hallway.GetComponent<Hallway>());

        return _hallway;
    }

    private IEnumerator RoomPlacement()
    {
        for (int i = 0; i < mapHallways.Count; i++)
        {
            foreach (var _door in mapHallways[i].GetDoors())
            {
                int placeRoom = Random.Range(0, 2);

                if (placeRoom == 1)
                {
                    Cell doorCellRoom = null;

                    int doorCellX = mapHallways[i].cells.Select(mh => mh.x).Distinct()
                                    .ToArray()[_door.GetComponent<Door>().roomPosXOffset - 1];
                    int doorCellZ = mapHallways[i].cells.Select(mh => mh.z).Distinct()
                                    .ToArray()[_door.GetComponent<Door>().roomPosZOffset - 1];

                    if (_door.GetComponent<Door>().rightSide)
                    {
                        doorCellRoom = cells.Where(c => c.x == doorCellX + 1 && c.z == doorCellZ).SingleOrDefault();
                    }
                    else
                    {
                        doorCellRoom = cells.Where(c => c.x == doorCellX - 1 && c.z == doorCellZ).SingleOrDefault();
                    }

                    doorCellRoom.gameObject.GetComponent<MeshRenderer>().material = doorCellMaterial;
                    _door.GetComponent<Door>().cell = doorCellRoom;

                    GameObject randomRoom = rooms[Random.Range(0, rooms.Count)];

                    if (_door.GetComponent<Door>().rightSide)
                    {
                        randomRoom = Instantiate(randomRoom, new Vector3(
                            doorCellRoom.transform.position.x + (randomRoom.GetComponent<Room>().x / 2 - 5), 0,
                            doorCellRoom.transform.position.z), Quaternion.identity);

                        Room _randomRoom = randomRoom.GetComponent<Room>();

                        _randomRoom.cells = SetRoomCells(randomRoom, (int)doorCellRoom.transform.position.x + offset, (int)doorCellRoom.transform.position.z);
                    }
                    else
                    {
                        randomRoom = Instantiate(randomRoom, new Vector3(
                            doorCellRoom.transform.position.x - (randomRoom.GetComponent<Room>().x / 2 - 5), 0,
                            doorCellRoom.transform.position.z), Quaternion.identity);

                        Room _randomRoom = randomRoom.GetComponent<Room>();

                        _randomRoom.cells = SetRoomCells(randomRoom, (int)doorCellRoom.transform.position.x - offset, (int)doorCellRoom.transform.position.z);
                    }

                    _door.GetComponent<Door>().gameObject.SetActive(false);
                }
            }
        }

        yield return null;
    }

    //Set the room cells that are occupied by it
    private List<Cell> SetRoomCells(GameObject room, int posX, int posZ)
    {
        //Create a new list to store the cells in
        List<Cell> roomCells = new List<Cell>();

        Room _room = room.GetComponent<Room>();

        //Get the room X and Z length
        int roomX = (int)_room.x / offset;
        int roomZ = (int)_room.z / offset;

        //Get the position X and Z of the cell in the scene
        int cellPosX = (posX - 5) / offset;
        int cellPosZ = (posZ - 5) / offset;

        //Get the starting width and height of the cells you need for the room
        int cellWidth = cellPosX - (roomX / 2 - 1);
        int cellHeight = cellPosZ - (roomZ / 2 - 1);

        //Loop through cells based on start position and length of room
        for (int i = cellWidth; i < cellWidth + roomX; i++)
        {
            for (int j = cellHeight; j < cellHeight + roomZ; j++)
            {
                Cell cell = cells.Where(c => c.x == i && c.z == j && c.isOccupied == false).SingleOrDefault();

                if (cell == null)
                {
                    Debug.Log("No cell");
                }
                else
                {
                    if (_room.GetComponent<StandardRoom>())
                    {
                        cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    }
                    else
                    {
                        cell.gameObject.GetComponent<MeshRenderer>().material = occupiedCellMaterial;
                    }
                    cell.isOccupied = true;

                    roomCells.Add(cell);
                }
            }
        }

        return roomCells;
    }

    private void InitializeRoomSizes()
    {
        List<GameObject> allRooms = new List<GameObject>();

        allRooms.Add(hallway);
        // allRooms.Add(spawnRoom);
        // allRooms.Add(bossRoom);
        allRooms.AddRange(rooms);

        foreach (var _room in allRooms)
        {
            var newSizes = RoomUtil.CalculateSizeBasedOnChildren(_room);
            var roomComp = _room.GetComponent<Room>();

            roomComp.x = newSizes["x"];
            roomComp.y = newSizes["y"];
            roomComp.z = newSizes["z"];
        }
    }
}
