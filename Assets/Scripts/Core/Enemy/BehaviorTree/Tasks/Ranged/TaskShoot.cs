using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree.Tasks
{
    public class TaskShoot : Node
    {   
        private EnemyRangeAttackController _enemyRangeAttackController;

        public TaskShoot(EnemyRangeAttackController enemyRangeAttackController)
        {
            _enemyRangeAttackController = enemyRangeAttackController;
        }

        public override NodeState Evaluate()
        {
            _enemyRangeAttackController.GetRangeWeapon().PerformShot();
            state = NodeState.RUNNING;
            return state;
        }
    }
}
