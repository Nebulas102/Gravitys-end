using Controllers.Player;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class RegularBulletBehaviour : MonoBehaviour
    {
        private int _minDamage;
        private int _maxDamage;
        private float _speed;

        private GameObject _boss;
        private GameObject _player;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector3 _direction;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;

            _startPosition = transform.position;
            _targetPosition = _player.transform.position;
        }

        private void Update()
        {
            transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _player.GetComponent<PlayerStatsController>().TakeDamage(_minDamage, _maxDamage, 0);

                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
        }

        public void SetDamage(int minDamage, int maxDamage)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }
    }
}
