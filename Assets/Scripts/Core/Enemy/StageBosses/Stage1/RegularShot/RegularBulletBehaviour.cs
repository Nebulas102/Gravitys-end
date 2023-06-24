using Controllers.Player;
using UI.Inventory;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class RegularBulletBehaviour : MonoBehaviour
    {
        private int _minDamage;
        private int _maxDamage;
        private float _speed;
        private ParticleSystem _destructionEffect;

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


            _destructionEffect = Instantiate(_destructionEffect);
        }

        private void Update()
        {
            transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
            _destructionEffect.gameObject.transform.position = transform.root.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var armor = _player.GetComponent<EquipmentSystem>()._equippedArmor;
                _player.GetComponent<PlayerStatsController>().TakeDamage(_minDamage, _maxDamage, armor != null ? armor.GetComponent<Item>().GetArmorModifier() : 0);

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

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }

        public void SetDestructionEffect(ParticleSystem destructionEffect)
        {
            _destructionEffect = destructionEffect;
        }
    }
}
