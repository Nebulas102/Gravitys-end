using Controllers.Player;
using Core.Enemy;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        private float attackCooldown;

        private EnemyBase _enemyBase;

        private EnemyController _enemyController;

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
            if (!(Time.time > _nextAttackTime)) return;

            // _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity
            //     .TakeDamage(_enemyBase.GetDamage(), 0f);
            _nextAttackTime = Time.time + attackCooldown;
        }
    }
}
