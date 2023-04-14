using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


    public class RegularShooting : BossAbility
    {
        [Header("Bullet")]
        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private float bulletInterval;

        [Header("Spray logic")]
        [SerializeField]
        private float sprayAmountBullets;
        [SerializeField]
        private float sprayInterval;

        private GameObject boss;
        private GameObject player;

        private void Start()
        {
            boss = BossManager.instance.boss;
            player = PlayerManager.instance.player;
        }

        public override IEnumerator UseBossAbility()
        {
            yield return StartCoroutine(Spray());
        }

        private IEnumerator Spray()
        {
            for (int i = 0; i < sprayAmountBullets; i++)
            {
                Shoot();
                yield return new WaitForSeconds(bulletInterval);
            }

            yield return new WaitForSeconds(sprayInterval);
        }

        private void Shoot()
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
