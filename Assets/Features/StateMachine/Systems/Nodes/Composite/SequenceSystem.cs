using Features.StateMachine.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Composite;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Core.SystemGroups;
using Features.StateMachine.Systems.Nodes.Composite;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(SequenceSystem.ExecuteNodesJob<Sequence, SequenceSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Composite
{
	[UpdateInGroup(typeof(CompositeNodeSystemGroup))]
	public partial class SequenceSystem : NodeExecutorSystem<Sequence, SequenceSystem.Processor>
	{
		protected override SequenceSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new SequenceSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Sequence>
		{
			[NativeDisableContainerSafetyRestriction]
			public ComponentTypeHandle<NodeComponent> NodeComponentType;
			
			[ReadOnly]
			[DeallocateOnJobCompletion]
			public NativeArray<ArchetypeChunk> AllNodesChunks;

			public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
	            
            }

			public NodeResult Start(in Entity rootEntity,
				in Entity sequenceEntity,
				ref Sequence sequenceComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				sequenceComponent.CurrentNodeIndex = 0;
				Runner(in sequenceEntity, in sequenceComponent);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity sequenceEntity,
				ref Sequence sequenceComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				(ok, child) = GetCurrentChild(in sequenceEntity, in sequenceComponent);
				if (!ok)
				{
					return NodeResult.Failed;
				}
			
				var result = IterateWithResult(ref sequenceComponent, in child);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					return result;
				}

				Runner(in sequenceEntity, in sequenceComponent);

				return NodeResult.Running;
			}

			private void Runner(in Entity sequenceEntity, in Sequence sequenceComponent)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != sequenceEntity)
							continue;

						var isCurrentNode = sequenceComponent.CurrentNodeIndex == node.ChildIndex;
						if (isCurrentNode)
						{
							if (node.IsExec == false)
							{
								node.IsExec = true;
								node.Started = false;
								node.Result = NodeResult.Running;
							}
						}
						else
						{
							node.IsExec = false;
							node.Started = false;
						}
						
						allNodes[i] = node;
					}
				}
			}

			private NodeResult IterateWithResult(ref Sequence sequenceComponent, in NodeComponent child)
			{
				if (child.Result == NodeResult.Running)
				{
					return NodeResult.Running;
				}

				if (child.Result == NodeResult.Failed)
				{
					return NodeResult.Failed;
				}

				++sequenceComponent.CurrentNodeIndex;
				if (sequenceComponent.CurrentNodeIndex < sequenceComponent.NodeCount)
				{
					return NodeResult.Running;
				}

				return NodeResult.Success;
			}
			
			private (bool, NodeComponent) GetCurrentChild(in Entity sequenceEntity, in Sequence sequenceComponent)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);
					
					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
					
						if (node.AgentEntity != sequenceEntity)
							continue;

						var isCurrentNode = sequenceComponent.CurrentNodeIndex == node.ChildIndex;
						if (isCurrentNode == false)
							continue;

						return (true, node);
					}
				}

				return (false, default);
			}
		}
	}
}