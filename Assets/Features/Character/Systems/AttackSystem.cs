using Features.Character.Components;
using Features.Character.Services;
using Features.InputSystem.Systems;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Systems
{
    [UpdateAfter(typeof(CharacterInputSystem))]
    [UpdateBefore(typeof(MovementSystem))]
    public partial class AttackSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Dependency = new AutoAttackJob { DeltaTime = deltaTime }.ScheduleParallel(Dependency);
            Dependency = new MovementDisableJob().ScheduleParallel(Dependency);
        }
        
        [BurstCompile]
        public partial struct AutoAttackJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(ref Autoattacks autoAttacks, ref Attack attack, ref AttackOverlapBox attackOverlapBox)
            {
                if (attack.Cooldown > 0)
                {
                    attack.Cooldown -= DeltaTime;
                    attack.Enable = false;
                    attack.Trigger = true;
                }
                else if (attack.Cooldown > -1)
                {
                    attack.Cooldown -= DeltaTime;
                }
                else
                {
                    attack.CurrentAttackId = 0;
                    attack.NextAttackId = 0;
                }
                
                ref var autoAttackInfos = ref autoAttacks.Value.Value.AutoattackInfos;
                    
                if (attack.Enable) // here its just a new attack
                {
                    attack.CurrentAttackId = attack.NextAttackId;
                    attack.NextAttackId += 1;

                    if (attack.NextAttackId >= autoAttackInfos.Length)
                    {
                        attack.NextAttackId = 0;
                    }
                    attack.Cooldown = autoAttackInfos[attack.CurrentAttackId].Time;
                    attack.Damage = autoAttackInfos[attack.CurrentAttackId].BaseDamage;
                        
                    //Spawn collider somewhere here I guess
                }
                
                ref var currentAttackInfo = ref autoAttackInfos[attack.CurrentAttackId];

                var timeFromAttackStart = currentAttackInfo.Time - attack.Cooldown;

                if (timeFromAttackStart >= currentAttackInfo.ColliderStartTime && timeFromAttackStart <= currentAttackInfo.ColliderStopTime)
                {
                    attackOverlapBox.Enable = true;
                    attackOverlapBox.Height = currentAttackInfo.ColliderHeight;
                    attackOverlapBox.Width = currentAttackInfo.ColliderWidth;
                    attackOverlapBox.OffsetXY = currentAttackInfo.OffsetXY;
                }
                else
                {
                    attackOverlapBox.Enable = false;
                }
            }
        }
        
        [BurstCompile]
        public partial struct MovementDisableJob : IJobEntity
        {
            private void Execute(ref Movement movement, in Attack attack)
            {
                if (attack.Cooldown > 0)
                {
                    movement.Enable = false;
                }
            }
        }
    }
}