using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core.Enemy;
using UnityEngine;


public class BannermanController : MonoBehaviour
{
    public GameObject bannerSphere;
    [SerializeField]
    private int healAmount = 10;
    [SerializeField]
    private float healCooldown = 4;
    public bool healingAllowed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                StartCoroutine(Healing(enemyBase));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                StopCoroutine(Healing(enemyBase));
            }
        }
    }

    private IEnumerator Healing(EnemyBase enemyBase)
    {
        while (true)
        {
            enemyBase.health += healAmount;
            yield return new WaitForSeconds(healCooldown);
        }
    }
}
