using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // This script makes sure that the game always knows which object is the Player
    // This is used for enemies, so they always know who the player is and you dont have to find the player object each time a new enemy spawns
    public GameObject player;

    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if(instance != null)
        return;

        instance = this;
    }

    #endregion
}
