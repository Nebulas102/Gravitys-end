using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class RoomGenerator
{
    private GameObject currentRoom;
    private GameObject previousRoom;

    public void BranchRoomGeneration(List<Hallway> mapHallways, int minWeightRoomsBranch, int maxWeightRoomsBranch)
    {
        int weightTotal = StageHelper.GetRooms().Sum(h => h.GetComponent<Room>().GetWeight());

        for (int i = 0; i < mapHallways.Count; i++)
        {
            mapHallways[i].GetComponent<Room>().SetDoorCells();

            foreach (var _door in mapHallways[i].GetDoors())
            {
                int branchLength = Random.Range(minWeightRoomsBranch, maxWeightRoomsBranch + 1);

                PlaceRooms(_door, branchLength, weightTotal);
            }

            StageHelper.ReplaceAllDoors(mapHallways[i].gameObject);

            currentRoom = null;
            previousRoom = null;
        }
    }

    private void PlaceRooms(GameObject spawnDoor, int branchLength, int weightTotal)
    {
        bool initialSpawned = false;

        for (int i = 0; i < branchLength; i++)
        {
            currentRoom = RoomUtil.GetRandomRoom(StageHelper.GetRooms(), weightTotal);

            Cell doorCell = null;
            GameObject currentSpawnDoor = spawnDoor;
            StageHelper.roomDirections placementSide;

            if (initialSpawned)
            {
                placementSide = DeterminePlacementSide(previousRoom);

                if (placementSide == StageHelper.roomDirections.Undefined)
                {
                    break;
                }

                currentSpawnDoor = previousRoom.GetComponent<Room>().GetDoors()
                    .Where(d => d.GetComponent<Door>().GetDirection() == placementSide).SingleOrDefault();

                doorCell = currentSpawnDoor.GetComponent<Door>().cell;      
            }
            else
            {
                placementSide = currentSpawnDoor.GetComponent<Door>().GetDirection();
                doorCell = currentSpawnDoor.GetComponent<Door>().cell;

                var initialPos = currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);
                bool canPlace = currentRoom.GetComponent<Room>().CanPlace((int)initialPos["x"], (int)initialPos["z"]);

                if (!canPlace)
                {
                    break;
                }
            }

            var pos = currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);

            currentRoom.GetComponent<Room>().SetRoomCells((int)pos["x"], (int)pos["z"]);
            currentRoom.GetComponent<Room>().SetDoorCells();

            previousRoom = currentRoom.GetComponent<Room>().PlaceRoom((int)pos["x"], (int)pos["z"], placementSide, currentSpawnDoor);

            initialSpawned = true;
        }
    }

    private StageHelper.roomDirections DeterminePlacementSide(GameObject previousRoom)
    {
        List<StageHelper.roomDirections> openDirections = new List<StageHelper.roomDirections>();

        foreach (StageHelper.roomDirections direction in System.Enum.GetValues(typeof(StageHelper.roomDirections)))
        {
            if (direction != StageHelper.roomDirections.Undefined)
            {
                openDirections.Add(direction);
            }
        }

        StageHelper.roomDirections doorDirection = StageHelper.RandomDirection();

        openDirections.Remove(doorDirection);

        bool canPlace = false;
        GameObject door = null;

        if (doorDirection != StageHelper.roomDirections.Undefined)
        {
            door = previousRoom.GetComponent<Room>().GetDoors().Where(d => d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();
            var pos = currentRoom.GetComponent<Room>().PlacementPos(doorDirection, door.GetComponent<Door>().cell);
            canPlace = currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);
        }

        if (!canPlace)
        {
            int iteration = 0;
            while(!canPlace && iteration < 3)
            {
                doorDirection = StageHelper.RandomDirection(openDirections);

                openDirections.Remove(doorDirection);

                door = previousRoom.GetComponent<Room>().GetDoors().Where(d => d.GetComponent<Door>().hasNeighbour == false && 
                        d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();       
                    
                var pos = currentRoom.GetComponent<Room>().PlacementPos(doorDirection, door.GetComponent<Door>().cell);        
                canPlace = currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);       
                
                iteration++;
            }
        }

        if (!canPlace)
        {
            doorDirection = StageHelper.roomDirections.Undefined;
        }
        else
        {
            door.GetComponent<Door>().hasNeighbour = true;
        }

        return doorDirection;
    }
}
