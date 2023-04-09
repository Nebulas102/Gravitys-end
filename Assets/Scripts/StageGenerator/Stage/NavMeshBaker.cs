using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        for(int i = 0; i < navMeshSurfaces.Length; i++) 
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }
}
