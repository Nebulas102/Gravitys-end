using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;

public class EnemyRangeAttackController : MonoBehaviour
{
    public float attackRange;
    public GameObject rangeWeaponObject;

    [HideInInspector]
    public float playerDistance;

    private EnemyBase enemyBase;
    private EnemyController enemyController;
    private GameObject player;
    private EnemyRangeWeapon rangeWeapon;

    private void Start()
    {
        enemyBase = gameObject.GetComponent<EnemyBase>();
        enemyController = gameObject.GetComponent<EnemyController>();
        player = PlayerManager.Instance.player;
        rangeWeapon = rangeWeaponObject.GetComponent<EnemyRangeWeapon>();

        rangeWeapon.SetEnemy(transform);
    }

    public void EnemyShoot()
    {
        rangeWeapon.PerformShot();
        
        if (enemyController.agent.velocity != Vector3.zero)
        {
            enemyController.enemyAnimator.SetTrigger("run_shoot");
        }
        else
        {
            enemyController.enemyAnimator.SetTrigger("shoot");
        }
    }
}
