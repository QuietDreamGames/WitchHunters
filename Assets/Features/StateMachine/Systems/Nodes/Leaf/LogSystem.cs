using System;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Services.Core;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Core.SystemGroups;
using Features.StateMachine.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using LogType = Features.StateMachine.Components.Nodes.Leaf.LogType;

[assembly: RegisterGenericJobType(typeof(LogSystem.ExecuteNodesJob<Log, LogSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Leaf
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