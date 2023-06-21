using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAntiPushing : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.isKinematic = false;
        }
    }
}
