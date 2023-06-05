using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree.Tasks
{
    public class TaskShoot : Node
    {   
        private Transform _target;

        public override NodeState Evaluate()
        {
            playerDistance = Vector3.Distance(player.transform.position, transform.position);

            if (playerDistance > enemyController.lookRadius)
            {
                allowShooting = false;
            }
            else
            {
                allowShooting = true;
            }
        }
    }
}
