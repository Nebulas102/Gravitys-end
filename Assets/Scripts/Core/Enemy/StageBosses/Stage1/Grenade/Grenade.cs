using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using Core.Enemy.StageBosses;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class Grenade : BossAbility
    {
        [Header("Bullet")]
        [SerializeField]
        private GameObject grenade;

        [SerializeField]
        private float grenadeInterval;

        private GameObject _boss;
        private GameObject _player;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;
        }

        public override IEnumerator UseBossAbility()
        {
            ThrowGrenade();

            yield return new WaitForSeconds(grenadeInterval);
        }

        private void ThrowGrenade()
        {
            GameObject newGrenade = Instantiate(grenade, transform.position, Quaternion.identity);

            SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.BossShoots);
        }
    }
}
