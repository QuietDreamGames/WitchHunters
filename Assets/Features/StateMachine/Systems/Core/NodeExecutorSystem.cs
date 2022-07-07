using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Features.StateMachine.Systems.Core
{
    public abstract partial class NodeExecutorSystem<TNodeFilter, TProcessor> : SystemBase
        where TNodeFilter : struct, IComponentData, INode 
        where TProcessor : struct, INodeProcessor<TNodeFilter>
    {
        private EntityQuery _query;

        protected bool IsNodeFilterHasArray;

        protected override void OnCreate()
        {
            _query = PrepareQuery();

            IsNodeFilterHasArray = !TypeManager.GetTypeInfo(TypeManager.GetTypeIndex<TNodeFilter>()).IsZeroSized;
        }

        protected virtual EntityQuery PrepareQuery()
        {
            return GetEntityQuery(typeof(NodeComponent), typeof(TNodeFilter));
        }

        protected override void OnUpdate()
        {
            var job = new ExecuteNodesJob<TNodeFilter, TProcessor>
            {
                NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
                NodeFilterType = GetComponentTypeHandle<TNodeFilter>(),
                EntityType = GetEntityTypeHandle(),
                IsNodeFilterHasArray = IsNodeFilterHasArray,
                Processor = PrepareProcessor()
            };

            var jobHandle = ShouldScheduleParallel
                ? job.ScheduleParallel(_query, Dependency)
                : job.Schedule(_query, Dependency);
            Dependency = jobHandle;
        }

        [BurstCompile]
        public struct ExecuteNodesJob<TNodeFilterJob, TProcessorJob> : IJobEntityBatchWithIndex
            where TNodeFilterJob : struct, IComponentData, INode 
            where TProcessorJob : struct, INodeProcessor<TNodeFilterJob>
        {
            [NativeDisableParallelForRestriction] 
            public ComponentTypeHandle<NodeComponent> NodeComponentType;
            public ComponentTypeHandle<TNodeFilterJob> NodeFilterType;
            
            [ReadOnly]
            public EntityTypeHandle EntityType;

            public bool IsNodeFilterHasArray;
            public TProcessorJob Processor;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
            {
                var nodes = batchInChunk.GetNativeArray(NodeComponentType);
                var entities = batchInChunk.GetNativeArray(EntityType);
                var nodeFilters = IsNodeFilterHasArray 
                    ? batchInChunk.GetNativeArray(NodeFilterType) 
                    : default;
                TNodeFilterJob defaultNodeFilter = default;

                Processor.BeforeChunkIteration(batchInChunk, batchIndex);

                var indexes = new NativeArray<int>(nodes.Length, Allocator.Temp);
                SortByDepth(ref nodes, ref indexes);

                for (int i = 0; i < batchInChunk.Count; i++)
                {
                    var reversed = indexes.Length - (i + 1);
                    var index = indexes[reversed];
                    
                    var node = nodes[index];
                    var entity = entities[index];
                    
                    if (node.IsExec == false)
                    {
                        continue;
                    }

                    if (IsNodeFilterHasArray)
                    {
                        var nodeFilter = nodeFilters[index];
                        ExecuteNode(in entity, ref node, ref nodeFilter, indexOfFirstEntityInQuery, i);
                        nodeFilters[index] = nodeFilter;
                    }
                    else
                    {
                        ExecuteNode(in entity, ref node, ref defaultNodeFilter, indexOfFirstEntityInQuery, i);
                    }

                    if (node.Result is NodeResult.Success or NodeResult.Failed)
                    {
                        node.IsExec = false;
                    }

                    nodes[index] = node;
                }

                indexes.Dispose();
            }

            private void ExecuteNode(in Entity nodeEntity,
                ref NodeComponent nodeComponent,
                ref TNodeFilterJob nodeFilter,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                if (nodeComponent.Started == false)
                {
                    nodeComponent.Result = Processor.Start(in nodeComponent.RootEntity,
                        in nodeEntity,
                        ref nodeFilter,
                        indexOfFirstEntityInQuery,
                        iterIndex);
                    nodeComponent.Started = true;

                    if (nodeComponent.Result is NodeResult.Success or NodeResult.Failed)
                    {
                        return;
                    }
                }

                nodeComponent.Result = Processor.Update(in nodeComponent.RootEntity,
                    in nodeEntity,
                    ref nodeFilter,
                    indexOfFirstEntityInQuery,
                    iterIndex);
            }

            private void SortByDepth(ref NativeArray<NodeComponent> nodes, ref NativeArray<int> indexes)
            {
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }
                
                IntroSort(ref indexes, ref nodes);
            }

            private void IntroSort(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes)
            {
                var partitionSize = Partition(ref indexes, ref nodes, 0, indexes.Length - 1);

                if (partitionSize < 16)
                {
                    InsertionSort(ref indexes, ref nodes);
                }
                else if (partitionSize > (2 * math.log(indexes.Length)))
                {
                    HeapSort(ref indexes, ref nodes);
                }
                else
                {
                    QuickSortRecursive(ref indexes, ref nodes, 0, indexes.Length - 1);
                }
            }

            private int Partition(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes, int left,
                int right)
            {
                var pivotIndex = indexes[right];
                var i = left;

                for (int j = left; j < right; ++j)
                {
                    var currentDepth = nodes[indexes[j]].DepthIndex;
                    var pivotDepth = nodes[pivotIndex].DepthIndex;

                    if (currentDepth <= pivotDepth)
                    {
                        (indexes[j], indexes[i]) = (indexes[i], indexes[j]);
                        i++;
                    }
                }

                indexes[right] = indexes[i];
                indexes[i] = pivotIndex;

                return i;
            } 

            private void InsertionSort(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes)
            {
                for (int i = 1; i < indexes.Length; i++)
                {
                    var j = i;

                    while (j > 0)
                    {
                        var last = indexes[j - 1];
                        var next = indexes[j];
                        
                        if (nodes[last].DepthIndex > nodes[next].DepthIndex)
                        {
                            indexes[j - 1] ^= indexes[j];
                            indexes[j] ^= indexes[j - 1];
                            indexes[j - 1] ^= indexes[j];

                            j--;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            private void HeapSort(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes)
            {
                var heapSize = indexes.Length;

                for (int p = (heapSize - 1) / 2; p >= 0; p--)
                {
                    MaxHeapify(ref indexes, ref nodes, heapSize, p);
                }

                for (int i = indexes.Length - 1; i > 0; i--)
                {
                    (indexes[i], indexes[0]) = (indexes[0], indexes[i]);

                    heapSize--;
                    MaxHeapify(ref indexes, ref nodes, heapSize, 0);
                }
            }

            private void MaxHeapify(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes, int heapSize,
                int current)
            {
                var left = (current + 1) * 2 - 1;
                var right = (current + 1) * 2;
                var largest = 0;

                if (left < heapSize)
                {
                    var currentIndex = indexes[current];
                    var leftIndex = indexes[left];

                    var currentDepth = nodes[currentIndex].DepthIndex;
                    var leftDepth = nodes[leftIndex].DepthIndex;

                    if (leftDepth > currentDepth)
                    {
                        largest = left;
                    }
                    else
                    {
                        largest = current;
                    }
                }
                else
                {
                    largest = current;
                }

                if (right < heapSize)
                {
                    var largestIndex = indexes[largest];
                    var rightIndex = indexes[right];

                    var largestDepth = nodes[largestIndex].DepthIndex;
                    var rightDepth = nodes[rightIndex].DepthIndex;

                    if (rightDepth > largestDepth)
                    {
                        largest = right;
                    }
                }

                if (largest != current)
                {
                    (indexes[current], indexes[largest]) = (indexes[largest], indexes[current]);

                    MaxHeapify(ref indexes, ref nodes, heapSize, largest);
                }
            }

            private void QuickSortRecursive(ref NativeArray<int> indexes, ref NativeArray<NodeComponent> nodes,
                int left, int right)
            {
                if (left < right)
                {
                    var q = Partition(ref indexes, ref nodes, left, right);
                    QuickSortRecursive(ref indexes, ref nodes, left, q - 1);
                    QuickSortRecursive(ref indexes, ref nodes, q + 1, right);
                }
            }
        }

        protected abstract TProcessor PrepareProcessor();

        protected virtual void AfterJobScheduling(in JobHandle handle)
        {
            // Routines like calling AddJobHandleForProducer() may be placed here
        }

        protected virtual bool ShouldScheduleParallel { get; set; } = true;

        protected ref readonly EntityQuery Query => ref _query;
    }
}