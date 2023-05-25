using UnityEngine;
using System.Collections.Generic;
using RoomEditor;

namespace RoomEditor {
	public class RoomScript : MonoBehaviour
	{
		// The save location for the room data file
		public string SaveLocation = "ExampleRoomData";

		// Models and Materials
		public AssetsScript Assets;
		private const int NORTH = 0;
		private const int SOUTH = 1;
		private const int EAST = 2;
		private const int WEST = 3;

		// Data for generated objects
		private Transform _parent;
		private Transform _parentBlocks;
		private Transform _parentModels;

		private Transform _parentFloor;
		private Transform _parentFloorFrames;
		private Transform _parentFloorTiles;

		private Transform _parentWalls;
		private Transform _parentWallFrames;
		private Transform _parentWallTiles;

		private Transform _parentDoors;

		// Door Block Objects
		private List<GameObject> _doorBlocks;

	    public void CreateParents()
	    {
	        // Creates the parent object for all objects
	        _parent = new GameObject("Room").transform;
	        _parent.SetParent(transform);

	        // Creates the parent object for all blocks
	        _parentBlocks = new GameObject("Blocks").transform;
	        _parentBlocks.SetParent(_parent);

	        // Creates the parent object for all 3D models
	        _parentModels = new GameObject("Room Models").transform;
	        _parentModels.SetParent(_parent);

	        // Creates parent object for all floors
	        _parentFloor = new GameObject("Floors").transform;
	        _parentFloor.SetParent(_parentModels);

	        // Creates parent object for floor frames
	        _parentFloorFrames = new GameObject("Frames").transform;
	        _parentFloorFrames.SetParent(_parentFloor);

	        // Creates parent object for floor tiles
	        _parentFloorTiles = new GameObject("Tiles").transform;
	        _parentFloorTiles.SetParent(_parentFloor);

	        // Creates parent object for all walls
	        _parentWalls = new GameObject("Walls").transform;
	        _parentWalls.SetParent(_parentModels);

	        // Creates parent object for wall frames
	        _parentWallFrames = new GameObject("Frames").transform;
	        _parentWallFrames.SetParent(_parentWalls);

	        // Creates parent object for wall tiles
	        _parentWallTiles = new GameObject("Tiles").transform;
	        _parentWallTiles.SetParent(_parentWalls);

	        // Creates parent object for doors
	        _parentDoors = new GameObject("Doors").transform;
	        _parentDoors.SetParent(_parentWalls);
	    }

		public bool ClearAllObjects()
		{
			_doorBlocks = new List<GameObject>();

			Transform t = transform.Find("Room");
			if (t is null) return false;

			_parent = t.gameObject.transform;
			DestroyImmediate(_parent.gameObject);
			_parent = null;
			return true;
		}

		public void CreateBlocks(float aWidth, float aHeight, float aWallHeight, float aCellSize, int aRoomWeight,
			GameObject aDoorReplacementModel, float aThickness, int[] aDoorPositions)
		{
			// Creates floor block
			var lPos = transform.position + new Vector3(0.0f, 0.0f - aThickness / 2.0f, 0.0f);
			var lScale = new Vector3(aWidth + 2*aCellSize, aThickness, aHeight + 2*aCellSize);
			CreateBlock(Assets.FloorBlock, lPos, lScale);

			// Creates wall blocks
			for (int i = 0; i < 4; i++) {
				int lDoorPos = aDoorPositions[i];
				if (lDoorPos >= 0) lDoorPos += (int)aCellSize;
				CreateWallBlocks(i, aWidth + 2*aCellSize, aHeight + 2*aCellSize, aWallHeight, aCellSize, aThickness, lDoorPos);
			}

			// Adds room script to room object
			StageGeneration.Rooms.RoomTypes.StandardRoom lRoom = _parent.gameObject.AddComponent<StageGeneration.Rooms.RoomTypes.StandardRoom>();
			lRoom.sizeX = aWidth + 2*aCellSize;
			lRoom.sizeY = aWallHeight;
			lRoom.sizeZ = aHeight + 2*aCellSize;
			lRoom.doors = _doorBlocks;
			lRoom.weight = aRoomWeight;
			lRoom.doorReplacement = aDoorReplacementModel;
		}

		private void CreateWallSingleBlock(int aSide, float aMin, float aMax, float aHeight,
			float aThickness, Vector3 aWallSideOrigin, float aVerticalOffset)
		{
			// If wall block has no width, don't create the block
			float lWidth = aMax - aMin;
			if (lWidth <= 0.0f) return;

			// Calculates the scale and position of the wall block
			Vector3 lScale = new Vector3();
			Vector3 lRelPos = new Vector3();
			switch (aSide) {
				case NORTH:
				case SOUTH:
					lScale = new Vector3(lWidth, aHeight, aThickness);
					lRelPos = new Vector3(aMin + lWidth / 2.0f, aVerticalOffset + aHeight / 2.0f, 0.0f);
					break;
				case WEST:
				case EAST:
					lScale = new Vector3(aThickness, aHeight, lWidth);
					lRelPos = new Vector3(0.0f, aVerticalOffset + aHeight / 2.0f, aMin + lWidth / 2.0f);
					break;
			}
			Vector3 lPos = aWallSideOrigin + lRelPos;

			// Creates the wall block
			CreateBlock(Assets.WallBlock, lPos, lScale);
		}

		private void CreateDoorBlock(int aSide, float aCellSize, float aDoorPosition,
			float aThickness, Vector3 aWallSideOrigin, float aRoomHeight)
		{
			// Calculates the scale and position for this door block
			Vector3 lScale = new Vector3();
			Vector3 lRelPos = new Vector3();
			switch (aSide) {
				case NORTH:
				case SOUTH:
					lScale = new Vector3(aCellSize, aCellSize, aThickness);
					lRelPos = new Vector3(aDoorPosition + aCellSize / 2.0f, aCellSize / 2.0f, 0.0f);
					break;
				case WEST:
				case EAST:
					lScale = new Vector3(aThickness, aCellSize, aCellSize);
					lRelPos = new Vector3(0.0f, aCellSize / 2.0f, aDoorPosition + aCellSize / 2.0f);
					break;
			}
			Vector3 lPos = aWallSideOrigin + lRelPos;

			// Creates the door block GameObject
			GameObject lObject = CreateBlock(Assets.DoorBlock, lPos, lScale);

			// Adds the door block GameObject to the list for the room script later
			_doorBlocks.Add(lObject);

			// Creates and sets the door script on this door block
			StageGeneration.Rooms.Door lDoor = lObject.GetComponent<StageGeneration.Rooms.Door>();
			if (lDoor != null)
			{
				// Sets the values for the door script
				lDoor.roomPosXOffset = (int)(lPos.x / aCellSize);
				lDoor.roomPosZOffset = (int)((lPos.z + aRoomHeight / 2.0f) / aCellSize);
				switch (aSide) {
					case NORTH:
						lDoor.direction = StageGeneration.Stage.StageHelper.RoomDirections.TOP;
						break;
					case SOUTH:
						lDoor.direction = StageGeneration.Stage.StageHelper.RoomDirections.BOTTOM;
						break;
					case WEST:
						lDoor.direction = StageGeneration.Stage.StageHelper.RoomDirections.LEFT;
						break;
					case EAST:
						lDoor.direction = StageGeneration.Stage.StageHelper.RoomDirections.RIGHT;
						break;
				}
			}
			else
			{
				Debug.Log("Door script component not found in door block");
			}
		}

		private void CreateWallBlocks(int aSide, float aWidth, float aHeight, float aWallHeight, float aCellSize,
			float aThickness, float aDoorPosition)
		{
			float lWallLength = 0.0f;

			// Sets lWallLength (the length of the entire wall side)
			// Sets lWallSideOrigin (the origin of the entire wall side)
			Vector3 lWallSideOrigin = new Vector3(0.0f, 0.0f, 0.0f);
			switch (aSide) {
				case NORTH:
					lWallLength = aWidth;

					lWallSideOrigin.x = -aWidth / 2.0f;
					lWallSideOrigin.z = aHeight / 2.0f - aThickness / 2.0f;
					break;
				case SOUTH:
					lWallLength = aWidth;

					lWallSideOrigin.x = -aWidth / 2.0f;
					lWallSideOrigin.z = -(aHeight / 2.0f - aThickness / 2.0f);
					break;
				case WEST:
					lWallLength = aHeight;

					lWallSideOrigin.x = -(aWidth / 2.0f - aThickness / 2.0f);
					lWallSideOrigin.z = -aHeight / 2.0f;
					break;
				case EAST:
					lWallLength = aHeight;

					lWallSideOrigin.x = aWidth / 2.0f - aThickness / 2.0f;
					lWallSideOrigin.z = -aHeight / 2.0f;
					break;
			}

			if (aDoorPosition < 0.0f)
			{
				// Creates the entire wall in one block (if there is no door)
				CreateWallSingleBlock(aSide, 0.0f, lWallLength, aWallHeight, aThickness, lWallSideOrigin, 0.0f);
			}
			else
			{
				// Creates bottom layer of wall (with gap for door)
				CreateWallSingleBlock(aSide, 0.0f, aDoorPosition, aCellSize, aThickness, lWallSideOrigin, 0.0f);
				CreateWallSingleBlock(aSide, aDoorPosition + aCellSize, lWallLength, aCellSize, aThickness, lWallSideOrigin, 0.0f);

				// Creates door block
				CreateDoorBlock(aSide, aCellSize, aDoorPosition, aThickness, lWallSideOrigin, aHeight);

				// Creates top layer of wall (if there was a door and the wall is taller than the door)
				if (aWallHeight > aCellSize)
				{
					CreateWallSingleBlock(aSide, 0.0f, lWallLength, aWallHeight - aCellSize, aThickness, lWallSideOrigin, aCellSize);
				}
			}
		}

	    private GameObject CreateBlock(GameObject aModel, Vector3 aPosition, Vector3 aScale)
	    {
	        GameObject lObject = Instantiate(aModel, aPosition, Quaternion.identity, _parentBlocks);
	        lObject.transform.localScale = aScale;
					return lObject;
	    }

	    public void CreateFloorFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial)
	    {
	        var lModel = Assets.FloorFrameModel;
	        CreateObject(_parentFloorFrames, lModel, aPosition, aRotation, aMaterial);
	    }

	    public void CreateFloorTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial)
	    {
	        var lModel = Assets.FloorTileModels[aModel];
	        CreateObject(_parentFloorTiles, lModel, aPosition, aRotation, aMaterial);
	    }

	    public void CreateWallFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial)
	    {
	        var lModel = Assets.WallFrameModel;
	        CreateObject(_parentWallFrames, lModel, aPosition, aRotation, aMaterial);
	    }

	    public void CreateWallTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial)
	    {
	        var lModel = Assets.WallTileModels[aModel];
	        CreateObject(_parentWallTiles, lModel, aPosition, aRotation, aMaterial);
	    }

	    public void CreateDoor(Vector3 aPosition, Quaternion aRotation, int aMaterial)
	    {
	        var lModel = Assets.DoorModel;
	        CreateObject(_parentDoors, lModel, aPosition, aRotation, aMaterial);
	    }

	    private void CreateObject(Transform aParent, GameObject aModel, Vector3 aPosition, Quaternion aRotation,
	        int aMaterial)
	    {
	        var lObject = Instantiate(aModel, aPosition, aRotation, aParent);
					if (aMaterial >= 0)
					{
	            var lMaterial = Assets.Materials[aMaterial];
	            lObject.GetComponent<Renderer>().material = lMaterial;
					}
	    }
	}
}
