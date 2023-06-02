using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.Tasks;
using Controllers;
using UnityEngine;

public class RangedEnemyBT : BTree
{
    private EnemyController enemyController;

    protected override Node SetupTree()
    {
        enemyController = GetComponent<EnemyController>();

        Node root = new Selector(new List<Node>
        {
            // Retreat sequence
            new Sequence(new List<Node>
            {
                // Is player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // Retreat from player
                new TaskRetreat(enemyController.transform, enemyController.target, enemyController.minDistance, enemyController.obstacleMask, enemyController.agent)
            }),
            // Follow player sequence
            new Sequence(new List<Node>
            {
                // Is Player in range
                new CheckPlayerInRange(enemyController.transform, enemyController.target, enemyController.lookRadius),
                // Follow the player
                new TaskFollow(enemyController.target, enemyController.agent)
            })
        });

        return root;
    }
}
