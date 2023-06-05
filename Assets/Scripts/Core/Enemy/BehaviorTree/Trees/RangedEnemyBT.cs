using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.Tasks;
using Controllers;
using UnityEngine;

public class RangedEnemyBT : BTree
{
    private EnemyController enemyController;
    private EnemyRangeAttackController enemyRangeAttackController;

    protected override Node SetupTree()
    {
        enemyController = GetComponent<EnemyController>();
        enemyRangeAttackController = GetComponent<EnemyRangeAttackController>();

        Node root = new Selector(new List<Node>
        {
            // Retreat sequence
            new Sequence(new List<Node>
            {
                // Is player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // While retreating from the player also shoot
                new Parallel(new List<Node>
                {
                    // Retreat from player
                    new TaskRetreat(enemyController.transform, enemyController.target, enemyController.retreatDistance, enemyController.obstacleMask, enemyController.agent),
                    // Is player in attack range
                    new CheckPlayerInAttackRange(enemyRangeAttackController.attackRange, enemyController.target, enemyController.transform),
                    // Shoot at player
                    new TaskShoot(enemyRangeAttackController)
                })
            }),
            // Follow player sequence
            new Sequence(new List<Node>
            {
                // Is Player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // While following the player shoot also
                new Parallel(new List<Node>
                {
                    // Follow the player
                    new TaskFollow(enemyController.target, enemyController.agent),
                    // Is player in attack range
                    new CheckPlayerInAttackRange(enemyRangeAttackController.attackRange, enemyController.target, enemyController.transform),
                    // Shoot at player
                    new TaskShoot(enemyRangeAttackController)
                })
            }),
            // Attack player sequence
            new Sequence(new List<Node>
            {
                // Is Player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // Is player in attack range
                new CheckPlayerInAttackRange(enemyRangeAttackController.attackRange, enemyController.target, enemyController.transform),
                // Shoot at player
                new TaskShoot(enemyRangeAttackController)
            })
        });

        return root;
    }
}
