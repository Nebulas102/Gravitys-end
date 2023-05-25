using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModel : MonoBehaviour
{
	public GameObject ClosedModel;

	[SerializeField]
	private GameObject _currentModelObject;

	public void Start() {
		_currentModelObject = transform.Find("DoorOpen").gameObject;
	}

	public void Close() {
		// Gets the current model if unknown
		if (_currentModelObject == null)
			_currentModelObject = transform.Find("DoorOpen").gameObject;

		// Destroys the open model
		Destroy(_currentModelObject);

		// Creates the closed model
		_currentModelObject = Instantiate(ClosedModel, this.transform);
	}
}
