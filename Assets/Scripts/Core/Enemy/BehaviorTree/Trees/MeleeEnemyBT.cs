using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.Tasks;
using Controllers;
using UnityEngine;

public class MeleeEnemyBT : BTree
{
    private EnemyController enemyController;

    protected override Node SetupTree()
    {
        enemyController = GetComponent<EnemyController>();

        Node root = new Selector(new List<Node>
        {
            // Follow player sequence
            new Sequence(new List<Node>
            {
                // Is Player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // Follow the player
                new TaskFollow(enemyController.target, enemyController.agent),
                // While in attack range, attack
                new Parallel(new List<Node>
                {
                    // Is player in attack range
                    new CheckPlayerInAttackRange(),
                    // perform melee attack at player
                    new TaskMeleeAttack()
                })
            }),
        });

        return root;
    }
}
