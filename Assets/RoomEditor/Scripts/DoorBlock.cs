using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlock : MonoBehaviour
{
	public GameObject WallBlock;
	public GameObject DoorModelObject;

	public void OnDestroy() {
		// Closes the door model
		DoorModelObject.GetComponent<DoorModel>().Close();

		// Creates a wall block
		GameObject lWallBlockObject = Instantiate(WallBlock, this.transform.position, this.transform.rotation, this.transform.parent);
		lWallBlockObject.transform.localScale = this.transform.localScale;
	}
}
