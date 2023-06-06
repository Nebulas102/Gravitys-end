using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree : MonoBehaviour
    {
        // The root node
        private Node _root = null;
        // Previous node state
        protected NodeState previousState;

        private void Update()
        {
            // If there is a root, evaluate
            if (_root != null)
                _root.Evaluate();
        }
        
        public virtual void SetTree()
        {
            _root = SetupTree();
        }

        protected abstract Node SetupTree();
    }
}
