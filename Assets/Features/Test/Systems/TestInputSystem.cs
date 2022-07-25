using Features.HealthSystem.Components;
using Features.HealthSystem.Systems;
using Features.Test.Services;
using Unity.Entities;
using UnityEngine;

namespace Features.Test.Systems
{
    [UpdateBefore(typeof(ApplyDamageSystem))]
    public partial class TestInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            bool applyDamage = false;
            
            Entities
                .WithAll<TestInputWrapper, TestInputConfiguration>()
                .ForEach(
                    (in TestInputWrapper input, in TestInputConfiguration conf) =>
                    { 
                        applyDamage  = input.Value.actions[conf.ApplyDamageActionID].WasPressedThisFrame();
                        if (!applyDamage) return;
                        
                        Debug.Log($"{conf.ApplyDamageActionID} input was pressed");
                    })
                .WithoutBurst()
                .Run();

            // Entities.WithAll<Damage>().ForEach((ref Damage damage) =>
            // {
            //     damage.Enable = applyDamage;
            //             
            //     if (damage.Enable)
            //     {
            //         damage.Value = 5f;
            //     }
            // }).WithBurst().Run();
        }
    }
}