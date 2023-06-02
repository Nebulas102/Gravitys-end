using System.Collections.Generic;

namespace BehaviorTree
{
    // State of an Node
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        // State of the node
        protected NodeState state;

        // Parent of the node, can be null
        public Node parent;
        // Children of the node
        protected List<Node> children = new List<Node>();

        // Shared data
        // private Dictionary<string, object> _dataContext =
        //         new Dictionary<string, object>();

        // If there are no children, it can't be a parent
        public Node()
        {
            parent = null;
        }

        // If there are children, it can be a parent
        public Node(List<Node> children)
        {
            // Attach the children to the parent
            foreach (Node child in children)
                _Attach(child);
        }

        // Attach children to the node parameter
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        // Each derived-Node can implement its own evaluation function
        // and have an unique role in the behavior tree
        public virtual NodeState Evaluate() => NodeState.FAILURE; // (Update)
        public virtual NodeState FixedEvaluate() => NodeState.FAILURE; // (FixedUpdate)

        // Set shared data
        // public void SetData(string key, object value)
        // {
        //     _dataContext[key] = value;
        // }

        // Get shared data (recursive)
        // public object GetData(string key)
        // {   
        //     // Temp val for object
        //     object val = null;
        //     // If the key is somewhere in the shared data, set the val
        //     if (_dataContext.TryGetValue(key, out val))
        //         return val;

        //     // Set the temp node as parent
        //     Node node = parent;
        //     // If there is a parent
        //     if (node != null)
        //         // Recursive the temp val till we find the node
        //         val = node.GetData(key);
        //     return val;
        // }

        // Clear shared data (recursive)
        // public bool ClearData(string key)
        // {   
        //     // Is the shared data cleared
        //     bool cleared = false;
        //     // If the shared data contains somewhere they key
        //     if (_dataContext.ContainsKey(key))
        //     {   
        //         // Remove the key from the shared data
        //         _dataContext.Remove(key);
        //         // Set cleared true
        //         return true;
        //     }

        //     // Set the temp node as parent
        //     Node node = parent;
        //     // If there is a parent
        //     if (node != null)
        //         // Recursive the cleared bool till we find the node
        //         cleared = node.ClearData(key);
        //     return cleared;
        // }
    }
}
