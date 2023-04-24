using Features.Animator.Components;
using Features.Character.Components;
using Features.Character.Systems.SystemGroups;
using Features.HealthSystem.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Character.Systems
{
    [UpdateAfter(typeof(LateSimulationSystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class DeathSystem : SystemBase
    {
        private NativeList<Entity> _parentList;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            _parentList = new NativeList<Entity>(Allocator.Persistent);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _parentList.Dispose();
        }

        protected override void OnUpdate()
        {
            var any = false;
            
            Entities
                .WithAll<DeathFlag>()
                .ForEach((AnimatorWrapper animator, in AnimatorConfiguration conf) =>
                {
                    animator.Value.SetTrigger(conf.Death.ToString());
                    any = true;
                })
                .WithoutBurst()
                .Run();

            if (any == false)
            {
                return;
            }
            
            Entities
                .WithAll<DeathFlag>()
                .ForEach((Rigidbody2DWrapper rigidbody2D) =>
                {
                    rigidbody2D.Value.velocity = Vector2.zero;
                })
                .WithoutBurst()
                .Run();

            _parentList.Clear();
            
            Entities
                .WithAll<DeathFlag>()
                .ForEach((Entity entity) =>
                {
                    _parentList.Add(entity);
                })
                .WithoutBurst()
                .Run();


            while (true)
            {
                var append = false;

                Entities
                    .WithAll<Parent>()
                    .ForEach((Entity entity, in Parent parent) =>
                    {
                        if (_parentList.Contains(parent.Value))
                        {
                            if (_parentList.Contains(entity) == false)
                            {
                                _parentList.Add(entity);
                                append = true;
                            }
                        }
                    })
                    .WithoutBurst()
                    .Run();

                if (append == false)
                {
                    break;
                }
            }

            EntityManager.DestroyEntity(_parentList);
        }
    }
}
