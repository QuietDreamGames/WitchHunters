using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components.Core
{
    [BurstCompile]
    public struct NodeComponent : IComponentData
    {
        public readonly Entity RootEntity;
        public readonly Entity AgentEntity;
        
        public readonly int ActionIndex;
        public readonly int DepthIndex;
 
        public NodeResult Result;
     
        public bool IsExec;
        public bool Started;
 
        public NodeComponent(Entity rootEntity, Entity agentEntity, int actionIndex, int depthIndex) : this()
        {
            RootEntity = rootEntity;
            AgentEntity = agentEntity;
            
            ActionIndex = actionIndex;
            DepthIndex = depthIndex;
            
            Result = NodeResult.Success;

            IsExec = false;
            Started = false;
        }
    }
}