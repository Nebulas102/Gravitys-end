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

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;

            _startPosition = transform.position;

            transform.LookAt(_player.transform.position);
        }

        private void Update()
        {
             Vector3 targetPosition = _player.transform.position;
            targetPosition.y = transform.position.y; // Preserve current Y position

            // Calculate the rotation towards the target
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

            // Smoothly rotate towards the target with curve modifier
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Move forward in the direction of the current rotation
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
           if (other.gameObject.CompareTag("Player"))
            {
                _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(bulletDamage, bulletDamage, 0);

                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
        }
    }
}
