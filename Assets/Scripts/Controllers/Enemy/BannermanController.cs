using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core.Enemy;
using UnityEngine;


public class BannermanController : MonoBehaviour
{
    public GameObject bannerSphere;
    public int healAmount = 10;
    public bool healingAllowed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && healingAllowed)
        {
            EnemyBase enemyBase = other.GetComponent<EnemyBase>();

            if (enemyBase != null)
            {
                Debug.Log("Healing");
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
            yield return new WaitForSeconds(4f);
        }
    }
}
