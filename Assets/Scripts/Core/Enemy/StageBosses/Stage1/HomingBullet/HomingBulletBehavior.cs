using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class HomingBulletBehavior : MonoBehaviour
    {
        [SerializeField]
        private float bulletSpeed = 5f;
        [SerializeField]
        private float rotationSpeed = 5f;
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

        private void FixedUpdate()
        {
            // Calculate the direction towards the target
            Vector3 direction = _player.transform.position - transform.position;

            // Calculate the rotation towards the target
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);

            // Move the bullet forward in its local space
            transform.Translate(Vector3.forward * bulletSpeed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision other)
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
