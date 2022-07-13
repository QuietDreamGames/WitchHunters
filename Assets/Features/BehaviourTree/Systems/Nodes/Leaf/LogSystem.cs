using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Services.Core;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(LogSystem.ExecuteNodesJob<Log, LogSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
	[UpdateInGroup(typeof(LeafNodeSystemGroup))]
    public partial class LogSystem : NodeExecutorSystem<Log, LogSystem.Processor>
    {
        protected override LogSystem.Processor PrepareProcessor()
        {
            return new LogSystem.Processor
            {

            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<Log>
        {
            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {

            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref Log nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                JobLogger.Log(nodeComponent.Message, nodeComponent.Type);
                
                return NodeResult.Success;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref Log nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                return NodeResult.Success;
            }
        }
    }
}