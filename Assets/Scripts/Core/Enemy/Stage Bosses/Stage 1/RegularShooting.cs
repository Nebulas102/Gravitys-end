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

        private void Start()
        {
            boss = GameObject.FindWithTag("Boss");
        }

        public override void UseBossAbility()
        {
            StartCoroutine(Spray());
        }

        private IEnumerator Spray()
        {
            for (int i = 0; i < sprayAmountBullets; i++)
            {
                StartCoroutine(Shoot(i));
            }

            yield return new WaitForSeconds(sprayInterval);
        }

        private IEnumerator Shoot(int i)
        {
            Debug.Log("Regular Shot " + i);

            yield return new WaitForSeconds(bulletInterval);
        }
    }
