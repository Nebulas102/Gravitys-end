using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Player;
using ScriptableObjects;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private GameObject boss;

    void Start()
    {
        boss = BossManager.instance.boss;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            // Entity playerEntity = other.gameObject.GetComponent<PlayerStatsController>().GetPlayerObject().entity;

            //take regularshooting damage and not boss damage
            // playerEntity.TakeDamage(boss.GetComponent<Boss>().GetDamage(), 0.2f);

            Destroy(other.gameObject);
        } 
        else if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
