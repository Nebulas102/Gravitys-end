using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsScript : MonoBehaviour {
	// Floor Assets
	public GameObject FloorFrameModel;
	public GameObject[] FloorTileModels;
	public int[] FloorTileModelScales;

	// Wall Assets
	public GameObject WallFrameModel;
	public GameObject[] WallTileModels;

	// Door Asset
	public GameObject DoorModel;

	// Materials
	public Material[] Materials;

	// Block Objects
	public GameObject FloorBlock;
	public GameObject WallBlock;
	public GameObject DoorBlock;

	public bool AssetsValid() {
		if (FloorFrameModel == null) return false;
		if (WallFrameModel == null) return false;
		if (DoorModel == null) return false;
		if (FloorBlock == null) return false;
		if (WallBlock == null) return false;
		if (DoorBlock == null) return false;

		if (FloorTileModelScales.Length != FloorTileModels.Length) return false;
		if (FloorTileModels.Length < 1) return false;
		for (int i = 0; i < FloorTileModels.Length; i++) {
			if (FloorTileModels[i] == null) return false;
			if (FloorTileModelScales[i] < 1) return false;
		}

		if (WallTileModels.Length < 1) return false;
		for (int i = 0; i < WallTileModels.Length; i++) {
			if (WallTileModels[i] == null) return false;
		}

		return true;
	}

	public bool FloorTileModelExists(int aModel) {
		return (aModel < FloorTileModels.Length && aModel >= 0);
	}

	public bool WallTileModelExists(int aModel) {
		return (aModel < WallTileModels.Length && aModel >= 0);
	}

	public bool MaterialExists(int aMaterial) {
		return (aMaterial < Materials.Length && aMaterial >= 0);
	}

	public bool MaterialsExist(List<int> aMaterials) {
		for (int i = 0; i < aMaterials.Count; i++) {
			if (!MaterialExists(aMaterials[i]))
				return false;
		}

		return true;
	}
}
