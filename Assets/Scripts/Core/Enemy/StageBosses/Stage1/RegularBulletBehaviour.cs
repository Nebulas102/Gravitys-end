using Controllers.Player;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class RegularBulletBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float bulletSpeed = 5f;

        [SerializeField]
        private int bulletDamage = 10;

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

            transform.LookAt(new Vector3(_targetPosition.x, _startPosition.y, _targetPosition.z), transform.forward);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }

private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(bulletDamage, bulletDamage, 0);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
    }
    }
}
