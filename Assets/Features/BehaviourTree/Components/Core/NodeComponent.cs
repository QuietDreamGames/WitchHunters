using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Core
{
    [BurstCompile]
    public struct NodeComponent : IComponentData
    {
        public readonly Entity RootEntity;
        public readonly Entity AgentEntity;
        
        public readonly int ChildIndex;
        public readonly int DepthIndex;
 
        public NodeResult Result;
     
        public bool IsExec;
        public bool Started;
 
        public NodeComponent(Entity rootEntity, Entity agentEntity, int childIndex, int depthIndex, bool isExec) : this()
        {
            RootEntity = rootEntity;
            AgentEntity = agentEntity;
            
            ChildIndex = childIndex;
            DepthIndex = depthIndex;
            
            Result = NodeResult.Success;

            IsExec = isExec;
            Started = false;
        }
    }
}
