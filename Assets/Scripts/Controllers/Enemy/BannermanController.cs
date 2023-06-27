using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;


public class BannermanController : MonoBehaviour
{
    [SerializeField]
    [Header("Enemy Settings")]
    private float healEnemyAmountMinimum = 65f;
    [SerializeField]
    private float healEnemyAmountMaximum = 85f;
    [SerializeField]
    [Header("Player Settings")]
    private float healPlayerAmountMinimum = 40f;
    [SerializeField]
    private float healPlayerAmountMaximum = 60f;
    [Header("Healing cooldown")]
    [SerializeField]
    private float healCooldown = 4f;
    [HideInInspector]
    public bool healingAllowed;
    private List<(Coroutine coroutine, EnemyBase enemy)> healingEnemies = new List<(Coroutine, EnemyBase)>();
    private Coroutine healingPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                Coroutine healingEnemy = StartCoroutine(HealEnemy(enemyBase));
                healingEnemies.Add((healingEnemy, enemyBase));
            }
        }
        else if (other.gameObject.CompareTag("Player") && healingAllowed)
        {
            PlayerStatsController playerStats = other.GetComponent<PlayerStatsController>();

            if (playerStats != null)
            {
                healingPlayer = StartCoroutine(HealPlayer(playerStats));
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
                var enemyEntry = healingEnemies.Find(entry => entry.enemy == enemyBase);
                if (enemyEntry.coroutine != null)
                {
                    StopCoroutine(enemyEntry.coroutine);
                    healingEnemies.Remove(enemyEntry);
                }
            }
        }

        else if (other.gameObject.CompareTag("Player") && healingAllowed)
        {
            PlayerStatsController playerStats = other.GetComponent<PlayerStatsController>();

            if (playerStats != null)
            {
                StopCoroutine(healingPlayer);
            }
        }
    }

    private IEnumerator HealEnemy(EnemyBase enemyBase)
    {
        while (true)
        {
            float healEnemyAmount = Random.Range(healEnemyAmountMinimum, healEnemyAmountMaximum);
            enemyBase.EnemyHeal(healEnemyAmount);
            yield return new WaitForSeconds(healCooldown);
        }
    }

    private IEnumerator HealPlayer(PlayerStatsController playerStats)
    {
        while (true)
        {
            float healPlayerAmount = Random.Range(healPlayerAmountMinimum, healPlayerAmountMaximum);
            playerStats.HealPlayer(healPlayerAmount);
            yield return new WaitForSeconds(healCooldown);
        }
    }
}
