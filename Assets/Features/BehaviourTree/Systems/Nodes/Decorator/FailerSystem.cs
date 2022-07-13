using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Decorator;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Decorator;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(FailerSystem.ExecuteNodesJob<Failer, FailerSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Decorator
{
	[UpdateInGroup(typeof(DecoratorNodeSystemGroup))]
    public partial class FailerSystem : NodeExecutorSystem<Failer, FailerSystem.Processor>
	{
		protected override FailerSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new FailerSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Failer>
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
				in Entity failerEntity,
				ref Failer failerComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				Runner(in failerEntity);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity failerEntity,
				ref Failer failerComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				(ok, child) = GetChild(in failerEntity);
				if (!ok)
				{
					return NodeResult.Failed;
				}
		
				var result = GetResult(in child);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					return NodeResult.Failed;
				}

				return NodeResult.Running;
			}

			private void Runner(in Entity inverterEntity)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != inverterEntity)
							continue;

						if (node.IsExec == false)
						{
							node.IsExec = true;
							node.Started = false;
							node.Result = NodeResult.Running;
						}

						allNodes[i] = node;
					}
				}
			}

			private NodeResult GetResult(in NodeComponent child)
			{
				return child.Result;
			}
			
			private (bool, NodeComponent) GetChild(in Entity repeaterEntity)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);
					
					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
					
						if (node.AgentEntity != repeaterEntity)
							continue;
						
						return (true, node);
					}
				}

				return (false, default);
			}
		}
	}
}