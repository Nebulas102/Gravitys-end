using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UnityEngine;

public class PlayerHitboxAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject meleeHitbox;

    private EquipmentSystem equipmentSystem;

    private void Start()
    {
        equipmentSystem = EquipmentSystem.Instance;
    }

    public void EnableHitbox()
    {
        if (equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>() != null)
        {
            MeleeWeapon meleeWeapon = equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>();
            meleeHitbox.SetActive(true);
            MeleeWeaponHitbox meleeWeaponHitbox = meleeHitbox.GetComponent<MeleeWeaponHitbox>();

            meleeWeaponHitbox.allowAttack = true;
            meleeWeaponHitbox.SetDamageHitbox(meleeWeapon.GetMinDamage(), meleeWeapon.GetMaxDamage());
        }
    }

    public void DisableHitbox()
    {
        if (equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>() != null)
        {
            MeleeWeapon meleeWeapon = equipmentSystem._equippedWeapon.GetComponent<MeleeWeapon>();
            MeleeWeaponHitbox meleeWeaponHitbox = meleeHitbox.GetComponent<MeleeWeaponHitbox>();

            meleeWeaponHitbox.allowAttack = false;
            meleeWeaponHitbox.SetDamageHitbox(0, 0);

            meleeHitbox.SetActive(false);
        }
    }
}
