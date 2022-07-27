using Features.BehaviourTree.Components.Core;

namespace Features.BehaviourTree.Services.Core
{
    public struct TreeNode
    {
        public INode Node;
        public TreeNode[] Children;
        
        public string Description;
    }
}