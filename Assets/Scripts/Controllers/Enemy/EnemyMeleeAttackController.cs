using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core.Enemy;
using UnityEngine;

public class EnemyMeleeAttackController : MonoBehaviour
{
    public float attackCooldown = 1f;
    public float attackRange = 1f;

    private EnemyBase _enemyBase;
    private Rigidbody _enemyRB;

    private EnemyController _enemyController;

    private float _nextAttackTime;
    private GameObject _player;
    [SerializeField]
    private EnemyMeleeWeapon _enemyMeleeWeapon;

    private void Start()
    {
        _enemyBase = gameObject.GetComponent<EnemyBase>();
        _enemyController = gameObject.GetComponent<EnemyController>();

        _player = PlayerManager.Instance.player;
    }

    public void PerformMeleeAttack()
    {
        // return if its still on cooldown
        // if (!(Time.time > _nextAttackTime)) return;

        // _enemyMeleeWeapon.AllowHitbox();
        // _enemyMeleeWeapon.MeleeAttack();
        // _enemyMeleeWeapon.DisAllowHitbox();

        // _nextAttackTime = Time.time + attackCooldown;


        // if (_enemyController.agent.velocity != Vector3.zero)
        // {
        //     _enemyController.enemyAnimator.SetTrigger("run");
        // }
        // else
        // {
        _enemyController.enemyAnimator.SetBool("attack1", true);
        
        // }
    }
}
