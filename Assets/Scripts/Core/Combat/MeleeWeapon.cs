using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private float damage = 5f;

    private bool isEqupped = false;

    private void Update()
    {
   
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<EnemyBase>())
        {
            other.GetComponent<EnemyBase>().TakeDamage(damage);
            Debug.Log("Damaged enemy");
        }
    }
}
