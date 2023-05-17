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

    [HideInInspector]
    public bool allowAttack = false;

    private void Start()
    {
        hitbox = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
   
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Enemy") && allowAttack)
        {
            other.GetComponent<EnemyBase>().TakeDamage(startDamage, endDamage, 0);
        }
    }
}
