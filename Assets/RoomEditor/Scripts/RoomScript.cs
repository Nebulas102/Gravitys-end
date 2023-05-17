using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {
	// The save location for the room data file
	public string SaveLocation = "ExampleRoomData";

	int NORTH = 0;
	int SOUTH = 1;
	int EAST = 2;
	int WEST = 3;

	// Data for generated objects
	Transform _parent = null;

	Transform _parentBlocks = null;
	Transform _parentModels = null;

	Transform _parentFloor = null;
	Transform _parentFloorFrames = null;
	Transform _parentFloorTiles = null;

	Transform _parentWalls = null;
	Transform _parentWallFrames = null;
	Transform _parentWallTiles = null;

	Transform _parentDoors = null;

	// Models and Materials
	public AssetsScript Assets = null;

	public void CreateParents() {
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

	public bool ClearAllObjects() {
		Transform t = transform.Find("Room");
		if (t != null) {
			_parent = t.gameObject.transform;
			DestroyImmediate(_parent.gameObject);
			_parent = null;
			return true;
		}

		return false;
	}

	public void CreateBlocks(float aWidth, float aHeight, float aWallHeight, float aDoorCellSize, float aThickness, int[] aDoorPositions) {
		Vector3 lPos, lScale;

		// Creates floor block
		lPos = transform.position + new Vector3(0.0f, 0.0f - (aThickness / 2.0f), 0.0f);
		lScale = new Vector3(aWidth, aThickness, aHeight);
		CreateBlock(Assets.FloorBlock, lPos, lScale);

		// North Wall
		CreateHorizontalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, (float)aDoorPositions[NORTH], true);

		// South Wall
		CreateHorizontalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, (float)aDoorPositions[SOUTH], false);

		// East Wall
		CreateVerticalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, (float)aDoorPositions[EAST], true);

		// West Wall
		CreateVerticalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, (float)aDoorPositions[WEST], false);
	}

	private void CreateWallSingleBlockHorizontal(float aMin, float aMax, float aHeight, float aThickness, Vector3 aWallSideOrigin, float aVerticalOffset) {
		float lWidth = aMax - aMin;
		if (lWidth > 0.0f) {
			Vector3 lScale = new Vector3(lWidth, aHeight, aThickness);
			Vector3 lPos = new Vector3(aMin + (lWidth/2.0f), aVerticalOffset + (aHeight / 2.0f), 0.0f);

			CreateBlock(Assets.WallBlock, aWallSideOrigin + lPos, lScale);
		}
	}

	private void CreateWallSingleBlockVertical(float aMin, float aMax, float aHeight, float aThickness, Vector3 aWallSideOrigin, float aVerticalOffset) {
		float lWidth = aMax - aMin;
		if (lWidth > 0.0f) {
			Vector3 lScale = new Vector3(aThickness, aHeight, lWidth);
			Vector3 lPos = new Vector3(0.0f, aVerticalOffset + (aHeight / 2.0f), aMin + (lWidth/2.0f));

			CreateBlock(Assets.WallBlock, aWallSideOrigin + lPos, lScale);
		}
	}

	private void CreateDoorBlockHorizontal(float aCellSize, float aDoorPosition, float aThickness, Vector3 aWallSideOrigin) {
		Vector3 lScale = new Vector3(aCellSize, aCellSize, aThickness);
		Vector3 lPos = new Vector3(aDoorPosition + (aCellSize/2.0f), aCellSize/2.0f, 0.0f);

		CreateBlock(Assets.DoorBlock, aWallSideOrigin + lPos, lScale);
	}

	private void CreateDoorBlockVertical(float aCellSize, float aDoorPosition, float aThickness, Vector3 aWallSideOrigin) {
		Vector3 lScale = new Vector3(aThickness, aCellSize, aCellSize);
		Vector3 lPos = new Vector3(0.0f, aCellSize/2.0f, aDoorPosition + (aCellSize/2.0f));

		CreateBlock(Assets.DoorBlock, aWallSideOrigin + lPos, lScale);
	}

	private void CreateHorizontalWallBlocks(float aWidth, float aHeight, float aWallHeight, float aCellSize, float aThickness, float aDoorPosition, bool aIsNorthSide) {
		Vector3 lWallSideOrigin = new Vector3(-aWidth/2.0f, 0.0f, 0.0f);
		if (aIsNorthSide) {
			lWallSideOrigin.z = aHeight/2.0f + aThickness/2.0f;
		} else {
			lWallSideOrigin.z = -(aHeight/2.0f + aThickness/2.0f);
		}

		if (aDoorPosition < 0.0f) {
			// Creates the entire wall in one block (if there is no door)
			CreateWallSingleBlockHorizontal(0.0f, aWidth, aWallHeight, aThickness, lWallSideOrigin, 0.0f);
		} else {
			// Creates bottom layer of wall (with gap for door)
			CreateWallSingleBlockHorizontal(0.0f, aDoorPosition, aCellSize, aThickness, lWallSideOrigin, 0.0f);
			CreateWallSingleBlockHorizontal(aDoorPosition + aCellSize, aWidth, aCellSize, aThickness, lWallSideOrigin, 0.0f);

			// Creates door block
			CreateDoorBlockHorizontal(aCellSize, aDoorPosition, aThickness, lWallSideOrigin);

			// Creates top layer of wall (if there was a door and the wall is taller than the door)
			if (aWallHeight > aCellSize) {
				CreateWallSingleBlockHorizontal(0.0f, aWidth, aWallHeight - aCellSize, aThickness, lWallSideOrigin, aCellSize);
			}
		}
	}

	private void CreateVerticalWallBlocks(float aWidth, float aHeight, float aWallHeight, float aCellSize, float aThickness, float aDoorPosition, bool aIsEastSide) {
		Vector3 lWallSideOrigin = new Vector3(0.0f, 0.0f, -aHeight/2.0f);
		if (aIsEastSide) {
			lWallSideOrigin.x = aWidth/2.0f + aThickness/2.0f;
		} else {
			lWallSideOrigin.x = -(aWidth/2.0f + aThickness/2.0f);
		}

		if (aDoorPosition < 0.0f) {
			// Creates the entire wall in one block (if there is no door)
			CreateWallSingleBlockVertical(0.0f, aHeight, aWallHeight, aThickness, lWallSideOrigin, 0.0f);
		} else {
			// Creates bottom layer of wall (with gap for door)
			CreateWallSingleBlockVertical(0.0f, aDoorPosition, aCellSize, aThickness, lWallSideOrigin, 0.0f);
			CreateWallSingleBlockVertical(aDoorPosition + aCellSize, aHeight, aCellSize, aThickness, lWallSideOrigin, 0.0f);

			// Creates door block
			CreateDoorBlockVertical(aCellSize, aDoorPosition, aThickness, lWallSideOrigin);

			// Creates top layer of wall (if there was a door and the wall is taller than the door)
			if (aWallHeight > aCellSize) {
				CreateWallSingleBlockVertical(0.0f, aHeight, aWallHeight - aCellSize, aThickness, lWallSideOrigin, aCellSize);
			}
		}
	}

	private void CreateBlock(GameObject aModel, Vector3 aPosition, Vector3 aScale) {
		GameObject lObject = Instantiate(aModel, aPosition, Quaternion.identity, _parentBlocks);
		lObject.transform.localScale = aScale;
	}

	public void CreateFloorFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lModel = Assets.FloorFrameModel;
		CreateObject(_parentFloorFrames, lModel, aPosition, aRotation, aMaterial);
	}

	public void CreateFloorTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lModel = Assets.FloorTileModels[aModel];
		CreateObject(_parentFloorTiles, lModel, aPosition, aRotation, aMaterial);
	}

	public void CreateWallFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lModel = Assets.WallFrameModel;
		CreateObject(_parentWallFrames, lModel, aPosition, aRotation, aMaterial);
	}

	public void CreateWallTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lModel = Assets.WallTileModels[aModel];
		CreateObject(_parentWallTiles, lModel, aPosition, aRotation, aMaterial);
	}

	public void CreateDoor(Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lModel = Assets.DoorModel;
		CreateObject(_parentDoors, lModel, aPosition, aRotation, aMaterial);
	}

	private void CreateObject(Transform aParent, GameObject aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial) {
		GameObject lObject = Instantiate(aModel, aPosition, aRotation, aParent);
		Material lMaterial = Assets.Materials[aMaterial];
		lObject.GetComponent<Renderer>().material = lMaterial;
	}
}
