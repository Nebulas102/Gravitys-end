using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
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

    [SerializeField]
    private bool debugMode;

    [Header("Hallway settings")]
    [SerializeField]
    private List<GameObject> hallways = new();

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

    private readonly List<Hallway> mapHallways = new();
    private readonly List<GameObject> mapRooms = new();

    private RoomGenerator roomGenerator;

    private GameObject stageParent;

    private void Start()
    {
        //Create empty gameobject called Stage to store the cells in
        stageParent = new GameObject("Stage");

        //Loop through the X and Z to create the grid
        for (var i = 0; i < gridX; i++)
        for (var j = 0; j < gridZ; j++)
            SpawnCell(i, j);

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
        var cellPos = new Vector3(x * offset, -4f, z * offset);

        //Place the gameobject in the scene
        var newCell = Instantiate(gridCell, cellPos, Quaternion.identity);

        //Set the cell X and Z position of the grid
        newCell.GetComponent<Cell>().x = x;
        newCell.GetComponent<Cell>().z = z;

        //Set the cell in the Stage gameobject and rename the cell to its position in the grid
        newCell.transform.parent = stageParent.transform;
        newCell.name = "Cell: X = " + x + ", Z = " + z;

        if (!debugMode) newCell.GetComponent<Renderer>().enabled = false;

        //Add the cell to the cells list
        cells.Add(newCell.GetComponent<Cell>());
    }

    //Generate the finite hallway in the game
    private IEnumerator HallwayGenerator()
    {
        var _spawnRoom = SpawnRoom().GetComponent<Room>();

        var totalHallwayZ = gridZ - (int)(spawnRoom.GetComponent<Room>().sizeZ / offset +
                                          bossRoom.GetComponent<Room>().sizeZ / offset);

        Room latestHallway = null;
        var generateHallway = true;

        while (generateHallway)
        {
            GameObject chosenHallway = null;
            var weightTotal = hallways.Sum(h => h.GetComponent<Room>().GetWeight());

            chosenHallway = RoomUtil.GetRandomRoom(hallways, weightTotal);

            var hallwayX = (int)chosenHallway.GetComponent<Room>().sizeX;
            var hallwayZ = (int)chosenHallway.GetComponent<Room>().sizeZ;

            totalHallwayZ -= hallwayZ / offset;

            if (totalHallwayZ <= 0)
            {
                generateHallway = false;
                break;
            }

            var posX = _spawnRoom.transform.position.x;
            float posZ = 0;

            if (latestHallway == null)
                posZ = hallwayZ / 2 + _spawnRoom.GetComponent<Room>().sizeZ - offset / 2;
            else
            {
                posZ = latestHallway.transform.position.z + Mathf.RoundToInt(latestHallway.sizeZ / 2) +
                       Mathf.RoundToInt(hallwayZ / 2);
            }

            latestHallway = Instantiate(chosenHallway, new Vector3(posX, 0, posZ), Quaternion.identity)
                .GetComponent<Room>();

            var roomCells = SetRoomCells(chosenHallway, posX, posZ);
            latestHallway.GetComponent<Room>().cells = roomCells;

            mapHallways.Add(latestHallway.GetComponent<Hallway>());
        }

        SpawnBossRoom(latestHallway.gameObject);

        yield return StartCoroutine(GenerateRooms());
    }

    private IEnumerator GenerateRooms()
    {
        roomGenerator.BranchRoomGeneration(mapHallways, minWeightRoomsBranch, maxWeightRoomsBranch);

        yield return StartCoroutine(ReplaceDoors());
    }

    private IEnumerator ReplaceDoors()
    {
        foreach (var room in mapRooms)
            if (room.GetComponent<Room>().GetDoorReplacement() != null)
                StageHelper.ReplaceAllDoors(room);
        yield return StartCoroutine(StageNavMeshBaker());
    }

    private IEnumerator StageNavMeshBaker()
    {
        StageHelper.NavMeshBaker();
        yield return StartCoroutine(StageEnemyGeneration());
    }

    private IEnumerator StageEnemyGeneration()
    {
        foreach (var room in mapRooms)
            if (room.GetComponent<EnemyGeneration>() != null)
                room.GetComponent<EnemyGeneration>().SpawnEnemy();
        yield return null;
    }

    private GameObject SpawnRoom()
    {
        var spawnRoomX = (int)spawnRoom.GetComponent<Room>().sizeX;
        var spawnRoomZ = (int)spawnRoom.GetComponent<Room>().sizeZ;

        float posX = gridX * offset / 2;
        float posZ = spawnRoomZ / 2 - offset / 2;

        var _spawnRoom = Instantiate(spawnRoom, new Vector3(posX, 0, posZ), Quaternion.identity);
        var roomCells = SetRoomCells(_spawnRoom, posX, posZ);

        _spawnRoom.GetComponent<Room>().cells = roomCells;
        _spawnRoom.name = "Spawn Room";

        _spawnRoom.GetComponent<Room>().GetDoors()[0].SetActive(false);

        var lootGeneration = _spawnRoom.GetComponent<LootGeneration>();
        StartCoroutine(lootGeneration.SpawnLoot(_spawnRoom));

        return _spawnRoom;
    }

    private GameObject SpawnBossRoom(GameObject lastHallway)
    {
        var spawnBossRoomX = (int)bossRoom.GetComponent<Room>().sizeX;
        var spawnBossRoomZ = (int)bossRoom.GetComponent<Room>().sizeZ;

        float posX = gridX * offset / 2;
        var posZ = lastHallway.transform.position.z + Mathf.RoundToInt(lastHallway.GetComponent<Room>().sizeZ / 2)
                                                    + spawnBossRoomZ / 2;

        var _bossRoom = Instantiate(bossRoom, new Vector3(posX, 0, posZ), Quaternion.identity);
        var roomCells = SetRoomCells(_bossRoom, posX, posZ);

        _bossRoom.GetComponent<Room>().cells = roomCells;
        _bossRoom.name = "Boss Room";

        _bossRoom.GetComponent<Room>().GetDoors()[0].SetActive(false);

        return _bossRoom;
    }

    //Set the room cells that are occupied by it
    private List<Cell> SetRoomCells(GameObject room, float posX, float posZ)
    {
        //Create a new list to store the cells in
        var roomCells = new List<Cell>();

        var _room = room.GetComponent<Room>();

        //Get the room X and Z length
        var roomX = (int)_room.sizeX / offset;
        var roomZ = (int)_room.sizeZ / offset;

        var cellPosX = (int)(posX % offset == offset / 2 ? posX - offset / 2 : posX) / offset;
        var cellPosZ = (int)(posZ % offset == offset / 2 ? posZ - offset / 2 : posZ) / offset;

        // cellPosX = roomX % 2 != 0 ? cellPosX : cellPosX + 1;
        // cellPosZ = roomZ % 2 != 0 ? cellPosZ : cellPosZ + 1;

        //Get the position X and Z of the cell in the scene
        var startPosX = cellPosX - roomX / 2;
        var startPosZ = cellPosZ - roomZ / 2;

        //Get the starting width and height of the cells you need for the room
        var endPosX = cellPosX + roomX / 2;
        var endPosZ = cellPosZ + roomZ / 2;

        //Loop through cells based on start position and length of room
        for (var i = startPosX; i <= endPosX; i++)
        for (var j = startPosZ; j <= endPosZ; j++)
        {
            var cell = cells.Where(c => c.x == i && c.z == j && c.isOccupied == false).SingleOrDefault();

            if (cell == null)
                Debug.Log("No cell x" + i + " z" + j);
            else
            {
                if (_room.GetComponent<StandardRoom>())
                    cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                else if (_room.GetComponent<SpawnRoom>())
                    cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                else
                    cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                cell.isOccupied = true;

                roomCells.Add(cell);
            }
        }

        return roomCells;
    }

    private void InitializeRoomSizes()
    {
        var allRooms = new List<GameObject>();

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
