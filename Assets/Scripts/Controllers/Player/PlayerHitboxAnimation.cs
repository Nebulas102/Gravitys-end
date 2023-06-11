using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UnityEngine;

public class PlayerHitboxAnimation : MonoBehaviour
{
    private EquipmentSystem equipmentSystem;

    private void Start()
    {
        equipmentSystem = EquipmentSystem.Instance;
    }

    public void EnableHitbox()
    {
        if (equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>() != null)
        {
            Debug.Log("allow hit");
            equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>().AllowHitbox();
        }
    }

    public void DisableHitbox()
    {
        if (equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>() != null)
        {  
            Debug.Log("disallow hit");
            equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>().DisAllowHitbox();
        }
    }
}
