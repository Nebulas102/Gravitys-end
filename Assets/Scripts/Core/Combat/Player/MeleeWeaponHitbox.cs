using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    private int startDamage;
    private int endDamage;

    [HideInInspector]
    public bool allowAttack;

    public void SetDamageHitbox(int _startDamage, int _endDamage)
    {
        startDamage = _startDamage;
        endDamage = _endDamage;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Enemy") && allowAttack)
        {
            other.GetComponent<EnemyBase>().TakeDamage(startDamage, endDamage, 0);
        }
    }
}
