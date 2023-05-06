using Features.Animator.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Presentation.Feature
{
    public partial class FlockSystem : SystemBase
    {
        private EntityQuery _query;
        
        private NativeArray<Translation> _translations;
        private NativeArray<FlockComponent> _flocks;

        protected override void OnCreate()
        {
            base.OnCreate();
            _query = GetEntityQuery(
                typeof(SeparationComponent), 
                typeof(AlignmentComponent),
                typeof(CohesionComponent), 
                typeof(FlockComponent),
                typeof(Translation));
        }

        protected override void OnUpdate()
        {
            if (_translations.IsCreated)
            {
                _translations.Dispose();
            }
            
            if (_flocks.IsCreated)
            {
                _flocks.Dispose();
            }
            
            _translations = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
            _flocks = _query.ToComponentDataArray<FlockComponent>(Allocator.TempJob);

            var separationType = GetComponentTypeHandle<SeparationComponent>(false);
            var alignmentType = GetComponentTypeHandle<AlignmentComponent>(false);
            var cohesionType = GetComponentTypeHandle<CohesionComponent>(false);
            
            var separationJob = new SeparationJob
            {
                Translations = _translations,
                SeparationType = separationType,
            };
            var alignmentJob = new AlignmentJob
            {
                Translations = _translations,
                Flocks = _flocks,
                AlignmentType = alignmentType,
            };
            var cohesionJob = new CohesionJob
            {
                Translations = _translations,
                CohesionType = cohesionType,
            };
            var boundsJob = new BoundsJob
            {
            };
            var animatorJob = new AnimatorJob
            {
            };
            
            animatorJob.Run(_query);
            Dependency = separationJob.ScheduleParallel(_query, Dependency);
            Dependency = alignmentJob.ScheduleParallel(_query, Dependency);
            Dependency = cohesionJob.ScheduleParallel(_query, Dependency);
            Dependency = boundsJob.ScheduleParallel(_query, Dependency);

            var flockJob = new FlockJob
            {
                DeltaTime = Time.DeltaTime,
            };

            Dependency = flockJob.ScheduleParallel(_query, Dependency);
        }

        [BurstCompile]
        private partial struct SeparationJob : IJobEntityBatchWithIndex
        {
            [ReadOnly]
            public NativeArray<Translation> Translations;
            [NativeDisableParallelForRestriction]
            public ComponentTypeHandle<SeparationComponent> SeparationType;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
            {
                var separations = batchInChunk.GetNativeArray(SeparationType);

                for (var index = 0; index < batchInChunk.Count; index++)
                {
                    var steer = float3.zero;
                    var total = 0;
                    var selfIndex = indexOfFirstEntityInQuery + index;
                    var self = Translations[selfIndex];                    
                    var separation = separations[index];
                    for (var i = 0; i < Translations.Length; i++)
                    {
                        var other = Translations[i];
                        var dist = math.distance(other.Value, self.Value);
                        if (selfIndex != i && dist < separation.Weight)
                        {
                            steer += other.Value;
                            total++;
                        }
                    }
                    if (total > 0)
                    {
                        steer /= total;
                        var dist = math.distance(steer, self.Value);
                        if (dist < separation.Weight)
                        {
                            steer = -math.normalize(steer - self.Value);
                        }
                    }
                    separation.Enabled = total > 0;
                    separation.Value = steer;
                    
                    separations[index] = separation;
                }
                
                separations.Dispose();
            }
        }
        
        [BurstCompile]
        private partial struct AlignmentJob : IJobEntityBatchWithIndex 
        { 
            [ReadOnly]
            public NativeArray<Translation> Translations;
            [ReadOnly]
            public NativeArray<FlockComponent> Flocks;

            [NativeDisableParallelForRestriction] 
            public ComponentTypeHandle<AlignmentComponent> AlignmentType;
            
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery) 
            { 
                var alignments = batchInChunk.GetNativeArray(AlignmentType);

                for (var index = 0; index < batchInChunk.Count; index++)
                {
                    var steer = float3.zero;
                    var total = 0;
                    var selfIndex = indexOfFirstEntityInQuery + index;
                    var self = Translations[selfIndex];
                    var alignment = alignments[index];
                    for (var i = 0; i < Translations.Length; i++)
                    {
                        var other = Translations[i];
                        var dist = math.distance(other.Value, self.Value);
                        if (selfIndex != i && dist < alignment.Weight)
                        {
                            steer += Flocks[i].Velocity;
                            total++;
                        }
                    }

                    if (total > 0)
                    {
                        steer /= total;
                        steer = math.normalize(steer);
                    }

                    alignment.Enabled = total > 0;
                    alignment.Value = steer;
                    
                    alignments[index] = alignment;
                }
                
                alignments.Dispose();
            }
        }
        
        [BurstCompile]
        private partial struct CohesionJob : IJobEntityBatchWithIndex 
        { 
            [ReadOnly]
            public NativeArray<Translation> Translations;
            
            [NativeDisableParallelForRestriction] 
            public ComponentTypeHandle<CohesionComponent> CohesionType;
            
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery) 
            { 
                var cohesions = batchInChunk.GetNativeArray(CohesionType);

                for (var index = 0; index < batchInChunk.Count; index++)
                {
                    var steer = float3.zero;
                    var total = 0;
                    var selfIndex = indexOfFirstEntityInQuery + index;
                    var self = Translations[selfIndex];   
                    var cohesion = cohesions[index];
                    for (var i = 0; i < Translations.Length; i++)
                    {
                        var other = Translations[i];
                        var dist = math.distance(other.Value, self.Value);
                        if (selfIndex != i && dist < cohesion.Weight)
                        {
                            steer += other.Value;
                            total++;
                        }
                    }

                    if (total > 0)
                    {
                        steer /= total;
                        steer = math.normalize(steer - self.Value);
                    }

                    cohesion.Enabled = total > 0;
                    cohesion.Value = steer;
                    
                    cohesions[index] = cohesion;
                }
                
                cohesions.Dispose();
            }
        }

        [BurstCompile]
        private partial struct BoundsJob : IJobEntity
        {
            public void Execute(
                [EntityInQueryIndex] int index,
                [ReadOnly] in Translation translation,
                [ReadOnly] in FlockComponent flockComponent,
                ref BoundsComponent boundsComponent)
            {
                var centerPosition = boundsComponent.Center;
                var size = boundsComponent.Size;
                var maxSpeed = flockComponent.MaxSpeed;
                var steer = float3.zero;
                var outOfBounds = false;
                if (translation.Value.x > (centerPosition.x + size.x / 2))
                {
                    steer.x = -maxSpeed;
                    outOfBounds = true;
                }
                else if (translation.Value.x < centerPosition.x - size.x / 2)
                {
                    steer.x = maxSpeed;
                    outOfBounds = true;
                }

                if (translation.Value.y > centerPosition.y + size.y / 2)
                {
                    steer.y = -maxSpeed;
                    outOfBounds = true;
                }
                else if (translation.Value.y < centerPosition.y - size.y / 2)
                {
                    steer.y = maxSpeed;
                    outOfBounds = true;
                }

                boundsComponent.Enabled = outOfBounds;
                boundsComponent.Value = steer;
            }
        }
        
        [BurstCompile]
        private partial struct FlockJob : IJobEntity
        {
            public float DeltaTime;

            public void Execute(
                [ReadOnly] in SeparationComponent separationComponent,
                [ReadOnly] in AlignmentComponent alignmentComponent,
                [ReadOnly] in CohesionComponent cohesionComponent,
                [ReadOnly] in BoundsComponent boundsComponent,
                ref Translation translation,
                ref FlockComponent flockComponent)
            {
                var steer = float3.zero;

                if (boundsComponent.Enabled)
                {
                    steer = boundsComponent.Value;
                }
                else if (separationComponent.Enabled)
                {
                    steer = separationComponent.Value * DeltaTime;
                }
                else if (alignmentComponent.Enabled)
                {
                    steer = alignmentComponent.Value * DeltaTime;
                }
                else if (cohesionComponent.Enabled)
                {
                    steer = cohesionComponent.Value * DeltaTime;
                }

                if (math.isnan(flockComponent.Velocity.x) || 
                    math.isnan(flockComponent.Velocity.y) ||
                    math.isnan(flockComponent.Velocity.z))
                {
                    flockComponent.Velocity = float3.zero;
                }
                
                translation.Value += flockComponent.Velocity;
                flockComponent.Velocity += steer;

                var velocity = flockComponent.Velocity;
                var maxSpeed = flockComponent.MaxSpeed;

                if (math.length(velocity) > maxSpeed)
                {
                    flockComponent.Velocity = (velocity / math.length(velocity)) * maxSpeed;
                }
                
                if (math.isnan(flockComponent.Velocity.x) || 
                    math.isnan(flockComponent.Velocity.y) ||
                    math.isnan(flockComponent.Velocity.z))
                {
                    flockComponent.Velocity = float3.zero;
                }
            }
        }
        
        private partial struct AnimatorJob : IJobEntity
        {
            public void Execute(
                AnimatorWrapper animatorWrapper,
                AnimatorConfComponent animatorConfComponent,
                [ReadOnly] in FlockComponent flockComponent)
            {
                var animator = animatorWrapper.Value;
                
                var horizontal = animatorConfComponent.Horizontal;
                var vertical = animatorConfComponent.Vertical;
                var moving = animatorConfComponent.Moving;
                
                var direction = flockComponent.Velocity;

                animator.SetFloat(horizontal, direction.x);
                animator.SetFloat(vertical, direction.y);
                animator.SetBool(moving, true);
            } 
        }
    }
}