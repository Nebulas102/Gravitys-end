using System.Collections;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class HomingShot : BossAbility
    {
        [Header("Bullet")]
        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletRotationSpeed;
        [SerializeField]
        private int minDamage;
        [SerializeField]
        private int maxDamage;
        [SerializeField]
        private float bulletInterval;

        [Header("Spray logic")]
        [SerializeField]
        private float sprayAmountBullets;

        [SerializeField]
        private float sprayInterval;

        private GameObject _boss;
        private GameObject _player;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;
        }

        public override IEnumerator UseBossAbility()
        {
            yield return StartCoroutine(Spray());
        }

        private IEnumerator Spray()
        {
            for (var i = 0; i < sprayAmountBullets; i++)
            {
                Shoot();
                yield return new WaitForSeconds(bulletInterval);
            }

            yield return new WaitForSeconds(sprayInterval);
        }

        private void Shoot()
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            HomingBulletBehavior newHomingBulletBehavior = newBullet.GetComponentInChildren<HomingBulletBehavior>();

            newHomingBulletBehavior.SetDamage(minDamage, maxDamage);
            newHomingBulletBehavior.SetSpeed(bulletSpeed);
            newHomingBulletBehavior.SetRotationSpeed(bulletRotationSpeed);

            SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.BossShoots);
        }
    }
}
