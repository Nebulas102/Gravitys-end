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

        //Initial spawnroom
        GameObject spawnRoom = SpawnRoom();

        GameObject latestHallway = null;

        int priorRoomsZ = ((int)spawnRoom.GetComponent<Room>().sizeZ / offset) + ((int)bossRoom.GetComponent<Room>().sizeZ / offset);
        int maxHallways = Mathf.FloorToInt((gridZ - priorRoomsZ) / (hallwayZ / offset));

        for (int i = 0; i < maxHallways; i++)
        {
            int posX = (int)spawnRoom.transform.position.x;
            int posZ = 0;

            if (latestHallway == null)
            {
                posZ = (int)hallwayZ / 2 + (int)spawnRoom.GetComponent<Room>().sizeZ - 5;
            }
            else
            {
                posZ = (int)latestHallway.transform.position.z + hallwayZ;
            }

            latestHallway = Instantiate(hallway, new Vector3(posX, 0, posZ), Quaternion.identity);
            
            List<Cell> roomCells = SetRoomCells(latestHallway, posX, posZ);
            latestHallway.GetComponent<Room>().cells = roomCells;

            mapHallways.Add(latestHallway.GetComponent<Hallway>());
        }

        SpawnBossRoom(latestHallway);

        yield return StartCoroutine(RoomPlacement());
    }

    private GameObject SpawnRoom()
    {
        int spawnRoomX = (int)spawnRoom.GetComponent<Room>().sizeX;
        int spawnRoomZ = (int)spawnRoom.GetComponent<Room>().sizeZ;

        int posX = gridX * offset / 2;
        int posZ = spawnRoomZ / 2 - 5;

        GameObject _spawnRoom = Instantiate(spawnRoom, new Vector3(posX, 0, posZ), Quaternion.identity);
        List<Cell> roomCells = SetRoomCells(_spawnRoom, posX, posZ);

        _spawnRoom.GetComponent<Room>().cells = roomCells;
        _spawnRoom.name = "Spawn Room";

        _spawnRoom.GetComponent<Room>().GetDoors()[0].SetActive(false);

        return _spawnRoom;
    }

    private GameObject SpawnBossRoom(GameObject lastHallway)
    {
        int spawnBossRoomX = (int)bossRoom.GetComponent<Room>().sizeX;
        int spawnBossRoomZ = (int)bossRoom.GetComponent<Room>().sizeZ;

        int posX = gridX * offset / 2;
        int posZ = (int)lastHallway.transform.position.z + ((int)lastHallway.GetComponent<Room>().sizeZ / 2)
                    + (spawnBossRoomZ / 2);

        GameObject _bossRoom = Instantiate(bossRoom, new Vector3(posX, 0, posZ), Quaternion.identity);
        List<Cell> roomCells = SetRoomCells(_bossRoom, posX, posZ);

        _bossRoom.GetComponent<Room>().cells = roomCells;
        _bossRoom.name = "Boss Room";

        _bossRoom.GetComponent<Room>().GetDoors()[0].SetActive(false);

        return _bossRoom;
    }

    private IEnumerator RoomPlacement()
    {
        for (int i = 0; i < mapHallways.Count; i++)
        {
            mapHallways[i].GetComponent<Room>().SetDoorCells();

            foreach (var _door in mapHallways[i].GetDoors())
            {
                if (Random.Range(0, 2) == 1)
                {
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Count)];

                    randomRoom.GetComponent<Room>().PlaceRooms(_door);

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
                    else if (_room.GetComponent<SpawnRoom>())
                    {
                        cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
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
        allRooms.Add(spawnRoom);
        allRooms.Add(bossRoom);
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
