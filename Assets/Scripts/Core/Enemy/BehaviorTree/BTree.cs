using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree : MonoBehaviour
    {
        // The root node
        private Node _root = null;

        protected virtual void Start()
        {
            // Build the behavior tree on start
            _root = SetupTree();
        }

        private void Update()
        {
            // If there is a root, evaluate
            if (_root != null)
                _root.Evaluate();
        }

        private void FixedUpdate()
        {
            // If there is a root, evaluate
            if (_root != null)
                _root.FixedEvaluate();
        }

        protected abstract Node SetupTree();
    }
}
