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

[assembly: RegisterGenericJobType(typeof(ParallelSystem.ExecuteNodesJob<Parallel, ParallelSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Composite
{
	[UpdateInGroup(typeof(CompositeNodeSystemGroup))]
    public partial class ParallelSystem : NodeExecutorSystem<Parallel, ParallelSystem.Processor>
	{
		protected override ParallelSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new ParallelSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}
		
		[BurstCompile]
		public struct Processor : INodeProcessor<Parallel>
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
				in Entity parallelEntity,
				ref Parallel parallelComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				Runner(in parallelEntity);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity parallelEntity,
				ref Parallel parallelComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				var result = GetResult(in parallelEntity, in parallelComponent);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					StopChildren(in parallelEntity);
				}

				return result;
			}

			private void Runner(in Entity parallelEntity)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != parallelEntity)
							continue;
						
						node.IsExec = true;
						node.Started = false;
						node.Result = NodeResult.Running;

						allNodes[i] = node;
					}
				}
			}

			private void StopChildren(in Entity parallelEntity)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != parallelEntity)
							continue;
						
						node.IsExec = false;
						node.Started = false;

						allNodes[i] = node;
					}
				}
			}
			
			private NodeResult GetResult(in Entity parallelEntity, in Parallel parallelComponent)
			{
				var isAnyChildRunning = false;
				
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != parallelEntity)
							continue;
						
						if (node.Result is NodeResult.Failed)
						{
							if (parallelComponent.Type is ParallelType.Sequence)
							{
								return NodeResult.Failed;
							}
						}
						
						if (node.Result is NodeResult.Success)
						{
							if (parallelComponent.Type is ParallelType.Selector)
							{
								return NodeResult.Success;
							}
						}

						if (node.Result is NodeResult.Running)
						{
							if (isAnyChildRunning == false)
							{
								isAnyChildRunning = true;
							}
						}
					}
				}

				return isAnyChildRunning 
					? NodeResult.Running 
					: NodeResult.Success;
			}
		}
	}
}