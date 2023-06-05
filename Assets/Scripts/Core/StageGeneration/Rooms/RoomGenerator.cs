using System;
using System.Collections.Generic;
using System.Linq;
using Core.StageGeneration.Rooms.RoomTypes;
using Core.StageGeneration.Rooms.Util;
using Core.StageGeneration.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.StageGeneration.Rooms
{
    public class RoomGenerator
    {
        private GameObject _currentRoom;
        private GameObject _previousRoom;

        private int hallwayDoorCount;
        private int currentHallwayDoorCount = 0;

        private int startDoorsLeftCount;
        private int startDoorsRightCount;

        private bool keyRoomInBranch = false;
        private bool keyRoomOutsideBranch = false;

        private bool initialSpawned = false;

        public void BranchRoomGeneration(List<Hallway> mapHallways, int minWeightRoomsBranch, int maxWeightRoomsBranch)
        {
            var weightTotal = StageHelper.GetRooms().Sum(h => h.GetComponent<Room>().GetWeight());

            hallwayDoorCount = mapHallways.Count * 4;
            startDoorsLeftCount = hallwayDoorCount / 2 - 4;
            startDoorsRightCount = hallwayDoorCount - 4;

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

        private void SpawnKeyRoom(bool _initialSpawned, GameObject _spawnDoor)
        {
            _currentRoom = StageHelper.GetKeyRoom();

            Cell doorCell = null;
            var currentSpawnDoor = _spawnDoor;
            StageHelper.RoomDirections placementSide;

            var canPlace = false;
            var isInitial = false;

            if (_initialSpawned)
            {
                placementSide = DeterminePlacementSide(_previousRoom);

                if (placementSide != StageHelper.RoomDirections.UNDEFINED)
                {
                    currentSpawnDoor = _previousRoom.GetComponent<Room>()
                                .GetDoors().SingleOrDefault(d => d.GetComponent<Door>().GetDirection() == placementSide);
                }

                if (currentSpawnDoor is not null) doorCell = currentSpawnDoor.GetComponent<Door>().cell;
            }
            else
            {
                placementSide = currentSpawnDoor.GetComponent<Door>().GetDirection();
                doorCell = currentSpawnDoor.GetComponent<Door>().cell;

                var initialPos = _currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);

                canPlace = _currentRoom.GetComponent<Room>()
                    .CanPlace((int)initialPos["x"], (int)initialPos["z"]);

                isInitial = true;
            }

            if (canPlace)
            {
                var pos = _currentRoom.GetComponent<Room>().PlacementPos(placementSide, doorCell);

                _currentRoom.GetComponent<Room>().SetRoomCells((int)pos["x"], (int)pos["z"]);
                _currentRoom.GetComponent<Room>().SetDoorCells();

                _previousRoom = _currentRoom.GetComponent<Room>()
                    .PlaceRoom((int)pos["x"], (int)pos["z"], placementSide, currentSpawnDoor);

                keyRoomInBranch = true;

                if (isInitial) initialSpawned = true;
            }
        }

        private void PlaceRooms(GameObject spawnDoor, int branchLength, int weightTotal)
        {
            initialSpawned = false;

            currentHallwayDoorCount++;

            for (var i = 0; i < branchLength; i++)
            {
                // If there is no key room spawned in a branch
                if (!keyRoomInBranch)
                {
                    // Pick only a door at the end of the right or left hallway
                    if (currentHallwayDoorCount >= startDoorsLeftCount && currentHallwayDoorCount <= (startDoorsLeftCount + 4)
                        || currentHallwayDoorCount >= startDoorsRightCount && currentHallwayDoorCount <= (startDoorsRightCount + 4))
                    {
                        SpawnKeyRoom(initialSpawned, spawnDoor);
                        continue;
                    }
                }

                _currentRoom = RoomUtil.GetRandomRoom(StageHelper.GetRooms(), weightTotal);

                Cell doorCell = null;
                var currentSpawnDoor = spawnDoor;
                StageHelper.RoomDirections placementSide;

                if (initialSpawned)
                {
                    placementSide = DeterminePlacementSide(_previousRoom);

                    if (placementSide == StageHelper.RoomDirections.UNDEFINED) continue;

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

                    if (!canPlace) continue;
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
            var openDirections = previousRoom.GetComponent<Room>().GetDoors()
                .Where(d => d.GetComponent<Door>().GetDirection() != StageHelper.RoomDirections.UNDEFINED)
                .Select(d => d.GetComponent<Door>().GetDirection()).ToList();

            var doorDirection = StageHelper.RandomDirectionFromRoom(previousRoom);

            openDirections.Remove(doorDirection);

            var canPlace = false;
            GameObject previousDoor = null;
            GameObject currentDoor = null;

            if (doorDirection != StageHelper.RoomDirections.UNDEFINED)
            {
                previousDoor = previousRoom.GetComponent<Room>().GetDoors().SingleOrDefault(d =>
                    d.GetComponent<Door>().hasNeighbour == false &&
                    d.GetComponent<Door>().GetDirection() == doorDirection);

                if (previousDoor != null)
                {
                    currentDoor = _currentRoom.GetComponent<Room>().GetDoors().SingleOrDefault(
                    d => d.GetComponent<Door>().hasNeighbour == false &&
                    d.GetComponent<Door>().GetDirection() == StageHelper.GetOppositeDirection(doorDirection));

                    if (currentDoor != null)
                    {
                        var pos = _currentRoom.GetComponent<Room>()
                        .PlacementPos(doorDirection, previousDoor.GetComponent<Door>().cell);

                        canPlace = _currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);
                    }
                }
            }

            if (!canPlace)
            {
                var iteration = 0;
                var previousRoomMaxDoors = previousRoom.GetComponent<Room>().GetDoors().Count;

                while (!canPlace && iteration < previousRoomMaxDoors)
                {
                    doorDirection = StageHelper.RandomDirection(openDirections);

                    openDirections.Remove(doorDirection);

                    previousDoor = previousRoom.GetComponent<Room>().GetDoors().SingleOrDefault(d =>
                        d.GetComponent<Door>().hasNeighbour == false &&
                        d.GetComponent<Door>().GetDirection() == doorDirection);

                    if (previousDoor != null)
                    {
                        currentDoor = _currentRoom.GetComponent<Room>().GetDoors().SingleOrDefault(
                        d => d.GetComponent<Door>().hasNeighbour == false &&
                        d.GetComponent<Door>().GetDirection() == StageHelper.GetOppositeDirection(doorDirection));

                        if (currentDoor != null)
                        {
                            var pos = _currentRoom.GetComponent<Room>()
                            .PlacementPos(doorDirection, previousDoor.GetComponent<Door>().cell);

                            canPlace = _currentRoom.GetComponent<Room>().CanPlace((int)pos["x"], (int)pos["z"]);
                        }
                    }

                    iteration++;
                }
            }

            if (!canPlace)
                doorDirection = StageHelper.RoomDirections.UNDEFINED;
            else
                previousDoor.GetComponent<Door>().hasNeighbour = true;

            return doorDirection;
        }
    }
}
