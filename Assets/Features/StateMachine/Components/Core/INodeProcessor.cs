using Unity.Entities;

namespace Features.StateMachine.Components.Core
{
    public interface INodeProcessor<T> 
        where T : struct, INodeComponent 
    {
        void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex);
        NodeResult Start(in Entity agentEntity, ref T nodeComponent, int indexOfFirstEntityInQuery, int iterIndex);
        NodeResult Update(in Entity agentEntity, ref T nodeComponent, int indexOfFirstEntityInQuery, int iterIndex);
    }
}