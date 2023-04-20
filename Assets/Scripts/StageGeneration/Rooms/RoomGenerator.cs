using System;
using System.Collections.Generic;
using System.Linq;
using StageGeneration.Rooms.RoomTypes;
using StageGeneration.Rooms.Util;
using StageGeneration.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StageGeneration.Rooms
{
    public class RoomGenerator
    {
        private GameObject _currentRoom;
        private GameObject _previousRoom;

        public void BranchRoomGeneration(List<Hallway> mapHallways, int minWeightRoomsBranch, int maxWeightRoomsBranch)
        {
            var weightTotal = StageHelper.GetRooms().Sum(h => h.GetComponent<Room>().GetWeight());

            foreach (var hallway in mapHallways)
            {
                hallway.GetComponent<Room>().SetDoorCells();

                foreach (var door in hallway.GetDoors())
                {
                    var branchLength = Random.Range(minWeightRoomsBranch, maxWeightRoomsBranch + 1);

                    PlaceRooms(door, branchLength, weightTotal);
                }

                StageHelper.ReplaceAllDoors(hallway.gameObject);

                _currentRoom = null;
                _previousRoom = null;
            }
        }

        private void PlaceRooms(GameObject spawnDoor, int branchLength, int weightTotal)
        {
            var initialSpawned = false;

            for (var i = 0; i < branchLength; i++)
            {
                _currentRoom = RoomUtil.GetRandomRoom(StageHelper.GetRooms(), weightTotal);

                Cell doorCell = null;
                var currentSpawnDoor = spawnDoor;
                StageHelper.RoomDirections placementSide;

                if (initialSpawned)
                {
                    placementSide = DeterminePlacementSide(_previousRoom);

                    if (placementSide == StageHelper.RoomDirections.UNDEFINED) break;

                    currentSpawnDoor = _previousRoom.GetComponent<Room>()
                        .GetDoors().SingleOrDefault(d => d.GetComponent<Door>().GetDirection() == placementSide);

                    if (currentSpawnDoor is not null) doorCell = currentSpawnDoor.GetComponent<Door>().cell;
                }
                else
                {
                    placementSide = currentSpawnDoor.GetComponent<Door>().GetDirection();
                    doorCell = currentSpawnDoor.GetComponent<Door>().cell;

                    var initialPos = _currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);
                    var canPlace = _currentRoom.GetComponent<Room>()
                        .CanPlace((int)initialPos["x"], (int)initialPos["z"]);

                    if (!canPlace) break;
                }

                var pos = _currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);

                _currentRoom.GetComponent<Room>().SetRoomCells((int)pos["x"], (int)pos["z"]);
                _currentRoom.GetComponent<Room>().SetDoorCells();

                _previousRoom = _currentRoom.GetComponent<Room>()
                    .PlaceRoom((int)pos["x"], (int)pos["z"], placementSide, currentSpawnDoor);

                initialSpawned = true;
            }
        }

        private StageHelper.RoomDirections DeterminePlacementSide(GameObject previousRoom)
        {
            var openDirections = Enum.GetValues(typeof(StageHelper.RoomDirections)).Cast<StageHelper.RoomDirections>()
                .Where(direction => direction != StageHelper.RoomDirections.UNDEFINED).ToList();

            var doorDirection = StageHelper.RandomDirection();

            openDirections.Remove(doorDirection);

            var canPlace = false;
            GameObject door = null;

            if (doorDirection != StageHelper.RoomDirections.UNDEFINED)
            {
                door = previousRoom.GetComponent<Room>()
                    .GetDoors().SingleOrDefault(d => d.GetComponent<Door>().GetDirection() == doorDirection);
                var pos = _currentRoom.GetComponent<Room>().PlacementPos(doorDirection, door.GetComponent<Door>().cell);
                canPlace = _currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);
            }

            if (!canPlace)
            {
                var iteration = 0;
                while (!canPlace && iteration < 3)
                {
                    doorDirection = StageHelper.RandomDirection(openDirections);

                    openDirections.Remove(doorDirection);

                    door = previousRoom.GetComponent<Room>().GetDoors().SingleOrDefault(d =>
                        d.GetComponent<Door>().hasNeighbour == false &&
                        d.GetComponent<Door>().GetDirection() == doorDirection);

                    var pos = _currentRoom.GetComponent<Room>()
                        .PlacementPos(doorDirection, door.GetComponent<Door>().cell);
                    canPlace = _currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);

                    iteration++;
                }
            }

            if (!canPlace)
                doorDirection = StageHelper.RoomDirections.UNDEFINED;
            else
                door.GetComponent<Door>().hasNeighbour = true;

            return doorDirection;
        }
    }
}
