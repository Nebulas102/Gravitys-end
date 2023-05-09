using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{    
    [SerializeField]
    private int startDamage = 5;
    [SerializeField]
    private int endDamage = 10;

    private bool isEquipped = false;
    private Collider hitbox;

    private void Start()
    {
        hitbox = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
   
    }

    //it wont even read the enemy or other objects, only the player
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().TakeDamage(startDamage, endDamage, 0);
        }
    }
}
