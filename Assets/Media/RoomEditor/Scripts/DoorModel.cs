using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModel : MonoBehaviour
{
    public GameObject ClosedModel;

    [SerializeField]
    private GameObject _currentModelObject;

    private GameObject OpenModel;

    public void Start()
    {
        _currentModelObject = transform.Find("Door Open").gameObject;
        OpenModel = _currentModelObject;
    }

    public void Close()
    {
        // Gets the current model if unknown
        if (_currentModelObject == null)
            _currentModelObject = transform.Find("Door Open").gameObject;

        // Destroys the open model
        Destroy(_currentModelObject);

        // Creates the closed model
        _currentModelObject = Instantiate(ClosedModel, this.transform);
    }

    public void Open()
    {
        // Gets the current model if unknown
        if (_currentModelObject == null)
            _currentModelObject = transform.Find("Door Close").gameObject;

        // Destroys the closed model
        Destroy(_currentModelObject);

        // Creates the open model
        _currentModelObject = Instantiate(OpenModel, this.transform);
    }
}
