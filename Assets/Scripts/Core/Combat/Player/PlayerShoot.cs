using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadEvent;

    [HideInInspector]
    public RangeWeapon weapon;

    private void Start()
    {
        weapon = gameObject.GetComponent<RangeWeapon>();
    }

    public void OnWeaponEquipped()
    {
        shootInput += weapon.Shoot;
        reloadEvent += weapon.StartReload;
    }

    public void OnWeaponUnequipped()
    {
        shootInput -= weapon.Shoot;
        reloadEvent -= weapon.StartReload;
    }

    public void Shoot()
    {
        if (EquipmentSystem.Instance._equippedWeapon == gameObject)
        {
            if (weapon.currentAmmo <= 0)
            {
                reloadEvent?.Invoke();
            }
            else
            {
                shootInput?.Invoke();
            }
        }
    }
}
