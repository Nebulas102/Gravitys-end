using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RoomBuilder {
	private const int FloorTileWidth = 5;
	private const int FloorTileHeight = 5;
	private const int WallTileWidth = 5;
	private const int WallTileHeight = 5;
	private Quaternion DefaultFloorRotation = Quaternion.identity;
	private Quaternion DefaultWallRotation = Quaternion.identity;

	public enum WallSide {
		North = 0,
		South = 1,
		East = 2,
		West = 3
	}



	public class TileType {
		public int Model;
		public List<int> Materials;
		public Quaternion Rotation;
		public float SpawnChanceWeight;

		public TileType(int aModel, List<int> aMaterials, Quaternion aRotation, float aSpawnChanceWeight) {
			Model = aModel;
			Materials = aMaterials;
			Rotation = aRotation;
			SpawnChanceWeight = aSpawnChanceWeight;
		}

		public TileType(TileType aTileType) {
			Model = aTileType.Model;
			Materials = new List<int>(aTileType.Materials);
			Rotation = aTileType.Rotation;
			SpawnChanceWeight = aTileType.SpawnChanceWeight;
		}
	}

	public class FloorTile {
		public float x, y;
		public int TileType;
		public int Material;
		public bool IsCovered;

		public FloorTile(float aX, float aY, int aTileType, int aMaterial) {
			x = aX;
			y = aY;
			TileType = aTileType;
			Material = aMaterial;
			IsCovered = false;
		}
	}

	public class WallTile {
		public int x, y, WallSide;
		public int TileType;
		public int Material;
		public bool IsDoor;

		public WallTile(int aX, int aY, int aWallSide, int aTileType, int aMaterial, bool isDoor) {
			x = aX;
			y = aY;
			WallSide = aWallSide;
			TileType = aTileType;
			Material = aMaterial;
			IsDoor = isDoor;
		}
	}

	//
	RoomScript _room;
	AssetsScript _assets;

	// Room basic data
	int _width = 5;
	int _height = 5;
	int _wallHeight = 5;

	// Door data
	int[] _doorPositions;
	int _doorDefaultMaterial = 0;

	// Floor data
	List<TileType> _floorTileTypes;
	List<FloorTile> _floorCustomTiles;
	FloorTile[,] _floorTiles;
	float _floorRandomSpawnChance = 0.0f;

	// Wall data
	List<TileType> _wallTileTypes;
	List<WallTile> _wallCustomTiles;
	List<WallTile[,]> _wallTiles;
	float _wallRandomSpawnChance = 0.0f;

	// Constructor
	public RoomBuilder(RoomScript aRoom, AssetsScript aAssets) {
		//
		_room = aRoom;
		_assets = aAssets;

		// Initialises door data
		_doorPositions = new int[4] {-1, -1, -1, -1};

		// Initialises floor data
		DefaultFloorRotation.eulerAngles = new Vector3(-90, 0, 0);
		_floorTileTypes = new List<TileType>();
		_floorTileTypes.Add(new TileType(0, new List<int>(), DefaultFloorRotation, 0));
		_floorTileTypes[0].Materials.Add(0);
		_floorCustomTiles = new List<FloorTile>();
		_floorTiles = new FloorTile[0,0];

		// Initialises wall data
		DefaultWallRotation.eulerAngles = new Vector3(-90, 0, 0);
		_wallTileTypes = new List<TileType>();
		_wallTileTypes.Add(new TileType(0, new List<int>(), DefaultWallRotation, 0));
		_wallTileTypes[0].Materials.Add(0);
		_wallTileTypes.Add(new TileType(0, new List<int>(), DefaultWallRotation, 0));
		_wallCustomTiles = new List<WallTile>();
		_wallTiles = new List<WallTile[,]>(4);
		_wallTiles.Add(null);
		_wallTiles.Add(null);
		_wallTiles.Add(null);
		_wallTiles.Add(null);
	}

	private bool CompliantRoomDimension(int a) {
		return (((a + 5) / 10) * 10 == a + 5);
	}


	/*   Building Floors   */

	//
	private void PopulateFloorTiles() {
		_floorTiles = new FloorTile[RoomWidth/FloorTileWidth, RoomHeight/FloorTileHeight];
		for (int x = 0; x < _floorTiles.GetLength(0); x++) {
			for (int y = 0; y < _floorTiles.GetLength(1); y++) {
				FloorTile newTile = new FloorTile(x, y, 0, 0);
				_floorTiles[x,y] = newTile;
			}
		}
	}

	// Checks if a tile with the given x, y and scale would overlap any tiles in _floorCustomTiles
	private bool NewFloorTileOverlapsAnyCustomTiles(int x, int y, int aScale) {
		bool lOverlapsOtherCustomTiles = false;

		// Coordinates of the proposed tile
		List<int> lProposedXCoords = new List<int>();
		List<int> lProposedYCoords = new List<int>();
		for (int i = 0; i < aScale; i++) {
			for (int j = 0; j < aScale; j++) {
				lProposedXCoords.Add(x+i);
				lProposedYCoords.Add(y+j);
			}
		}

		for (int lCustomTileIndex = 0; lCustomTileIndex < _floorCustomTiles.Count; lCustomTileIndex++) {
			FloorTile lTile = _floorCustomTiles[lCustomTileIndex];
			int lModel = _floorTileTypes[lTile.TileType].Model;
			int lScale = _assets.FloorTileModelScales[lModel];
			for (int lProposedCoordIndex = 0; lProposedCoordIndex < lProposedXCoords.Count; lProposedCoordIndex++) {
				int lCurrentProposedX = lProposedXCoords[lProposedCoordIndex];
				int lCurrentProposedY = lProposedYCoords[lProposedCoordIndex];
				for (int i = 0; i < lScale; i++) {
					for (int j = 0; j < lScale; j++) {
						int lTileX = (int)lTile.x + i;
						int lTileY = (int)lTile.y + j;
						if (lCurrentProposedX == lTileX)
							if (lCurrentProposedY == lTileY)
								lOverlapsOtherCustomTiles = true;
					}
				}
			}
		}

		return lOverlapsOtherCustomTiles;
	}

	// Randomly creates custom floor tiles
	private void CreateRandomFloorTiles() {
		if (_floorTileTypes.Count > 1) {
			// Calculates spawn weight total
			float lSpawnChanceWeightTotal = 0;
			for (int i = 0; i < _floorTileTypes.Count; i++) {
				lSpawnChanceWeightTotal += _floorTileTypes[i].SpawnChanceWeight;
			}

			// Checks each tile and replaces with a tile variation based on random chance
			for (int x = 0; x < _floorTiles.GetLength(0); x++) {
				for (int y = 0; y < _floorTiles.GetLength(1); y++) {
					if (Random.value < _floorRandomSpawnChance) {
						// Chooses which floor tile type to create based on the spawn chance weights
						float lTypeRandomFloat = Random.Range(0, lSpawnChanceWeightTotal);
						float lCurrentFloat = 0;
						int lType = 0;
						for (int i = 0; i < _floorTileTypes.Count; i++) {
							lCurrentFloat += _floorTileTypes[i].SpawnChanceWeight;
							if (lCurrentFloat > lTypeRandomFloat) {
								lType = i;
								break;
							}
						}

						// Chooses a random material for the floor tile type with equal weight to all materials
						int lMaterial = Random.Range(0, _floorTileTypes[lType].Materials.Count);

						// Checks that the location for placing the tile is possible
						bool lCanPlaceTile = true;
						int lScale = _assets.FloorTileModelScales[_floorTileTypes[lType].Model];
						// Ensures that the tile doesn't stick out of the room
						if (x + lScale - 1 >= _width / FloorTileWidth)
							lCanPlaceTile = false;
						if (y + lScale - 1 >= _height / FloorTileHeight)
							lCanPlaceTile = false;
						// Ensures that the tile doesn't overlap any other custom tiles
						if (NewFloorTileOverlapsAnyCustomTiles(x, y, lScale))
							lCanPlaceTile = false;

						// Adds the tile if possible
						if (lCanPlaceTile)
							_floorCustomTiles.Add(new FloorTile(x, y, lType, lMaterial));
					}
				}
			}
		}
	}

	// Adds the custom floor tiles into the actual array
	private void PopulateCustomFloorTilesIntoArray() {
		for (int i = 0; i < _floorCustomTiles.Count; i++) {
			FloorTile lTile = _floorCustomTiles[i];
			TileType lTileType = _floorTileTypes[lTile.TileType];

			// Finds the x,y index within _floorTiles for this custom tile
			int xIndex = (int)lTile.x;
			int yIndex = (int)lTile.y;

			// If the tile is a non-standard size, the x,y coordinates need to be adjusted,
			// and the other tiles it would overlap need to be removed
			int lScale = _assets.FloorTileModelScales[lTileType.Model];
			if (lScale > 1) {
				// Adjusts x,y coordinates for the large tile
				float xPos = (float)xIndex + (((float)lScale - 1) / 2);
				float yPos = (float)yIndex + (((float)lScale - 1) / 2);
				lTile.x = xPos;
				lTile.y = yPos;

				// Removes other tiles that this tile would overlap
				for (int xOffset = 0; xOffset < lScale; xOffset++) {
					for (int yOffset = 0; yOffset < lScale; yOffset++) {
						_floorTiles[xIndex + xOffset, yIndex + yOffset].IsCovered = true;
					}
				}
			}

			// Puts lTile into _floorTiles
			_floorTiles[xIndex,yIndex] = lTile;
		}
	}

	//
	private void BuildFloors() {
		// Creates frame and tile objects
		for (int ix = 0; ix < _floorTiles.GetLength(0); ix++) {
			for (int iy = 0; iy < _floorTiles.GetLength(1); iy++) {
				// Base position ("origin" for the whole floor)
				Vector3 lBasePos = _room.transform.position;
				lBasePos -= new Vector3((float)RoomWidth / 2, 0, (float)RoomHeight / 2);
				lBasePos += new Vector3((float)FloorTileWidth / 2, 0, (float)FloorTileHeight / 2);
				float x, y;

				// Position of the frame relative to lBasePos
				x = ix;
				y = iy;
				Vector3 lFramePos = lBasePos + new Vector3(x*(float)FloorTileWidth, 0, y*(float)FloorTileHeight);

				// Position of the tile relative to lBasePos
				FloorTile lTile = _floorTiles[ix,iy];
				x = lTile.x;
				y = lTile.y;
				Vector3 lPos = lBasePos + new Vector3(x*(float)FloorTileWidth, 0, y*(float)FloorTileHeight);

				// Sets rotation and material
				TileType lTileType = _floorTileTypes[lTile.TileType];
				Quaternion lRot = lTileType.Rotation;
				int lMaterial = lTileType.Materials[lTile.Material];

				// Creates frame object
				_room.CreateFloorFrame(lFramePos, lRot, lMaterial);

				// Creates tile object
				if (!lTile.IsCovered)
					_room.CreateFloorTile(lTileType.Model, lPos, lRot, lMaterial);
			}
		}
	}

	/*   ==========================   */



	/*  ===  Building Walls  ===  */

	// Fills _wallTiles with arrays of the correct size based on the room dimensions
	private void PrepareWallTileArrays() {
		_wallTiles[(int)WallSide.North] = new WallTile[RoomWidth/WallTileWidth, WallHeight/WallTileHeight];
		_wallTiles[(int)WallSide.South] = new WallTile[RoomWidth/WallTileWidth, WallHeight/WallTileHeight];
		_wallTiles[(int)WallSide.West] = new WallTile[RoomHeight/WallTileWidth, WallHeight/WallTileHeight];
		_wallTiles[(int)WallSide.East] = new WallTile[RoomHeight/WallTileWidth, WallHeight/WallTileHeight];
	}

	// Fills all of _wallTiles with the deafault wall tile types
	private void PopulateWallTiles() {
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < _wallTiles[i].GetLength(0); j++) {
				for (int k = 0; k < _wallTiles[i].GetLength(1); k++) {
					_wallTiles[i][j,k] = new WallTile(j, k, i, 0, 0, false);
				}
			}
		}
	}

	// Randomly creates custom wall tiles
	private void CreateRandomWallTiles() {
		if (_wallTileTypes.Count > 2) {
			// Calculates spawn weight total
			float lSpawnChanceWeightTotal = 0;
			for (int i = 0; i < _wallTileTypes.Count; i++) {
				lSpawnChanceWeightTotal += _wallTileTypes[i].SpawnChanceWeight;
			}

			// Checks each tile and replaces with a tile variation based on random chance
			for (int s = 0; s < 4; s++) {
				for (int x = 0; x < _wallTiles[s].GetLength(0); x++) {
					for (int y = 0; y < _wallTiles[s].GetLength(1); y++) {
						if (Random.value < _wallRandomSpawnChance) {
							// Chooses which floor tile type to create based on the spawn chance weights
							float lTypeRandomFloat = Random.Range(0, lSpawnChanceWeightTotal);
							float lCurrentFloat = 0;
							int lType = 0;
							for (int i = 0; i < _wallTileTypes.Count; i++) {
								lCurrentFloat += _wallTileTypes[i].SpawnChanceWeight;
								if (lCurrentFloat > lTypeRandomFloat) {
									lType = i;
									break;
								}
							}

							// Chooses a random material for the floor tile type with equal weight to all materials
							int lMaterial = Random.Range(0, _wallTileTypes[lType].Materials.Count);

							// Adds the tile
							_wallCustomTiles.Add(new WallTile(x, y, s, lType, lMaterial, false));
						}
					}
				}
			}
		}
	}

	// Adds the custom wall tiles
	private void PopulateCustomWallTilesIntoArray() {
		for (int i = 0; i < _wallCustomTiles.Count; i++) {
			WallTile lTile = _wallCustomTiles[i];
			TileType lTileType = _wallTileTypes[lTile.TileType];

			// Finds the x,y index within _wallTiles for this custom tile
			int xIndex = (int)lTile.x;
			int yIndex = (int)lTile.y;

			// Puts lTile into _wallTiles
			_wallTiles[lTile.WallSide][xIndex,yIndex] = lTile;
		}
	}

	// Adds door tiles to _wallTiles in all door positions
	private void PopulateDoors() {
		for (int i = 0; i < _doorPositions.Length; i++) {
			if (_doorPositions[i] >= 0) {
				int x = _doorPositions[i] / WallTileWidth;
				int y = 0;
				_wallTiles[i][x,y] = new WallTile(x, y, i, 0, _doorDefaultMaterial, true);
			}
		}
	}

	// Builds one side of the wall
	private void BuildWallSide(int aSide, Vector3 aBasePos, Quaternion aRot, Vector3 aXOffset, Vector3 aYOffset) {
		for (int x = 0; x < _wallTiles[aSide].GetLength(0); x++) {
			for (int y = 0; y < _wallTiles[aSide].GetLength(1); y++) {
				// Gets the position for this slot (frame and tile)
				Vector3 lPos = aBasePos + x*aXOffset + y*aYOffset;

				// Gets the material
				WallTile lTile = _wallTiles[aSide][x,y];
				TileType lTileType = _wallTileTypes[lTile.TileType];
				int lMaterial = lTileType.Materials[lTile.Material];

				// Creates wall frame
				if (!_wallTiles[aSide][x,y].IsDoor)
					_room.CreateWallFrame(lPos, aRot, lMaterial);

				// Creates wall tile
				if (lTile.IsDoor) {
					_room.CreateDoor(lPos, aRot, lMaterial);
				} else {
					_room.CreateWallTile(lTileType.Model, lPos, aRot, lMaterial);
				}
			}
		}
	}

	// Builds wall frames, tiles and doors
	private void BuildWalls() {
		// Creates frame and tile objects
		for (int i = 0; i < 4; i++) {
			WallSide side = (WallSide)i;

			/*
				Sets base position, x offset, y offset and rotation
				lXOffset is a vector for one wall tile unit in the x direction on this wall
				lYOffset is a vector for one wall tile unit in the y direction on this wall
				lBasePos is the origin (0,0) on this wall
				lRot is the rotation for wall tiles/frames on this wall
			*/
			Vector3 lBasePos = _room.transform.position;
			Quaternion lRot = Quaternion.identity;
			Vector3 lXOffset = Vector3.zero;
			Vector3 lYOffset = new Vector3(0, WallTileHeight, 0);
			float w = (float)RoomWidth / 2;
			float h = (float)RoomHeight / 2;
			switch (side) {
				case WallSide.North:
					lBasePos.x -= w;
					lBasePos.z += h;
					lRot.eulerAngles = new Vector3(-90, 180, 0);
					lXOffset = new Vector3(WallTileWidth, 0, 0);
					lBasePos.x += (float)WallTileWidth / 2;
					break;
				case WallSide.South:
					lBasePos.x -= w;
					lBasePos.z -= h;
					lRot.eulerAngles = new Vector3(-90, 0, 0);
					lXOffset = new Vector3(WallTileWidth, 0, 0);
					lBasePos.x += (float)WallTileWidth / 2;
					break;
				case WallSide.West:
					lBasePos.x -= w;
					lBasePos.z -= h;
					lRot.eulerAngles = new Vector3(-90, 90, 0);
					lXOffset = new Vector3(0, 0, WallTileWidth);
					lBasePos.z += (float)WallTileWidth / 2;
					break;
				case WallSide.East:
					lBasePos.x += w;
					lBasePos.z -= h;
					lRot.eulerAngles = new Vector3(-90, 270, 0);
					lXOffset = new Vector3(0, 0, WallTileWidth);
					lBasePos.z += (float)WallTileWidth / 2;
					break;
			}

			BuildWallSide(i, lBasePos, lRot, lXOffset, lYOffset);
		}
	}

	/*   ==========================   */



	/*  ===  Building Room  ===  */

	// Checks if the doors are valid
	private bool DoorsAreValid() {
		if (NumberOfDoors() < 1) return false;

		for (int i = 0; i < _doorPositions.Length; i++) {
			if (!DoorIsValid((WallSide)i, _doorPositions[i])) return false;
		}

		return true;
	}

	// Checks if a given tile type is a valid floor tile type
	public bool FloorTileTypeIsValid(TileType aTileType) {
		if (!_assets.FloorTileModelExists(aTileType.Model)) return false;
		if (!_assets.MaterialsExist(aTileType.Materials)) return false;

		return true;
	}

	// Checks if a given tile type is a valid wall tile type
	public bool WallTileTypeIsValid(TileType aTileType) {
		if (!_assets.WallTileModelExists(aTileType.Model)) return false;
		if (!_assets.MaterialsExist(aTileType.Materials)) return false;

		return true;
	}

	// Re-Checks that all values are valid (i.e. in case the assets object was modified)
	public bool CanBuildRoom() {
		// Checks if doors are valid
		if (!DoorsAreValid()) return false;

		// Checks if floor types are valid
		for (int i = 0; i < _floorTileTypes.Count; i++) {
			if (!FloorTileTypeIsValid(_floorTileTypes[i])) return false;
		}

		// Checks if wall types are valid
		for (int i = 0; i < _wallTileTypes.Count; i++) {
			if (!WallTileTypeIsValid(_wallTileTypes[i])) return false;
		}

		return true;
	}

	// Physically builds the objects of the room in the scene
	public void BuildRoom() {
		ClearCustomWallTiles();
		ClearCustomFloorTiles();
		// Clears the room if necessary
		_room.ClearAllObjects();
		_room.CreateParents();

		// Builds the floor
		PopulateFloorTiles();
		CreateRandomFloorTiles();
		PopulateCustomFloorTilesIntoArray();
		BuildFloors();

		// Creates the _wallTiles arrays to the right size, to prepare for the doors and walls
		PrepareWallTileArrays();
		// Populates _wallTiles with the walls
		PopulateWallTiles();
		// Creates random wall tiles into _wallCustomTiles
		CreateRandomWallTiles();
		// Populates _wallCustomTiles into _wallTiles
		PopulateCustomWallTilesIntoArray();
		// Populates _wallTiles with the doors
		PopulateDoors();
		// Builds the contents of _wallTiles (including doors)
		BuildWalls();

		// Builds the objects to be used by the stage generator and AI
		_room.CreateBlocks(_width, _height, _wallHeight, WallTileHeight, 0.1f, _doorPositions);
	}

	/*   ==========================   */



	/*  ===  Editing Doors  ===  */

	// Gets the number of doors
	public int NumberOfDoors() {
		int n = 0;
		for (int i = 0; i < _doorPositions.Length; i++) {
			if (_doorPositions[i] >= 0)
				n++;
		}
		return n;
	}

	// Gets the door position for the given side
	public int GetDoor(WallSide side) {
		return _doorPositions[(int)side];
	}

	// Checks if the given door value is within the limits
	private bool DoorIsValid(WallSide side, int pos) {
		if (pos == -1)
			return true;

		if ((pos / 5) * 5 == pos)
			if (pos >= 0)
				if (side == WallSide.North || side == WallSide.South) {
					if (pos <= RoomWidth - 5)
						return true;
				} else {
					if (pos <= RoomHeight - 5)
						return true;
				}

		return false;
	}

	// Sets the door for the given side, if valid
	public void SetDoor(WallSide side, int pos) {
		if (DoorIsValid(side, pos))
			_doorPositions[(int)side] = pos;
	}

	/*   ==========================   */



	/*  ===  Editing Custom Floor Tile Types  ===  */

	//
	public int AddCustomFloorTileType() {
		TileType lNewTile = new TileType(0, new List<int>(), DefaultFloorRotation, 1);
		lNewTile.Materials.Add(0);
		_floorTileTypes.Add(lNewTile);
		return _floorTileTypes.Count - 1;
	}

	//
	public void RemoveCustomFloorTileType(int aTypeID) {
		if (aTypeID > 0 && aTypeID < _floorTileTypes.Count)
			_floorTileTypes.RemoveAt(aTypeID);
	}

	//
	public void EditCustomFloorTileType(int aTypeID, int aModel, List<int> aMaterials, float aSpawnChanceWeight) {
		if (aTypeID > 0 && aTypeID < _floorTileTypes.Count) {
			// Sets the model if the model exists
			if (_assets.FloorTileModelExists(aModel))
				_floorTileTypes[aTypeID].Model = aModel;

			// Sets the spawn chance if the number is positive
			if (aSpawnChanceWeight >= 0)
				_floorTileTypes[aTypeID].SpawnChanceWeight = aSpawnChanceWeight;

			// Sets the materials if all materials exist
			if (_assets.MaterialsExist(aMaterials))
				_floorTileTypes[aTypeID].Materials = aMaterials;
		} else {
			Debug.Log("Error: Invalid aTypeID for floor custom tile type: " + aTypeID.ToString());
		}
	}

	//
	public int GetCustomFloorTileTypeCount() {
		return _floorTileTypes.Count;
	}

	//
	public TileType GetCustomFloorTileType(int aTypeID) {
		if (aTypeID > 0 && aTypeID < _floorTileTypes.Count) {
			return new TileType(_floorTileTypes[aTypeID]);
		} else {
			Debug.Log("Error: Invalid aTypeID for floor custom tile type: " + aTypeID.ToString());
			return null;
		}
	}

	//
	private bool ClearCustomFloorTiles() {
		bool lCleared = false;

		if (_floorCustomTiles.Count > 0) lCleared = true;

		_floorCustomTiles.Clear();

		return lCleared;
	}

	/*   ==========================   */



	/*  ===  Editing Custom Wall Tile Types  ===  */

	//
	public int AddCustomWallTileType() {
		TileType lNewTile = new TileType(0, new List<int>(), DefaultWallRotation, 1);
		lNewTile.Materials.Add(0);
		_wallTileTypes.Add(lNewTile);
		return _wallTileTypes.Count - 1;
	}

	//
	public void RemoveCustomWallTileType(int aTypeID) {
		if (aTypeID > 0 && aTypeID < _wallTileTypes.Count)
			_wallTileTypes.RemoveAt(aTypeID);
	}

	//
	public void EditCustomWallTileType(int aTypeID, int aModel, List<int> aMaterials, float aSpawnChanceWeight) {
		if (aTypeID > 1 && aTypeID < _wallTileTypes.Count) {
			// Sets the model if the model exists
			if (_assets.WallTileModelExists(aModel))
				_wallTileTypes[aTypeID].Model = aModel;

			// Sets the spawn chance if the number is positive
			if (aSpawnChanceWeight >= 0)
				_wallTileTypes[aTypeID].SpawnChanceWeight = aSpawnChanceWeight;

			// Sets the materials if all materials exist
			if (_assets.MaterialsExist(aMaterials))
				_wallTileTypes[aTypeID].Materials = aMaterials;
		} else {
			Debug.Log("Error: Invalid aTypeID for wall custom tile type: " + aTypeID.ToString());
		}
	}

	//
	public int GetCustomWallTileTypeCount() {
		return _wallTileTypes.Count;
	}

	//
	public TileType GetCustomWallTileType(int aTypeID) {
		if (aTypeID > 1 && aTypeID < _wallTileTypes.Count) {
			return new TileType(_wallTileTypes[aTypeID]);
		} else {
			Debug.Log("Error: Invalid aTypeID for wall custom tile type: " + aTypeID.ToString());
			return null;
		}
	}

	//
	private bool ClearCustomWallTiles() {
		bool lCleared = false;

		if (_wallCustomTiles.Count > 0) lCleared = true;

		_wallCustomTiles.Clear();

		return lCleared;
	}

	/*   ==========================   */



	/*  ===  Saving and Loading Data to Files  ===  */

	public void SaveData(string aFilePath) {
		FileStream lFile;

		if (File.Exists(aFilePath))
			lFile = File.OpenWrite(aFilePath);
		else
			lFile = File.Create(aFilePath);

		RoomData lData = new RoomData(_width, _height, _wallHeight, _doorPositions, _doorDefaultMaterial, _floorTileTypes, _floorRandomSpawnChance, _wallTileTypes, _wallRandomSpawnChance);


		BinaryFormatter lBinaryFormatter = new BinaryFormatter();
		lBinaryFormatter.Serialize(lFile, lData);

		lFile.Close();
	}

	public void LoadData(string aFilePath) {
		FileStream lFile;

		if (File.Exists(aFilePath))
			lFile = File.OpenRead(aFilePath);
		else {
			Debug.Log("Error: Invalid filename");
			return;
		}

		BinaryFormatter lBinaryFormatter = new BinaryFormatter();
		RoomData lData = (RoomData)lBinaryFormatter.Deserialize(lFile);
		lFile.Close();

		_width = lData.Width;
		_height = lData.Height;
		_wallHeight = lData.WallHeight;

		_doorPositions = lData.DoorPositions;
		_doorDefaultMaterial = lData.DoorMaterial;

		_floorTileTypes = lData.FloorTileTypes;
		_floorRandomSpawnChance = lData.FloorRandomSpawnChance;

		_wallTileTypes = lData.WallTileTypes;
		_wallRandomSpawnChance = lData.WallRandomSpawnChance;
	}

	/*   ==========================   */



	public int RoomWidth {
		get {return _width;}
		set {
			int newWidth = Mathf.Abs(value);
			if (CompliantRoomDimension(newWidth))
				_width = newWidth;
		}
	}
	public int RoomHeight {
		get {return _height;}
		set {
			int newHeight = Mathf.Abs(value);
			if (CompliantRoomDimension(newHeight))
				_height = newHeight;
		}
	}
	public int WallHeight {
		get {return _wallHeight;}
		set {
			int newWallHeight = Mathf.Abs(value);
			if ((newWallHeight / 5) * 5 == newWallHeight)
				if (newWallHeight > 0)
						_wallHeight = newWallHeight;
		}
	}

	// Door Material
	public int DefaultDoorMaterial {
		get {return _doorDefaultMaterial;}
		set {
			if (_assets.MaterialExists(value))
				_doorDefaultMaterial = value;
		}
	}

	// Floor Default Tile Model
	public int FloorDefaultTileModel {
		get {return _floorTileTypes[0].Model;}
		set {
			if (_assets.FloorTileModelExists(value))
				if (_assets.FloorTileModelScales[value] == 1)
					_floorTileTypes[0].Model = value;
		}
	}

	// Floor Default Material
	public int FloorDefaultTileMaterial {
		get {return _floorTileTypes[0].Materials[0];}
		set {
			if (_assets.MaterialExists(value))
				_floorTileTypes[0].Materials[0] = value;
		}
	}

	// Wall Default Tile Model
	public int WallDefaultTileModel {
		get {return _wallTileTypes[0].Model;}
		set {
			if (_assets.WallTileModelExists(value))
				_wallTileTypes[0].Model = value;
		}
	}

	// Wall Default Material
	public int WallDefaultTileMaterial {
		get {return _wallTileTypes[0].Materials[0];}
		set {
			if (_assets.MaterialExists(value))
				_wallTileTypes[0].Materials[0] = value;
		}
	}

	// Spawn Chances for floor and wall variations
	public float FloorRandomSpawnChance {
		get {return _floorRandomSpawnChance;}
		set {
			if (value >= 0.0f && value <= 1.0f)
				_floorRandomSpawnChance = value;
		}
	}
	public float WallRandomSpawnChance {
		get {return _wallRandomSpawnChance;}
		set {
			if (value >= 0.0f && value <= 1.0f)
				_wallRandomSpawnChance = value;
		}
	}
}
