using Controllers.Player;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        private float attackCooldown;

        private EnemyController _enemyController;

        private EnemyStatsController _enemyStatsController;

        private float _nextAttackTime;
        private GameObject _player;

        private void Start()
        {
            _enemyStatsController = gameObject.GetComponent<EnemyStatsController>();
            _enemyController = gameObject.GetComponent<EnemyController>();

            _player = PlayerManager.instance.player;
        }

        public void Attack()
        {
            if (Time.time > _nextAttackTime)
            {
                _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity
                    .TakeDamage(_enemyStatsController.GetEntity().GetDamage(), 1f);
                _nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
