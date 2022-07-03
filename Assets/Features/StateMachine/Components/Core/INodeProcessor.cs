using Unity.Collections;
using Unity.Entities;

namespace Features.StateMachine.Components.Core
{
    public interface INodeProcessor<T> 
        where T : struct, IComponentData, INode
    {
        void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex);
        NodeResult Start(in Entity rootEntity,
            in Entity agentEntity,
            ref T nodeComponent,
            int indexOfFirstEntityInQuery,
            int iterIndex);
        NodeResult Update(in Entity rootEntity,
            in Entity agentEntity,
            ref T nodeComponent,
            int indexOfFirstEntityInQuery,
            int iterIndex);
    }
}