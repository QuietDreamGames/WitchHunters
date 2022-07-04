using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Services.Core
{
    public struct TreeNode
    {
        public INode Node;
        public TreeNode[] Children;
        
        public string Description;
    }
}