using Features.HealthSystem.Components;
using Features.UI.RegularEnemyHealth.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.UI.RegularEnemyHealth.Systems
{
    public partial class RegularEnemyHealthSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<RegularHealthBar, Health>().ForEach((in Health health,
                in RegularHealthBar regularHealthBar) =>
            {
                var currentHealthPercent = Mathf.Clamp01(health.Value / health.MaxValue);
                var newScale = regularHealthBar.FullHealthBarTransform.localScale;
                newScale.x *= currentHealthPercent;
                
                regularHealthBar.CurrentHealthBarTransform.localScale = newScale;
            }).WithoutBurst().Run();
        }
        
        // private partial struct UpdateHealthBarJob : IJobEntity
        // {
        //     public void Execute(Health health, RegularHealthBar regularHealthBar)
        //     {
        //         
        //     }
        // }
    }
}