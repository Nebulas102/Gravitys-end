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
        private float bulletSpeed;
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

        public override void UseBossAbility()
        {
            StartCoroutine(Spray());
        }

        private IEnumerator Spray()
        {
            for (int i = 0; i < sprayAmountBullets; i++)
            {
                StartCoroutine(Shoot());
            }

            yield return new WaitForSeconds(sprayInterval);
        }

        private IEnumerator Shoot()
        {
            GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            
            _bullet.transform.position = Vector3.MoveTowards(_bullet.transform.position, player.transform.position, bulletSpeed * Time.deltaTime);

            yield return new WaitForSeconds(bulletInterval);
        }
    }
