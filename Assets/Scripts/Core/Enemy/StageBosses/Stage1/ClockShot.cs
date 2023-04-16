using System.Collections;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
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

        private int _amountOfBullets;

        private GameObject _boss;
        private Quaternion _bulletRot;
        private GameObject _player;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.instance.player;

            _amountOfBullets = 360 / 30;
        }

        public override IEnumerator UseBossAbility()
        {
            yield return StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            var spawnPos = bossRoom.TransformPoint(_boss.transform.localPosition);

            var angleIncrement = 360f / _amountOfBullets;

            for (var i = 0; i < _amountOfBullets; i++)
            {
                var rotationAngle = i * angleIncrement;

                var x = radius * Mathf.Cos(rotationAngle * Mathf.Deg2Rad);
                var z = radius * Mathf.Sin(rotationAngle * Mathf.Deg2Rad);

                var bulletPosition = transform.position + new Vector3(x, 0, z);

                var _bullet = Instantiate(bullet, bulletPosition, Quaternion.identity);

                _bullet.transform.forward = _bullet.transform.position - transform.position;
            }

            yield return new WaitForSeconds(clockShotInterval);
        }
    }
}
