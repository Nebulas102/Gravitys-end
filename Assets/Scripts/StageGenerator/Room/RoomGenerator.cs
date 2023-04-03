using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                currentSpawnDoor = previousRoom.GetComponent<Room>().GetDoors()
                    .Where(d => d.GetComponent<Door>().GetDirection() == placementSide).SingleOrDefault();

                doorCell = currentSpawnDoor.GetComponent<Door>().cell;    

                if (doorCell == null)
                {
                    break;
                }    
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
        StageHelper.roomDirections doorDirection = StageHelper.RandomDirection();
        
        var door = previousRoom.GetComponent<Room>().GetDoors().Where(d => d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();
        var pos = currentRoom.GetComponent<Room>().PlacementPos(doorDirection, door.GetComponent<Door>().cell);
        bool canPlace = currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);

        while(door == null || door.GetComponent<Door>().hasNeighbour || door.GetComponent<Door>().GetDirection() != doorDirection || !canPlace)
        {
            doorDirection = StageHelper.RandomDirection();
            door = previousRoom.GetComponent<Room>().GetDoors().Where(d => d.GetComponent<Door>().hasNeighbour == false && 
                    d.GetComponent<Door>().GetDirection() == doorDirection).SingleOrDefault();       
                
            pos = currentRoom.GetComponent<Room>().PlacementPos(doorDirection, door.GetComponent<Door>().cell);        
            canPlace = currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);        
        }

        door.GetComponent<Door>().hasNeighbour = true;

        return doorDirection;
    }
}
