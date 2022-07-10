using Features.Character.Components;
using Features.Character.Services;
using Features.InputSystem.Systems;
using Unity.Entities;

namespace Features.Character.Systems
{
    [UpdateAfter(typeof(CharacterInputSystem))]
    [UpdateBefore(typeof(MovementSystem))]
    public partial class AttackSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities
                .WithAll<Attack, Autoattacks>()
                .ForEach((ref Autoattacks autoattacks, ref Attack attack) => //always `ref` for blobs!
                {
                    // ref AutoattackInfo[] autoattackInfos = autoattacks.Value.;

                    if (attack.Cooldown > 0)
                    {
                        attack.Cooldown -= deltaTime;
                        attack.Enable = false;
                    }
                    else if (attack.Cooldown > -1)
                    {
                        attack.Cooldown -= deltaTime;
                    }
                    else
                    {
                        attack.CurrentAttackId = 0;
                        attack.NextAttackId = 0;
                    }
                    
                    if (attack.Enable) // here its just a new attack
                    {
                        ref var autoattackInfos = ref autoattacks.Value.Value.AutoattackInfos;
                        
                        attack.CurrentAttackId = attack.NextAttackId;
                        attack.NextAttackId += 1;

                        if (attack.NextAttackId >= autoattackInfos.Length)
                            attack.NextAttackId = 0;

                        ref var currentAttackInfo = ref autoattackInfos[attack.CurrentAttackId];
                        
                        attack.Cooldown = currentAttackInfo.Time;
                        attack.Damage = currentAttackInfo.BaseDamage;
                        
                        //Spawn collider somewhere here I guess

                    }
                })
                .WithBurst()
                .Run();
            
            
            
            
            Entities
                .WithAll<Movement, Attack>()
                .ForEach((ref Movement movement, in Attack attack) =>
                {
                    if (attack.Cooldown > 0)
                        movement.Enable = false;
                })
                .WithBurst()
                .Run();
            
            
        }
    }
}