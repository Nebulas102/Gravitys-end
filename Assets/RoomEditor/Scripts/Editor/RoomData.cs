using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData {
	[System.Serializable]
	class TileTypeSerializable {
		public int Model;
		public List<int> Materials;
		public float[] Rotation;
		public float SpawnChanceWeight;

		public TileTypeSerializable(RoomBuilder.TileType aTileType) {
			Model = aTileType.Model;

			Materials = aTileType.Materials;

			Vector3 lVec = aTileType.Rotation.eulerAngles;
			Rotation = new float[3] {lVec.x, lVec.y, lVec.z};

			SpawnChanceWeight = aTileType.SpawnChanceWeight;
		}

		public RoomBuilder.TileType GetTileType() {
			Quaternion lRot = Quaternion.identity;
			lRot.eulerAngles = new Vector3(Rotation[0], Rotation[1], Rotation[2]);

			return new RoomBuilder.TileType(Model, Materials, lRot, SpawnChanceWeight);
		}
	}

	// Room basic data
	public int Width;
	public int Height;
	public int WallHeight;

	// Door data
	public int[] DoorPositions;
	public int DoorMaterial;

	// Floor data
	TileTypeSerializable[] _floorTileTypes;
	public List<RoomBuilder.TileType> FloorTileTypes {
		get {
			List<RoomBuilder.TileType> lTileTypes = new List<RoomBuilder.TileType>();

			for (int i = 0; i < _floorTileTypes.Length; i++) {
				lTileTypes.Add(_floorTileTypes[i].GetTileType());
			}

			return lTileTypes;
		}
		set {
			_floorTileTypes = new TileTypeSerializable[value.Count];

			for (int i = 0; i < value.Count; i++) {
				_floorTileTypes[i] = new TileTypeSerializable(value[i]);
			}
		}
	}
	public float FloorRandomSpawnChance = 0.0f;

	// Wall data
	TileTypeSerializable[] _wallTileTypes;
	public List<RoomBuilder.TileType> WallTileTypes {
		get {
			List<RoomBuilder.TileType> lTileTypes = new List<RoomBuilder.TileType>();

			for (int i = 0; i < _wallTileTypes.Length; i++) {
				lTileTypes.Add(_wallTileTypes[i].GetTileType());
			}

			return lTileTypes;
		}
		set {
			_wallTileTypes = new TileTypeSerializable[value.Count];

			for (int i = 0; i < value.Count; i++) {
				_wallTileTypes[i] = new TileTypeSerializable(value[i]);
			}
		}
	}
	public float WallRandomSpawnChance = 0.0f;

	public RoomData(int aWidth, int aHeight, int aWallHeight, int[] aDoorPositions, int aDoorMaterial, List<RoomBuilder.TileType> aFloorTileTypes, float aFloorRandomSpawnChance, List<RoomBuilder.TileType> aWallTileTypes, float aWallRandomSpawnChance) {
		Width = aWidth;
		Height = aHeight;
		WallHeight = aWallHeight;

		DoorPositions = aDoorPositions;
		DoorMaterial = aDoorMaterial;

		FloorTileTypes = aFloorTileTypes;
		FloorRandomSpawnChance = aFloorRandomSpawnChance;

		WallTileTypes = aWallTileTypes;
		WallRandomSpawnChance = aWallRandomSpawnChance;
	}
}
