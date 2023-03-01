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

    [Header("Room Grid Settings")]
    [SerializeField]
    private GameObject hallway;
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

    [SerializeField]
    private List<Hallway> mapHallways;
    [SerializeField]
    private List<StandardRoom> mapRooms;

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

        //Start making the finite hallway
        StartCoroutine(HallwayGenerator());
    }

    //Spawn cell in the grid on the calculated location
    private void SpawnCell(int x, int z)
    {
        //Create the position of the cell to place in scene
        Vector3 cellPos = new Vector3(x * offset, -5f, z * offset);

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

            latestHallway = Instantiate(hallway, new Vector3(posX, 0, posZ), Quaternion.identity);
            List<Cell> roomCells = SetRoomCells(hallwayX, hallwayZ, posX, posZ);

            latestHallway.GetComponent<Room>().cells = roomCells;

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
        List<Cell> roomCells = SetRoomCells(hallwayX, hallwayZ, posX, posZ);

        _hallway.GetComponent<Room>().cells = roomCells;
        _hallway.name = "Initial hallway (temp spawn)";

        mapHallways.Add(_hallway.GetComponent<Hallway>());

        return _hallway;
    }

    private IEnumerator RoomPlacement()
    {
        for (int i = 0; i < mapHallways.Count; i++)
        {
            foreach (var _door in mapHallways[i].doors)
            {
                int placeRoom = Random.Range(0, 2);
                Debug.Log(placeRoom);
                if (placeRoom == 1)
                {
                    int doorCellX = mapHallways[i].cells.Select(mh => mh.x).Distinct()
                                    .OrderByDescending(mh => mh).ToArray()[_door.GetComponent<Door>().roomPosXOffset - 1];
                    int doorCellZ = mapHallways[i].cells.Select(mh => mh.z).Distinct()
                                    .OrderByDescending(mh => mh).ToArray()[_door.GetComponent<Door>().roomPosZOffset - 1];

                    Cell doorCell = null;

                    if (_door.GetComponent<Door>().rightSide)
                    {
                        doorCell = cells.Where(c => c.x == doorCellX + 1 && c.z == doorCellZ).SingleOrDefault();
                    }
                    else
                    {
                        doorCell = cells.Where(c => c.x == doorCellX - 1 && c.z == doorCellZ).SingleOrDefault();
                    }

                    doorCell.gameObject.GetComponent<MeshRenderer>().material = doorCellMaterial;
                    _door.GetComponent<Door>().cell = doorCell;

                    GameObject randomRoom = rooms[Random.Range(0, rooms.Count)];

                    _door.gameObject.SetActive(false);

                    if (_door.GetComponent<Door>().rightSide)
                    {
                        Instantiate(randomRoom, new Vector3(doorCell.transform.position.x + 5, 0, doorCell.transform.position.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(randomRoom, new Vector3(doorCell.transform.position.x - 5, 0, doorCell.transform.position.z), Quaternion.identity);
                    }
                }
            }
        }

        yield return null;
    }

    //Set the room cells that are occupied by it
    private List<Cell> SetRoomCells(int width, int height, int posX, int posZ)
    {
        //Create a new list to store the cells in
        List<Cell> roomCells = new List<Cell>();

        //Get the room X and Z length
        int roomX = width / offset;
        int roomZ = height / offset;

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
                    cell.isOccupied = true;
                    cell.gameObject.name = "Hallway cell";
                    cell.gameObject.GetComponent<MeshRenderer>().material = occupiedCellMaterial;

                    roomCells.Add(cell);
                }
            }
        }

        return roomCells;
    }

    private Cell GetRandomCell()
    {
        int randomx = Random.Range(0, gridX + 1);
        int randomz = Random.Range(0, gridZ + 1);

        Cell cell = cells.Where(c => c.x == randomx && c.z == randomz && c.isOccupied == false).SingleOrDefault();

        while (cell.isOccupied)
        {
            GetRandomCell();
        }

        return cell;
    }
}
