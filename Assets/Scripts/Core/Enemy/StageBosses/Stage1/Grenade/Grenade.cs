using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using Core.Enemy.StageBosses;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class Grenade : BossAbility
    {
        [Header("Grenade")]
        [SerializeField]
        private GameObject grenade;
        [SerializeField]
        private float throwDuration;
        [SerializeField]
        private float curveHeight;
        [SerializeField]
        private int minDamage;
        [SerializeField]
        private int maxDamage;
        [SerializeField]
        private float grenadeInterval;

        [Header("Decal")]
        [SerializeField]
        private GameObject decal;
        [SerializeField]
        private float decalRadius;

        private GameObject _boss;
        private GameObject _player;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;

            GrenadeBehavior grenadeBehavior = grenade.GetComponentInChildren<GrenadeBehavior>();

            grenadeBehavior.SetDamage(minDamage, maxDamage);
            grenadeBehavior.SetThrowDuration(throwDuration);
            grenadeBehavior.SetCurveHeight(curveHeight);
        }

        public override IEnumerator UseBossAbility()
        {
            ThrowGrenade();

            yield return new WaitForSeconds(grenadeInterval);
        }

        private void ThrowGrenade()
        {
            GameObject newGrenade = Instantiate(grenade, transform.position, Quaternion.identity);

            GameObject newDecal = Instantiate(decal);
            newDecal.GetComponent<Decal>().SetRadius(decalRadius);
            newGrenade.GetComponent<GrenadeBehavior>().SetDecal(newDecal);

            SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.BossShoots);
        }
    }
}
