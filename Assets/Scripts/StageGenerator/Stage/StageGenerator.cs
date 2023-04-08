using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

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
    [SerializeField]
    private bool debugMode;

    [Header("Hallway settings")]
    [SerializeField]
    private List<GameObject> hallways = new List<GameObject>();

    [Header("Room Settings")]
    [SerializeField]
    private int minWeightRoomsBranch;
    [SerializeField]
    private int maxWeightRoomsBranch;
    [SerializeField]
    private GameObject spawnRoom;
    [SerializeField]
    private GameObject bossRoom;
    [SerializeField]
    private List<GameObject> rooms;
    // [SerializeField]
    // private int maxRooms;

    [HideInInspector]
    public List<Cell> cells;

    private GameObject stageParent;

    private List<Hallway> mapHallways = new List<Hallway>();
    private List<GameObject> mapRooms = new List<GameObject>();

    private RoomGenerator roomGenerator;

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

        StageHelper.SetGridX(gridX);
        StageHelper.SetGridZ(gridZ);
        StageHelper.SetOffset(offset);
        StageHelper.SetCells(cells);
        StageHelper.SetRooms(rooms);

        InitializeRoomSizes();

        roomGenerator = new RoomGenerator();

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

        if (!debugMode)
        {
            newCell.GetComponent<Renderer>().enabled = false;
        }

        //Add the cell to the cells list
        cells.Add(newCell.GetComponent<Cell>());
    }

    //Generate the finite hallway in the game
    private IEnumerator HallwayGenerator()
    {
        Room _spawnRoom = SpawnRoom().GetComponent<Room>();

        int totalHallwayZ = gridZ - (int)((spawnRoom.GetComponent<Room>().sizeZ / offset) + (bossRoom.GetComponent<Room>().sizeZ / offset));

        Room latestHallway = null;
        bool generateHallway = true;

        while (generateHallway)
        {
            GameObject chosenHallway = null;
            int weightTotal = hallways.Sum(h => h.GetComponent<Room>().GetWeight());

            chosenHallway = RoomUtil.GetRandomRoom(hallways, weightTotal);

            int hallwayX = (int)chosenHallway.GetComponent<Room>().sizeX;
            int hallwayZ = (int)chosenHallway.GetComponent<Room>().sizeZ;

            totalHallwayZ -= (hallwayZ / offset);

            if (totalHallwayZ <= 0)
            {
                generateHallway = false;
                break;
            }

            int posX = (int)_spawnRoom.transform.position.x;
            int posZ = 0;

            if (latestHallway == null)
            {
                posZ = (int)hallwayZ / 2 + (int)_spawnRoom.GetComponent<Room>().sizeZ - 5;
            }
            else
            {
                posZ = (int)(latestHallway.transform.position.z + latestHallway.sizeZ / 2) + (hallwayZ / 2);
            }

            latestHallway = Instantiate(chosenHallway, new Vector3(posX, 0, posZ), Quaternion.identity).GetComponent<Room>();
            
            List<Cell> roomCells = SetRoomCells(chosenHallway, posX, posZ);
            latestHallway.GetComponent<Room>().cells = roomCells;

            mapHallways.Add(latestHallway.GetComponent<Hallway>());
        }

        SpawnBossRoom(latestHallway.gameObject);

        // yield return null;
        yield return StartCoroutine(GenerateRooms());
    }

    private IEnumerator GenerateRooms()
    {
        roomGenerator.BranchRoomGeneration(mapHallways, minWeightRoomsBranch, maxWeightRoomsBranch);

        yield return StartCoroutine(ReplaceDoors());
    }
    
    private IEnumerator ReplaceDoors()
    {
        foreach (GameObject room in mapRooms)
        {
            if (room.GetComponent<Room>().GetDoorReplacement() != null)
            {
                StageHelper.ReplaceAllDoors(room);
            }
        }
        yield return StartCoroutine(StageNavMeshBaker());
    }

    private IEnumerator StageNavMeshBaker()
    {
        StageHelper.NavMeshBaker();
        yield return StartCoroutine(StageEnemyGeneration());
    }

    private IEnumerator StageEnemyGeneration()
    {
        foreach (GameObject room in mapRooms)
        {
            if (room.GetComponent<EnemyGeneration>() != null)
            {
                room.GetComponent<EnemyGeneration>().SpawnEnemy();
            }
        }
        yield return null;
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
                        cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
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

        allRooms.Add(spawnRoom);
        allRooms.Add(bossRoom);
        allRooms.AddRange(rooms);
        allRooms.AddRange(hallways);

        foreach (var _room in allRooms)
        {
            var newSizes = RoomUtil.CalculateSizeBasedOnChildren(_room);
            var roomComp = _room.GetComponent<Room>();

            roomComp.sizeX = newSizes["x"];
            roomComp.sizeY = newSizes["y"];
            roomComp.sizeZ = newSizes["z"];
        }
    }

    public void AddRoomToMap(GameObject room)
    {
        mapRooms.Add(room);
    }
}