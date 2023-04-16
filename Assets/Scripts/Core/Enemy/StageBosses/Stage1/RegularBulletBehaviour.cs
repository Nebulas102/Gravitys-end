using Controllers.Player;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class RegularBulletBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float bulletSpeed = 5f;

        [SerializeField]
        private float bulletDamage = 10f;

        private GameObject _boss;
        private GameObject _player;
        private Vector3 _startPosition;

        private Vector3 _targetPosition;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.instance.player;

            _startPosition = transform.position;
            _targetPosition = _player.transform.position;

            transform.LookAt(new Vector3(_targetPosition.x, _startPosition.y, _targetPosition.z));
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var playerEntity = _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity;

                playerEntity.TakeDamage(bulletDamage, 0.2f);

                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
        }
    }
}
