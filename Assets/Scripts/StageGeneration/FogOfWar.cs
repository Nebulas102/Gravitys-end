using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField]
    Material transparent;

    [SerializeField]
    Transform Fog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Fog.GetComponent<Renderer>().material = transparent; ;
        }
    }
}
