using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core.Enemy;
using UnityEngine;

public class EnemyMeleeAttackController : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown;

    private EnemyBase _enemyBase;

    private EnemyController _enemyController;

    private float _nextAttackTime;
    private GameObject _player;

    private void Start()
    {
        _enemyBase = gameObject.GetComponent<EnemyBase>();
        _enemyController = gameObject.GetComponent<EnemyController>();

        _player = PlayerManager.Instance.player;
    }

    public void Attack()
    {
        if (!(Time.time > _nextAttackTime)) return;

        PerformAttack();
        _nextAttackTime = Time.time + attackCooldown;
    }

    private void PerformAttack()
    {
        //Do animation for attack
    }
}
