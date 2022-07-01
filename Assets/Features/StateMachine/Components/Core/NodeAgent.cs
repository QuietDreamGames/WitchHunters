using Unity.Entities;

namespace Features.StateMachine.Components.Core
{
    public struct NodeAgent : IComponentData 
    {
        public readonly int NodeCount;
        public int CurrentNodeIndex;
        
        public NodeResult LastResult;
 
        public NodeAgent(int nodeCount) 
        {
            NodeCount = nodeCount;
            CurrentNodeIndex = 0;
            LastResult = NodeResult.Success;
        }
    }
}