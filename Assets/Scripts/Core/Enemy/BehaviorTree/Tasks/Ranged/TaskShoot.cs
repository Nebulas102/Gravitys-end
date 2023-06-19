using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree.Tasks
{
    public class TaskShoot : Node
    {   
        private EnemyRangeAttackController _enemyRangeAttackController;
        private Transform _transform;

        public TaskShoot(EnemyRangeAttackController enemyRangeAttackController, Transform transform)
        {
            _enemyRangeAttackController = enemyRangeAttackController;
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            // Ryan: animations shooting trigger here

            state = NodeState.RUNNING;
            return state;
        }
    }
}
