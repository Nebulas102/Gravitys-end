using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            ObjectiveSystem.Instance.CompleteObjective("Find the bossroom key");
            ObjectiveSystem.Instance.setKeycardCollected(true);
            Destroy(gameObject);
        }
    }
}
