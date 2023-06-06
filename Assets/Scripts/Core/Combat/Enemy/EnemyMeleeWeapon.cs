using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{    
    [SerializeField]
    private int startDamage = 5;
    [SerializeField]
    private int endDamage = 10;
    [SerializeField]
    private GameObject hitbox;

    private bool isEquipped = false;

    private void Start()
    {
        hitbox.GetComponent<EnemyMeleeWeaponHitbox>().SetDamageHitbox(startDamage, endDamage);
    }

    public void MeleeAttack()
    {
        // perform animation
    }

    public void AllowHitbox()
    {
        hitbox.GetComponent<EnemyMeleeWeaponHitbox>().allowAttack = true;
    }

    public void DisAllowHitbox()
    {
        hitbox.GetComponent<EnemyMeleeWeaponHitbox>().allowAttack = false;
    }
}
