using Unity.Entities;

namespace Features.StateMachine.Components.Core
{
    public struct NodeComponent : IComponentData 
    {
        public readonly Entity AgentEntity;
        public readonly int ActionIndex;
 
        public NodeResult Result;
     
        public bool IsExec;
        public bool Started;
 
        public NodeComponent(Entity agentEntity, int actionIndex) : this()
        {
            AgentEntity = agentEntity;
            ActionIndex = actionIndex;
        }
    }
}
