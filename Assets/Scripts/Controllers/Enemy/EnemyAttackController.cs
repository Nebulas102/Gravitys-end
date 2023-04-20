using Controllers.Player;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        private float attackCooldown;

        private EnemyController _enemyController;

        private EnemyBase _enemyBase;

        private float _nextAttackTime;
        private GameObject _player;

        private void Start()
        {
            _enemyBase = gameObject.GetComponent<EnemyBase>();
            _enemyController = gameObject.GetComponent<EnemyController>();

            _player = PlayerManager.Instance.player;
        }

        public void Attack()
        {
            if (Time.time > _nextAttackTime)
            {
                _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity
                    .TakeDamage(_enemyBase.GetDamage(), 0f);
                _nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
