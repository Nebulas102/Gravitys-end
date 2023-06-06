using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

namespace BehaviorTree.Tasks
{
    public class TaskFollow : Node
    {
        private Transform _target;
        private NavMeshAgent _agent;

        public TaskFollow(Transform target, NavMeshAgent agent)
        {
            _target = target;
            _agent = agent;
        }

        public override NodeState Evaluate()
        {
            bool destinationValid = _agent.SetDestination(_target.position);
            
            if (destinationValid)
                state = NodeState.RUNNING;
            else
                state = NodeState.FAILURE;
            
            return state;
        }
    }
}
