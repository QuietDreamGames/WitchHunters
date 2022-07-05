using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Decorator;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Nodes.Decorator;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(SucceederSystem.ExecuteNodesJob<Succeeder, SucceederSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Decorator
{
    public partial class SucceederSystem : NodeExecutorSystem<Succeeder, SucceederSystem.Processor>
	{
		protected override SucceederSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new SucceederSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Succeeder>
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
				in Entity succeederEntity,
				ref Succeeder succeederComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				Runner(in succeederEntity);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity succeederEntity,
				ref Succeeder succeederComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				(ok, child) = GetChild(in succeederEntity);
				if (!ok)
				{
					return NodeResult.Failed;
				}
		
				var result = GetResult(in child);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					return NodeResult.Success;
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