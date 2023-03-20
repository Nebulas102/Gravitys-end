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
    [SerializeField]
    private int maxRooms;


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

        StageHelper.gridX = gridX;
        StageHelper.gridZ = gridZ;
        StageHelper.offset = offset;
        StageHelper.cells = cells;

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
        int hallwayX = (int)hallway.GetComponent<Room>().sizeX;
        int hallwayZ = (int)hallway.GetComponent<Room>().sizeZ;

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

            List<Cell> roomCells = SetRoomCells(latestHallway, posX, posZ);
            latestHallway = Instantiate(hallway, new Vector3(posX, 0, posZ), Quaternion.identity);

            latestHallway.GetComponent<Hallway>().cells = roomCells;

            mapHallways.Add(latestHallway.GetComponent<Hallway>());
        }

        yield return StartCoroutine(RoomPlacement());
    }

    private GameObject InitialHallway()
    {
        int hallwayX = (int)hallway.GetComponent<Room>().sizeX;
        int hallwayZ = (int)hallway.GetComponent<Room>().sizeZ;

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
            mapHallways[i].GetComponent<Room>().SetDoorCells();

            foreach (var _door in mapHallways[i].GetDoors())
            {   
                // Room previousRoom = mapHallways[i].GetComponent<Room>();

                if (Random.Range(0, 2) == 1)
                {
                    // Cell doorCellRoom = null;

                    // int doorCellX = mapHallways[i].cells.Select(mh => mh.x).Distinct()
                    //                 .ToArray()[_door.GetComponent<Door>().roomPosXOffset];
                    // int doorCellZ = mapHallways[i].cells.Select(mh => mh.z).Distinct()
                    //                 .ToArray()[_door.GetComponent<Door>().roomPosZOffset];

                    // doorCellRoom = cells.Where(c => c.x == doorCellX && c.z == doorCellZ).SingleOrDefault();
            
                    // doorCellRoom.gameObject.GetComponent<MeshRenderer>().material = doorCellMaterial;
                    // doorCellRoom.gameObject.name = "Door Cell";

                    // _door.GetComponent<Door>().cell = doorCellRoom;

                    GameObject randomRoom = rooms[Random.Range(0, rooms.Count)];

                    randomRoom.GetComponent<Room>().PlaceRooms(_door);


                    // if (_door.GetComponent<Door>().GetDirection() == StageHelper.roomDirections.Right)
                    // {
                    //     float roomX = doorCellRoom.transform.position.x + (randomRoom.GetComponent<Room>().sizeX / 2 - 5 + offset);
                    //     float roomZ = doorCellRoom.transform.position.z;

                    //     randomRoom = Instantiate(randomRoom, new Vector3(roomX, 0, roomZ), Quaternion.identity);

                    //     Room _randomRoom = randomRoom.GetComponent<Room>();

                    //     _randomRoom.cells = SetRoomCells(randomRoom, (int)roomX, (int)roomZ);
                    // }
                    // else
                    // {
                    //     float roomX = doorCellRoom.transform.position.x - (randomRoom.GetComponent<Room>().sizeX / 2 - 5 + offset);
                    //     float roomZ = doorCellRoom.transform.position.z;

                    //     randomRoom = Instantiate(randomRoom, new Vector3(roomX, 0, roomZ), Quaternion.identity);

                    //     Room _randomRoom = randomRoom.GetComponent<Room>();

                    //     _randomRoom.cells = SetRoomCells(randomRoom, (int)roomX, (int)roomZ);
                    // }

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
        int roomX = (int)_room.sizeX / offset;
        int roomZ = (int)_room.sizeZ / offset;

        int cellPosX = (posX % offset == 5 ? posX - 5 : posX) / offset;
        int cellPosZ = (posZ % offset == 5 ? posZ - 5 : posZ) / offset;

        // cellPosX = roomX % 2 != 0 ? cellPosX : cellPosX + 1;
        // cellPosZ = roomZ % 2 != 0 ? cellPosZ : cellPosZ + 1;

        //Get the position X and Z of the cell in the scene
        int startPosX = cellPosX - roomX / 2;
        int startPosZ = cellPosZ - roomZ / 2;

        //Get the starting width and height of the cells you need for the room
        int endPosX = cellPosX + roomX / 2;
        int endPosZ = cellPosZ + roomZ / 2;

        //Loop through cells based on start position and length of room
        for (int i = startPosX; i <= endPosX; i++)
        {
            for (int j = startPosZ; j <= endPosZ; j++)
            {
                Cell cell = cells.Where(c => c.x == i && c.z == j && c.isOccupied == false).SingleOrDefault();

                if (cell == null)
                {
                    Debug.Log("No cell x" + i + " z" + j);
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

            roomComp.sizeX = newSizes["x"];
            roomComp.sizeY = newSizes["y"];
            roomComp.sizeZ = newSizes["z"];
        }
    }
}
