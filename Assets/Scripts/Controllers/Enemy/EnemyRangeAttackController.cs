using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;

public class EnemyRangeAttackController : MonoBehaviour
{
    public GameObject rangeWeaponObject;
    
    [HideInInspector]
    public bool allowShooting;
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
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);

        if(playerDistance > enemyController.lookRadius)
        {
            allowShooting = false;
        }
        else
        {
            allowShooting = true;
        }
    }
}
