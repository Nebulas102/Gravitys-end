using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<int> openSides = new List<int> { 0, 1, 2, 3 };

    public int id = 0;

    [HideInInspector]
    public bool rayCastReady = false;

    [HideInInspector]
    public Ray topRay;
    [HideInInspector]
    public Ray rightRay;
    [HideInInspector]
    public Ray bottomRay;
    [HideInInspector]
    public Ray leftRay;

    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;

        transform.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (rayCastReady == true)
        {
            gameObject.GetComponent<RoomCastBehaviour>().RayCastsDirectionsStandardRoom();
            gameObject.GetComponent<RoomCastBehaviour>().RayCastDirectionCheck();
        }
    }

    public void DisplayOpenSides()
    {
        Debug.Log("Room " + id + " open sides: " + string.Join(", ", openSides));
    }
}
