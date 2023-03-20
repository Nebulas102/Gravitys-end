using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public float sizeX;
    public float sizeY;
    public float sizeZ;

    [HideInInspector]
    public List<Cell> cells;

    [SerializeField]
    private int spawnChance;

    [SerializeField]
    private List<GameObject> doors;

    public List<GameObject> GetDoors()
    {
        return doors;
    }

    public int GetSpawnChance()
    {
        return spawnChance;
    }

    public void SetDoorCells()
    {
        Room room = gameObject.GetComponent<Room>();
        Cell doorCellRoom = null;

        foreach (GameObject _door in room.doors)
        {
            int doorCellX = room.cells.Select(mh => mh.x).Distinct()
                .ToArray()[_door.GetComponent<Door>().roomPosXOffset];
            int doorCellZ = room.cells.Select(mh => mh.z).Distinct()
                .ToArray()[_door.GetComponent<Door>().roomPosZOffset];

            doorCellRoom = StageHelper.cells.Where(c => c.x == doorCellX && c.z == doorCellZ).SingleOrDefault();
            
            doorCellRoom.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

            _door.GetComponent<Door>().cell = doorCellRoom;  
        }
    }

    public void PlaceRooms(GameObject spawnDoor)
    {
        bool initialSpawned = false;
        GameObject previousRoom = null;

        while (Random.Range(0, 2) == 1 || !initialSpawned)
        {
            Cell doorCell = null;
            StageHelper.roomDirections placementSide;

            if (initialSpawned)
            {
                placementSide = DeterminePlacementSide(previousRoom);
                doorCell = previousRoom.GetComponent<Room>().doors.Where(d => d.GetComponent<Door>().GetDirection() == placementSide).SingleOrDefault().GetComponent<Door>().cell;
            }
            else
            {
                doorCell = spawnDoor.GetComponent<Door>().cell;
                placementSide = spawnDoor.GetComponent<Door>().GetDirection();
            }

            var pos = RoomPlacementPos(placementSide, doorCell);

            SetRoomCells((int)pos["x"], (int)pos["z"]);
            SetDoorCells();

            previousRoom = roomPlacement((int)pos["x"], (int)pos["z"]);

            initialSpawned = true;
        }
    }

    private StageHelper.roomDirections DeterminePlacementSide(GameObject previousRoom)
    {
        StageHelper.roomDirections doorDirection = StageHelper.RandomDirection();
        var door = previousRoom.GetComponent<Room>().doors.Where(d => d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();
        var pos = RoomPlacementPos(doorDirection, door.GetComponent<Door>().cell);
        bool canPlace = CanPlaceRoom((int)pos["x"], (int)pos["z"]);

        while(door == null || door.GetComponent<Door>().hasNeighbour || door.GetComponent<Door>().GetDirection() != doorDirection || !canPlace)
        {
            doorDirection = StageHelper.RandomDirection();
            door = previousRoom.GetComponent<Room>().doors.Where(d => d.GetComponent<Door>().hasNeighbour == false && 
             d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();       

            // if (door == null)
            // {
            //     Debug.Log("door empty");
            // }
            // else
            // {
            //     Debug.Log("door not empty");
            // }

            Debug.Log("While loop new pos: " + doorDirection);
                
            pos = RoomPlacementPos(doorDirection, door.GetComponent<Door>().cell);        
            canPlace = CanPlaceRoom((int)pos["x"], (int)pos["z"]);        
        }

        Debug.Log(canPlace);
        Debug.Log(doorDirection);

        door.GetComponent<Door>().hasNeighbour = true;

        return doorDirection;
    }

    private bool CanPlaceRoom(int posX, int posZ)
    {
        bool canPlace = true;
        int offset = StageHelper.offset;

        //Get the room X and Z length
        int roomX = (int)sizeX / offset;
        int roomZ = (int)sizeZ / offset;

        int cellPosX = (posX % offset == 5 ? posX - 5 : posX) / offset;
        int cellPosZ = (posZ % offset == 5 ? posZ - 5 : posZ) / offset;

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
                Cell cell = StageHelper.cells.Where(c => c.x == i && c.z == j && c.isOccupied == false).SingleOrDefault();

                if (cell == null)
                {
                    canPlace = false;
                }
            }
        }

        return canPlace;
    }

    private Dictionary<string, float> RoomPlacementPos(StageHelper.roomDirections roomDirection, Cell doorCell)
    {
        Dictionary<string, float> pos = new Dictionary<string, float>();

        float roomX = 0;
        float roomZ = 0;

        switch(roomDirection)
        {
            case StageHelper.roomDirections.Top:
                roomX = doorCell.transform.position.x;
                roomZ = doorCell.transform.position.z + (sizeZ / 2 - 5 + StageHelper.offset);
            break;
            case StageHelper.roomDirections.Right:
                roomX = doorCell.transform.position.x + (sizeX / 2 - 5 + StageHelper.offset);
                roomZ = doorCell.transform.position.z;
            break;
            case StageHelper.roomDirections.Bottom:
                roomX = doorCell.transform.position.x;
                roomZ = doorCell.transform.position.z - (sizeZ / 2 - 5 + StageHelper.offset);
            break;
            case StageHelper.roomDirections.Left:
                roomX = doorCell.transform.position.x - (sizeX / 2 - 5 + StageHelper.offset);
                roomZ = doorCell.transform.position.z;
            break;
        }

        pos.Add("x", roomX);
        pos.Add("z", roomZ);

        return pos;
    }

    private GameObject roomPlacement(int posX, int posZ)
    {
        return Instantiate(gameObject, new Vector3(posX, 0, posZ), Quaternion.identity);
    }

    //Set the room cells that are occupied by it
    public void SetRoomCells(int posX, int posZ)
    {
        int offset = StageHelper.offset;

        //Create a new list to store the cells in
        List<Cell> roomCells = new List<Cell>();

        Room room = gameObject.GetComponent<Room>();
        
        //Get the room X and Z length
        int roomX = (int)sizeX / offset;
        int roomZ = (int)sizeZ / offset;

        int cellPosX = (posX % offset == 5 ? posX - 5 : posX) / offset;
        int cellPosZ = (posZ % offset == 5 ? posZ - 5 : posZ) / offset;

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
                Cell cell = StageHelper.cells.Where(c => c.x == i && c.z == j && c.isOccupied == false).SingleOrDefault();

                if (cell == null)
                {
                    Debug.Log("No cell x" + i + " z" + j);
                }
                else
                {
                    if (room.GetComponent<StandardRoom>())
                    {
                        // Debug.Log("Cell x" + i + " z" + j);
                        cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
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

        cells = roomCells;
    }
}
