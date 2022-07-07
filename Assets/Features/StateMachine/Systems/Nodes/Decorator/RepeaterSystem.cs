using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Composite;
using Features.StateMachine.Components.Nodes.Decorator;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Core.SystemGroups;
using Features.StateMachine.Systems.Nodes.Decorator;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(RepeaterSystem.ExecuteNodesJob<Repeater, RepeaterSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Decorator
{
	[UpdateInGroup(typeof(DecoratorNodeSystemGroup))]
    public class RepeaterSystem : NodeExecutorSystem<Repeater, RepeaterSystem.Processor>
	{
		protected override RepeaterSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new RepeaterSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Repeater>
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
				in Entity repeaterEntity,
				ref Repeater repeaterComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				Runner(in repeaterEntity);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity repeaterEntity,
				ref Repeater repeaterComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				if (repeaterComponent.Type is RepeaterType.Success or RepeaterType.Failed)
				{
					(ok, child) = GetChild(in repeaterEntity);
					if (!ok)
					{
						return NodeResult.Failed;
					}
			
					var result = GetResult(in repeaterComponent, in child);

					if (result is NodeResult.Success or NodeResult.Failed)
					{
						return result;
					}
				}

				Runner(in repeaterEntity);

				return NodeResult.Running;
			}

			private void Runner(in Entity repeaterEntity)
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

			private NodeResult GetResult(in Repeater repeater, in NodeComponent child)
			{
				if (child.Result == NodeResult.Running)
				{
					return NodeResult.Running;
				}
				
				if (repeater.Type == RepeaterType.Failed)
				{
					if (child.Result == NodeResult.Failed)
					{
						return NodeResult.Failed;
					}
				}
				
				if (repeater.Type == RepeaterType.Success)
				{
					if (child.Result == NodeResult.Success)
					{
						return NodeResult.Success;
					}
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