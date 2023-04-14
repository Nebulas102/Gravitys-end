using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockShot : BossAbility
{
        [Header("Bullet")]
        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private float radius = 3f;

        [Header("Clockshot")]
        [SerializeField]
        private float clockShotInterval;

        [SerializeField]
        private Transform bossRoom;

        private GameObject boss;
        private GameObject player;

        private int amountOfBullets;
        private Quaternion bulletRot;

        private void Start()
        {
            boss = BossManager.instance.boss;
            player = PlayerManager.instance.player;

            amountOfBullets = 360 / 30;
        }

        public override IEnumerator UseBossAbility()
        {
            yield return StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            Debug.Log("clockshot");

            Vector3 spawnPos = bossRoom.TransformPoint(boss.transform.localPosition);

            float angleIncrement = 360f / amountOfBullets;

            for (int i = 0; i < amountOfBullets; i++)
            {
               float rotationAngle = i * angleIncrement;

                float x = radius * Mathf.Cos(rotationAngle * Mathf.Deg2Rad);
                float z = radius * Mathf.Sin(rotationAngle * Mathf.Deg2Rad);

                Vector3 bulletPosition = transform.position + new Vector3(x, 0, z);

                GameObject _bullet = Instantiate(bullet, bulletPosition, Quaternion.identity);

                _bullet.transform.forward = _bullet.transform.position - transform.position;
            }

            yield return null;
        }
}
