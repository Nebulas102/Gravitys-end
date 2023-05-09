using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        
        switch (tag) {
            case "SpawnRoomTrigger":
                break;
        }
        if (other.tag == "SpawnRoomTrigger"){
            Debug.Log("In the spawn room");
        }
        if (other.tag == "BossRoomTrigger") {
            Debug.Log("In the boss room");
        }
        if (other.tag == "HallwayTrigger") {
            Debug.Log("In the hallway");
        }
    }
}
