using Features.Character.Components;
using Features.Character.Systems;
using Features.HealthSystem.Components;
using Features.HealthSystem.Systems;
using Features.UI.DeathScreen.Components;
using Unity.Entities;

namespace Features.Gameplay
{
    [UpdateAfter(typeof(ApplyDamageSystem))]
    [UpdateBefore(typeof(DeathSystem))]
    public partial class PlayerDeathReactionSystem : SystemBase
    {
        private static readonly int StartDeath = UnityEngine.Animator.StringToHash("StartDeath");

        protected override void OnUpdate()
        {
            Entities
                .WithAll<DeathFlag, PlayerTag>()
                .ForEach(() =>
                {
                    var deathScreenEntity = GetSingletonEntity<DeathScreen>();
                    var deathScreen = EntityManager.GetComponentData<DeathScreen>(deathScreenEntity);
                    
                    if (deathScreen.IsRunning) return;
                    deathScreen.IsRunning = true;
                    deathScreen.Animator.SetTrigger(StartDeath);
                })
                .WithoutBurst()
                .Run();
        }
    }
}