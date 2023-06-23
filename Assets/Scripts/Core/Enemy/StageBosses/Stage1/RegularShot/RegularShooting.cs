using System.Collections;
using Core.Audio;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class RegularShooting : BossAbility
    {
        [Header("Bullet")]
        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private float bulletSpeed;
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

        [Header("Effect")]
        [SerializeField]
        private ParticleSystem destructionEffect;

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

            Vector3 bulletOutputWorldPos = newBullet.transform.TransformPoint(Vector3.zero);
            RegularBulletBehaviour newRegularBulletBehaviour = newBullet.GetComponentInChildren<RegularBulletBehaviour>();

            Vector3 bulletDirection = (_player.transform.position - bulletOutputWorldPos).normalized;

            bulletDirection.y = 0f;

            newRegularBulletBehaviour.SetDirection(bulletDirection);
            newRegularBulletBehaviour.SetDamage(minDamage, maxDamage);
            newRegularBulletBehaviour.SetSpeed(bulletSpeed);
            newRegularBulletBehaviour.SetDestructionEffect(destructionEffect);

            newBullet.transform.LookAt(_player.transform.position);
            newBullet.transform.rotation = new Quaternion(0, newBullet.transform.rotation.y, 0, newBullet.transform.root.rotation.w);

            SoundEffectsManager.instance.PlaySoundEffect(SoundEffect.BOSS_SHOOTS);
        }
    }
}
