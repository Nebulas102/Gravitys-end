using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModel : MonoBehaviour
{
	public GameObject OpenModel;
	public GameObject ClosedModel;

	public GameObject _currentModelObject;

	public void Start() {
		_currentModelObject = transform.Find("DoorModel").gameObject;
	}

	public void Close() {
		Destroy(_currentModelObject);
		_currentModelObject = Instantiate(ClosedModel, this.transform);
	}
}
