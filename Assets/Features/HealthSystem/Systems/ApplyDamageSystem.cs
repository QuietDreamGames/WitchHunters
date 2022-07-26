using Features.Character.Components;
using Features.HealthSystem.Components;
using Features.UI.CharacterHealth;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Features.HealthSystem.Systems
{
    [BurstCompile]
    public partial class ApplyDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            var applyDamageJob  = new ApplyDamageJob{EntityManager = EntityManager, EntityCommandBuffer = ecb, DeltaTime = Time.DeltaTime}.Schedule();
            applyDamageJob.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
        
        [BurstCompile]
        public partial struct ApplyDamageJob : IJobEntity
        {
            public EntityManager EntityManager;
            public EntityCommandBuffer EntityCommandBuffer;
            
            public float DeltaTime;
            private void Execute(Entity e, ref Health health, DamageableTag damageableTag) //, in DynamicBuffer<Damage> damages)
            {
                var damages = EntityManager.GetBuffer<Damage>(e);

                for (var i = 0; i < damages.Length; i++)
                {
                    if (!damages[i].Enabled)
                    {
                        var  tmp = damages[i].Cooldown;
                        tmp -= DeltaTime;

                        damages[i] = new Damage
                        {
                            SourceEntityId = damages[i].SourceEntityId,
                            Cooldown = tmp,
                            Enabled = false
                        };

                        if (damages[i].Cooldown <= 0)
                        {
                            damages.RemoveAt(i);
                            i--;
                        }
                        continue;
                    }
                    
                    health.Value -= damages[i].Value;
                    damages[i] = new Damage
                    {
                        SourceEntityId = damages[i].SourceEntityId,
                        Cooldown = damages[i].Cooldown,
                        Enabled = false
                    };

                    if (health.Value <= 0f)
                    {
                        EntityCommandBuffer.AddComponent<DeathFlag>(e);
                        return;
                    }
                
                    
                    
                }
            }
        }
    }
}