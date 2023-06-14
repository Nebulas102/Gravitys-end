using System;
using System.Collections.Generic;
using System.Linq;
using Core.StageGeneration.Rooms.RoomTypes;
using Core.StageGeneration.Stage;
using UnityEngine;

namespace Core.StageGeneration.Rooms
{
    public abstract class Room : MonoBehaviour
    {
        public float sizeX;
        public float sizeY;
        public float sizeZ;

        public List<GameObject> doors;

        public int weight;

        public GameObject doorReplacement;

        [HideInInspector]
        public List<Cell> cells;

        public int GetWeight()
        {
            return weight;
        }

        public List<GameObject> GetDoors()
        {
            return doors;
        }

        public GameObject GetDoorReplacement()
        {
            return doorReplacement;
        }

        public GameObject PlaceRoom()
        {
            return Instantiate(gameObject, new Vector3(0, 20, 0), Quaternion.identity);
        }

        public GameObject SetRoomData(int posX, int posZ, Quaternion rotation,StageHelper.RoomDirections direction, GameObject spawnDoor)
        {
            gameObject.transform.position = new Vector3(posX, 0, posZ);
            gameObject.transform.rotation = rotation;

            spawnDoor.SetActive(false);

            gameObject.GetComponent<Room>().doors
                .SingleOrDefault(d =>
                    d.GetComponent<Door>().GetDirection() == StageHelper.GetOppositeDirection(direction))
                ?.SetActive(false);

            GameObject.FindWithTag("StageGenerator").GetComponent<StageGenerator>().AddRoomToMap(gameObject);

            return gameObject;
        }

        public Quaternion RotateRoom(StageHelper.RoomDirections placementDirection)
        {
            Door door = doors[0].GetComponent<Door>();

            var rotation = NewRotationData(door, placementDirection);

            return rotation;
        }

 public Quaternion NewRotationData(Door door, StageHelper.RoomDirections placementDirection)
        {

            var rotation = new Quaternion(0, 0, 0, 0);

            var storedSizeX = sizeX;
            var storedSizeZ = sizeZ;

            if (door.direction == placementDirection)
            {
                door.direction = StageHelper.GetOppositeDirection(placementDirection);
                rotation.y = 180;
            }
            else
            {
                switch (placementDirection)
                {
                    case StageHelper.RoomDirections.TOP:
                        if (door.direction == StageHelper.RoomDirections.RIGHT)
                        {
                            rotation.y = 90;

                            door.roomPosXOffset = (int)(sizeZ / 5) - 1;
                            door.roomPosZOffset = (int)(sizeZ / 2) / 5;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        else if (door.direction == StageHelper.RoomDirections.LEFT)
                        {
                            rotation.y = 270;

                            door.roomPosXOffset = 0;
                            door.roomPosZOffset = (int)(sizeZ / 2) / 5;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        break;
                    case StageHelper.RoomDirections.RIGHT:
                        if (door.direction == StageHelper.RoomDirections.TOP)
                        {
                            rotation.y = 270;

                            door.roomPosXOffset = (int)(sizeX / 2) / 5;
                            door.roomPosZOffset = (int)(sizeZ / 2);

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        else if (door.direction == StageHelper.RoomDirections.BOTTOM)
                        {
                            rotation.y = 90;

                            door.roomPosXOffset = (int)(sizeZ / 2) / 5;
                            door.roomPosZOffset = 0;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        break;
                    case StageHelper.RoomDirections.BOTTOM:
                        if (door.direction == StageHelper.RoomDirections.RIGHT)
                        {
                            rotation.y = 270;

                            door.roomPosXOffset = (int)(sizeZ / 5) - 1;
                            door.roomPosZOffset = (int)(sizeZ / 2) / 5;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        else if (door.direction == StageHelper.RoomDirections.LEFT)
                        {
                            rotation.y = 90;
                            
                            door.roomPosXOffset = 0;
                            door.roomPosZOffset = (int)(sizeZ / 2) / 5;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        break;
                    case StageHelper.RoomDirections.LEFT:
                        if (door.direction == StageHelper.RoomDirections.TOP)
                        {
                            rotation.y = 90;

                            door.roomPosXOffset = (int)(sizeX / 2) / 5;
                            door.roomPosZOffset = (int)(sizeZ / 5) - 1;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        else if (door.direction == StageHelper.RoomDirections.BOTTOM)
                        {
                            rotation.y = 270;

                            door.roomPosXOffset = (int)(sizeZ / 2) / 5;
                            door.roomPosZOffset = 0;

                            sizeX = storedSizeZ;
                            sizeZ = storedSizeX;
                        }
                        break;
                }
            }

            return rotation;
        }

        public Dictionary<string, float> PlacementPos(StageHelper.RoomDirections roomDirection, Cell doorCell)
        {
            var pos = new Dictionary<string, float>();

            float roomX = 0;
            float roomZ = 0;

            var resizeZ = Mathf.RoundToInt(sizeZ / 2);
            var resizeX = Mathf.RoundToInt(sizeX / 2);

            var position = doorCell.transform.position;

            var doorCellX = position.x;
            var doorCellZ = position.z;
            var offset = StageHelper.GetOffset();
            var divOffset = offset / 2;

            switch (roomDirection)
            {
                case StageHelper.RoomDirections.TOP:
                    roomX = doorCellX;
                    roomZ = doorCellZ +
                            (resizeZ - divOffset) + offset;
                    break;
                case StageHelper.RoomDirections.RIGHT:
                    roomX = doorCellX +
                            (resizeX - divOffset) + offset;
                    roomZ = doorCellZ;
                    break;
                case StageHelper.RoomDirections.BOTTOM:
                    roomX = doorCellX;
                    roomZ = doorCellZ -
                            (resizeZ - divOffset + offset);
                    break;
                case StageHelper.RoomDirections.LEFT:
                    roomX = doorCellX -
                            (resizeX - divOffset + offset);
                    roomZ = doorCellZ;
                    break;
                case StageHelper.RoomDirections.UNDEFINED:
                    Debug.Log("Is Undefined");
                    roomX = 0;
                    roomZ = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roomDirection), roomDirection, null);
            }

            pos.Add("x", roomX);
            pos.Add("z", roomZ);

            return pos;
        }

        public void SetDoorCells()
        {
            var room = gameObject.GetComponent<Room>();

            if (room.doors is not null)
            {
                foreach (var door in room.doors)
                {
                    var doorCellX = room.cells.Select(mh => mh.x).Distinct()
                        .ToArray()[door.GetComponent<Door>().roomPosXOffset];
                    var doorCellZ = room.cells.Select(mh => mh.z).Distinct()
                        .ToArray()[door.GetComponent<Door>().roomPosZOffset];

                    var doorCellRoom = StageHelper.GetCells()
                        .SingleOrDefault(c => c.x == doorCellX && c.z == doorCellZ);

                    if (doorCellRoom is null) continue;
                    doorCellRoom.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

                    door.GetComponent<Door>().cell = doorCellRoom;
                }
            }

        }

        public bool CanPlace(int posX, int posZ)
        {
            var canPlace = true;
            var offset = StageHelper.GetOffset();

            //Get the room X and Z length
            var roomX = (int)sizeX / offset;
            var roomZ = (int)sizeZ / offset;

            var cellPosX = (posX % offset == offset / 2 ? posX - offset / 2 : posX) / offset;
            var cellPosZ = (posZ % offset == offset / 2 ? posZ - offset / 2 : posZ) / offset;

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
                    var cell = StageHelper.GetCells()
                        .SingleOrDefault(c => c.x == i && c.z == j && c.isOccupied == false);

                    if (cell is null) canPlace = false;
                }

            return canPlace;
        }

        //Set the room cells that are occupied by it
        public void SetRoomCells(int posX, int posZ)
        {
            var offset = StageHelper.GetOffset();

            //Create a new list to store the cells in
            var roomCells = new List<Cell>();

            var room = gameObject.GetComponent<Room>();

            //Get the room X and Z length
            var roomX = (int)sizeX / offset;
            var roomZ = (int)sizeZ / offset;

            var cellPosX = (posX % offset == offset / 2 ? posX - offset / 2 : posX) / offset;
            var cellPosZ = (posZ % offset == offset / 2 ? posZ - offset / 2 : posZ) / offset;

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
                    var cell = StageHelper.GetCells()
                        .SingleOrDefault(c => c.x == i && c.z == j && c.isOccupied == false);

                    if (cell is null)
                        Debug.Log("No cell x" + i + " z" + j);
                    else
                    {
                        cell.gameObject.GetComponent<MeshRenderer>().material.color =
                            room.GetComponent<StandardRoom>() ? Color.cyan : Color.red;
                        cell.isOccupied = true;

                        roomCells.Add(cell);
                    }
                }

            cells = roomCells;
        }
    }
}
