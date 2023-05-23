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
    [SerializeField]
    private GameObject hitbox;

    private bool isEquipped = false;

    private void Start()
    {
        hitbox.GetComponent<MeleeWeaponHitbox>().SetDamageHitbox(startDamage, endDamage);
    }

    public void AllowHitbox()
    {
        hitbox.GetComponent<MeleeWeaponHitbox>().allowAttack = true;
    }

    public void DisAllowHitbox()
    {
        hitbox.GetComponent<MeleeWeaponHitbox>().allowAttack = false;
    }
}
