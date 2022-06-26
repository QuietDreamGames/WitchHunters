using Features.Character.Components;
using Features.HealthSystem.Components;
using Features.UI.CharacterHealth;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Features.HealthSystem.Systems
{
    [BurstCompile]
    public partial class ApplyDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            Entities.WithAll<Damage, Health>().ForEach((Entity e, ref Health health, ref Damage damage) =>
            {
                if (!damage.Enable) return;
                
                health.Value -= damage.Value;

                if (health.Value <= 0f)
                {
                    Debug.Log(EntityManager.AddComponent(e, typeof(DeathFlag)));
                }

                damage.Enable = false;

            }).WithoutBurst().WithStructuralChanges().Run();
        }
    }
}