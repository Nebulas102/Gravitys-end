using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
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
        playerManager = PlayerManager.Instance;

        // if (playerManager)
        // {

        // }

        // if (playerManager.GetComponent<Character>().movementSM != null)
        // {
        //     Debug.Log("Not null");
        // }

        attackAction = playerManager.GetComponent<Character>().movementSM.currentState.attackAction;
        weapon = gameObject.GetComponent<RangeWeapon>();
    }

    private void Update()
    {
        // if (playerManager.GetComponent<Character>() != null)
        // {
        //     attackAction = playerManager.GetComponent<Character>().movementSM.currentState.attackAction;
        // }

        if (attackAction.triggered && attackAction != null)
        {
            shootInput?.Invoke();
        }

        if (weapon.currentAmmo < 0)
        {
            reloadEvent?.Invoke();
        }
    }
}
