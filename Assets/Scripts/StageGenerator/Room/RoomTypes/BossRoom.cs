using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField]
    private GameObject roomBossSpawnPoint;

    private GameObject roomBoss;
    private bool playerEnterBossFight = false;
    
    private void Start()
    {
        roomBoss = GameObject.FindWithTag("Boss");
    }

    private void Update()
    {
        if (playerEnterBossFight)
        {
            roomBoss.GetComponent<Boss>().SetStartFight(true);
        }
    }

    public bool GetPlayerEnterBossFight()
    {
        return playerEnterBossFight;
    }

    public void SetPlayerEnterBossFight(bool bossFight)
    {
        playerEnterBossFight = bossFight;
    }

    public GameObject GetRoomBoss()
    {
        return roomBoss;
    }
}
