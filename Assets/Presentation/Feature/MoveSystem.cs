using Features.Animator.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Presentation.Feature
{
    [BurstCompile]
    public partial class MoveSystem : SystemBase
    {
        private Random _random;
        
        protected override void OnCreate()
        {
            base.OnCreate();

            Application.targetFrameRate = 0;
            
            _random = new Random(1234);
        }

        protected override void OnUpdate()
        {
            _random.state = (uint) UnityEngine.Random.Range(1, int.MaxValue);
            var moveJob = new MoveJob
            {
                Random = _random,
                DeltaTime = Time.DeltaTime,
            };
            var animatorJob = new AnimatorJob();
            var syncJob = new PositionSyncJob();
            
            syncJob.Run();
            animatorJob.Run();
            Dependency = moveJob.ScheduleParallel(Dependency);
        }
        
        [BurstCompile]
        private partial struct MoveJob : IJobEntity
        {
            public Random Random;
            public float DeltaTime;
        
            public void Execute(
                [EntityInQueryIndex] int index,
                in MoveComponent moveComponent, 
                ref MoveStateComponent moveStateComponent, 
                ref Translation translation)
            {
                var pos = translation.Value;
                var targetPos = moveStateComponent.TargetPos;
                var dist =  math.distance(pos, targetPos);
                if (dist < 0.1f)
                {
                    var centerPosition = moveComponent.Center;
                    var size = moveComponent.Size;

                    Random.state += (uint)index;
                    var posX = Random.NextFloat(moveComponent.ClampMin.x, moveComponent.ClampMax.x);
                    var posY = Random.NextFloat(moveComponent.ClampMin.y, moveComponent.ClampMax.y);
                    var modX = math.abs(math.fmod(posX, 2));
                    var modY = math.abs(math.fmod(posY, 2));
                    var signX = modX > 1f ? 1 : -1;
                    var signY = modY > 1f ? 1 : -1;
                    moveStateComponent.TargetPos = new float3(posX * signX, posY * signY, 0);
                    
                    return;
                }
        
                moveStateComponent.Direction = math.normalize(targetPos - pos);
                var diff = moveStateComponent.Direction * moveComponent.Speed * DeltaTime;
                translation.Value += diff;
            }
        }

        private partial struct AnimatorJob : IJobEntity
        {
            public void Execute(
                AnimatorWrapper animatorWrapper,
                AnimatorConfComponent animatorConfComponent,
                [ReadOnly] in MoveStateComponent moveStateComponent)
            {
                var animator = animatorWrapper.Value;
                
                var horizontal = animatorConfComponent.Horizontal;
                var vertical = animatorConfComponent.Vertical;
                var moving = animatorConfComponent.Moving;
                
                var direction = moveStateComponent.Direction;

                animator.SetFloat(horizontal, direction.x);
                animator.SetFloat(vertical, direction.y);
                animator.SetBool(moving, math.length(direction) > 0.1f);
            } 
        }
        
        private partial struct PositionSyncJob : IJobEntity
        {
            public void Execute(
                TransformComponent transformComponent, 
                [ReadOnly] in Translation translation)
            {
                transformComponent.Value.position = translation.Value;
            }
        }
    }
}
