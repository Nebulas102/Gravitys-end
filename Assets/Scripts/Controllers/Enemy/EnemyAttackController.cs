using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Assets.Scripts.Controllers.Player;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        private float attackCooldown;

        private EnemyStatsController enemyStatsController;
        private EnemyController enemyController;
        private GameObject player;

        private float nextAttackTime = 0f;

        private void Start()
        {
            enemyStatsController = gameObject.GetComponent<EnemyStatsController>();
            enemyController = gameObject.GetComponent<EnemyController>();

            player = PlayerManager.instance.player;
        }

        public void Attack()
        {
            if (Time.time > nextAttackTime)
            {   
                player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(enemyStatsController.GetEntity().GetDamage(), 1f);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
