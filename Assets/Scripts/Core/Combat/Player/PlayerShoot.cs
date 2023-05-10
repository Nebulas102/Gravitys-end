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
        weapon = gameObject.GetComponent<RangeWeapon>();
    }

    private void Update()
    {
        if (playerManager != null)
        {
            if (playerManager.player.GetComponent<Character>().movementSM.currentState == playerManager.player.GetComponent<Character>().attacking)
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

        if (attackAction != null && attackAction.triggered)
        {
            shootInput?.Invoke();
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        //     shootInput?.Invoke();
        // }

        if (weapon.currentAmmo <= 0)
        {
            reloadEvent?.Invoke();
        }
    }

    // private void FixedUpdate()
    // {
    //     if (playerManager.GetComponent<Character>() != null)
    //     {
    //         if (playerManager.GetComponent<Character>().movementSM.currentState == playerManager.GetComponent<Character>().attacking)
    //         {
    //             attackAction = playerManager.GetComponent<Character>().movementSM.currentState.attackAction;
    //         }
    //     }    
    // }
}
