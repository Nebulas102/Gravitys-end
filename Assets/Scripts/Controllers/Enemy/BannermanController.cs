using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;


public class BannermanController : MonoBehaviour
{
    [SerializeField]
    private float healEnemyAmount = 10f;
    [SerializeField]
    private float healPlayerAmount = 5f;
    [SerializeField]
    private float healCooldown = 4f;
    public bool healingAllowed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                StartCoroutine(HealEnemy(enemyBase));
            }
        }
        else if (other.gameObject.CompareTag("Player") && healingAllowed)
        {
            PlayerStatsController playerStats = other.GetComponent<PlayerStatsController>();

            if (playerStats != null)
            {
                StartCoroutine(HealPlayer(playerStats));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                StopCoroutine(HealEnemy(enemyBase));
            }
        }
    }

    private IEnumerator HealEnemy(EnemyBase enemyBase)
    {
        while (true)
        {
            enemyBase.health += healEnemyAmount;
            yield return new WaitForSeconds(healCooldown);
        }
    }

    private IEnumerator HealPlayer(PlayerStatsController playerStats)
    {
        while (true)
        {
            playerStats.HealPlayer(healPlayerAmount);
            yield return new WaitForSeconds(healCooldown);
        }
    }
}
