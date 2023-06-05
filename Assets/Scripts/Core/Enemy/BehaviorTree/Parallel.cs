using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Parallel : Node
    {
        public Parallel() : base() { }
        public Parallel(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool allChildrenSucceeded = true;
            bool anyChildRunning = false;

            foreach (Node child in children)
            {
                NodeState childState = child.Evaluate();

                if (childState == NodeState.FAILURE)
                {
                    allChildrenSucceeded = false;
                }
                else if (childState == NodeState.RUNNING)
                {
                    anyChildRunning = true;
                }
            }

            if (allChildrenSucceeded)
            {
                state = NodeState.SUCCESS;
            }
            else if (anyChildRunning)
            {
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }

        public override NodeState FixedEvaluate()
        {
            bool allChildrenSucceeded = true;
            bool anyChildRunning = false;

            foreach (Node child in children)
            {
                NodeState childState = child.Evaluate();

                if (childState == NodeState.FAILURE)
                {
                    allChildrenSucceeded = false;
                }
                else if (childState == NodeState.RUNNING)
                {
                    anyChildRunning = true;
                }
            }

            if (allChildrenSucceeded)
            {
                state = NodeState.SUCCESS;
            }
            else if (anyChildRunning)
            {
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}