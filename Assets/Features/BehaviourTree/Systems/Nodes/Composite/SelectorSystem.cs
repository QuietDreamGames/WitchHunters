﻿using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Composite;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Composite;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(SelectorSystem.ExecuteNodesJob<Selector, SelectorSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Composite
{
	[UpdateInGroup(typeof(CompositeNodeSystemGroup))]
    public partial class SelectorSystem : NodeExecutorSystem<Selector, SelectorSystem.Processor>
    {
        protected override SelectorSystem.Processor PrepareProcessor()
		{
			var allNodesQuery = GetEntityQuery(typeof(NodeComponent));
			
			return new SelectorSystem.Processor
			{
				NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
				AllNodesChunks = allNodesQuery.CreateArchetypeChunkArray(Allocator.TempJob),
			};
		}

		[BurstCompile]
		public struct Processor : INodeProcessor<Selector>
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
				in Entity selectorEntity,
				ref Selector selectorComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				selectorComponent.CurrentNodeIndex = 0;
				Runner(in selectorEntity, in selectorComponent);

				return NodeResult.Running;
			}

			public NodeResult Update(in Entity rootEntity,
				in Entity selectorEntity,
				ref Selector selectorComponent,
				int indexOfFirstEntityInQuery,
				int iterIndex)
			{
				NodeComponent child;
				bool ok;

				(ok, child) = GetCurrentChild(in selectorEntity, in selectorComponent);
				if (!ok)
				{
					return NodeResult.Failed;
				}
			
				var result = IterateWithResult(ref selectorComponent, in child);

				if (result is NodeResult.Success or NodeResult.Failed)
				{
					return result;
				}

				Runner(in selectorEntity, in selectorComponent);

				return NodeResult.Running;
			}

			private void Runner(in Entity selectorEntity, in Selector selectorComponent)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);

					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
                					
						if (node.AgentEntity != selectorEntity)
							continue;

						var isCurrentNode = selectorComponent.CurrentNodeIndex == node.ChildIndex;
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

			private NodeResult IterateWithResult(ref Selector selectorComponent, in NodeComponent child)
			{
				if (child.Result == NodeResult.Running)
				{
					return NodeResult.Running;
				}

				if (child.Result == NodeResult.Success)
				{
					return NodeResult.Success;
				}

				++selectorComponent.CurrentNodeIndex;
				if (selectorComponent.CurrentNodeIndex < selectorComponent.NodeCount)
				{
					return NodeResult.Running;
				}

				return NodeResult.Failed;
			}
			
			private (bool, NodeComponent) GetCurrentChild(in Entity selectorEntity, in Selector selectorComponent)
			{
				for (int j = 0; j < AllNodesChunks.Length; j++)
				{
					var batchInChunk = AllNodesChunks[j];
					var allNodes = batchInChunk.GetNativeArray(NodeComponentType);
					
					for (int i = 0; i < allNodes.Length; i++)
					{
						var node = allNodes[i];
					
						if (node.AgentEntity != selectorEntity)
							continue;

						var isCurrentNode = selectorComponent.CurrentNodeIndex == node.ChildIndex;
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