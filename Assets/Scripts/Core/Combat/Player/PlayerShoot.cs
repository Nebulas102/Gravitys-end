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
    public InputAction attackAction;
    [HideInInspector]
    public RangeWeapon weapon;

    private PlayerManager playerManager;

    private void Start()
    {
        weapon = gameObject.GetComponent<RangeWeapon>();
    }

    private void Update()
    {
        if (playerManager != null)
        {
            if (playerManager.player.GetComponent<Character>().movementSM.currentState == playerManager.player.GetComponent<Character>().combatting)
            {
                attackAction = playerManager.player.GetComponent<Character>().movementSM.currentState.attackAction;
            }
            else
            {
                attackAction = null;
            }
        }
        else
        {
            playerManager = PlayerManager.Instance;
        }

        //if weaponslot is NOT empty AND the equipped weapon is a ranged weapon THEN u can shoot
        if (InventoryManager.instance.equippedWeaponSlot.IsEmpty())
        {
            return;
        }
        else if (!InventoryManager.instance.equippedWeaponSlot.item.CompareTag("Ranged"))
        {
            return;
        }
        else
        {
            attackAction = null;

            if (attackAction != null && attackAction.triggered)
            {
                shootInput?.Invoke();
            }
        }

        if (weapon.currentAmmo <= 0)
        {
            reloadEvent?.Invoke();
        }
    }
}
