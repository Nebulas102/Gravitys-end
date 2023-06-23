using Controllers.Player;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class ClockBulletBehaviour : MonoBehaviour
    {
        private int _minDamage;
        private int _maxDamage;
        private float _speed;
        private ParticleSystem _destructionEffect;

        private GameObject _boss;
        private GameObject _player;
        private Vector3 _startPosition;

        private Vector3 _targetPosition;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;

            _startPosition = transform.position;
            _targetPosition = _player.transform.position;

            _destructionEffect = Instantiate(_destructionEffect);
        }

        private void Update()
        {
            transform.root.Translate(Vector3.forward * _speed * Time.deltaTime);
            _destructionEffect.gameObject.transform.position = transform.root.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _player.GetComponent<PlayerStatsController>().TakeDamage(_minDamage, _maxDamage, 0);

                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
            {
                _destructionEffect.Play();
                Destroy(gameObject);
            }
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

        public void SetDestructionEffect(ParticleSystem destructionEffect)
        {
            _destructionEffect = destructionEffect;
        }
    }
}
