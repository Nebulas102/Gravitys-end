using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class HomingBulletBehavior : MonoBehaviour
    {
        private int _minDamage;
        private int _maxDamage;
        private float _speed;
        private float _rotationSpeed;

        private GameObject _boss;
        private GameObject _player;
        private Vector3 _startPosition;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;

            _startPosition = transform.position;

            transform.root.LookAt(_player.transform.position);
        }

        private void Update()
        {
            Vector3 targetPosition = _player.transform.position;
            targetPosition.y = transform.root.position.y; // Preserve current Y position

            // Calculate the rotation towards the target
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.root.position);

            // Smoothly rotate towards the target with curve modifier
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, rotation, _rotationSpeed * Time.deltaTime);

            // Move forward in the direction of the current rotation
            transform.root.Translate(Vector3.forward * _speed * Time.deltaTime, Space.World);
        }

        private void OnCollisionEnter(Collision other)
        {
           if (other.gameObject.CompareTag("Player"))
            {
                _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(_minDamage, _maxDamage, 0);

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

        public void SetRotationSpeed(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }
    }
}
