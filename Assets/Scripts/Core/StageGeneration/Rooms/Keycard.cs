using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    [SerializeField]
    private GameObject mapIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectiveSystem.instance.HandleKeycardCollected();
            Destroy(gameObject);

            if(mapIcon != null)
                mapIcon.SetActive(false);
        }
    }
}
