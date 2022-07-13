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

[assembly: RegisterGenericJobType(typeof(InverterSystem.ExecuteNodesJob<Inverter, InverterSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Decorator
{
	[UpdateInGroup(typeof(DecoratorNodeSystemGroup))]
    public class InverterSystem : NodeExecutorSystem<Inverter, InverterSystem.Processor>
	{
		protected override InverterSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new InverterSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Inverter>
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
				in Entity inverterEntity,
				ref Inverter inverterComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				Runner(in inverterEntity);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity inverterEntity,
				ref Inverter inverterComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				(ok, child) = GetChild(in inverterEntity);
				if (!ok)
				{
					return NodeResult.Failed;
				}
		
				var result = GetResult(in child);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					return result;
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
				if (child.Result == NodeResult.Running)
				{
					return NodeResult.Running;
				}
				
				if (child.Result == NodeResult.Success)
				{
					return NodeResult.Failed;
				}
				
				if (child.Result == NodeResult.Failed)
				{
					return NodeResult.Success;
				}
				
				return NodeResult.Running;
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