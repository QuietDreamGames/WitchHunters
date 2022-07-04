using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Services.Core
{
    public struct TreeNode
    {
        public INode Node;
        public TreeNode[] Children;

        public TreeNode(INode node)
        {
            Node = node;
            Children = default;
        }

        public TreeNode(INode node, TreeNode child)
        {
            Node = node;
            Children = new[] { child };
        }
        
        public TreeNode(INode node, TreeNode[] children)
        {
            Node = node;
            Children = children;
        }
    }
}