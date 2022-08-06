using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Features.Character.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(DamageDealSystem.ExecuteNodesJob<DamageDeal, DamageDealSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
    public class DamageDealSystem : NodeExecutorSystem<DamageDeal, DamageDealSystem.Processor>
    {
        protected override DamageDealSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
            return new DamageDealSystem.Processor
            {
                SelfTimeComponents = GetComponentDataFromEntity<DeltaTime>(),
                
                SelfAttackComponents = GetComponentDataFromEntity<Attack>(),
                SelfAttackOverlapBoxComponents = GetComponentDataFromEntity<AttackOverlapBox>(),
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<DamageDeal>
        {
            [ReadOnly]
            public ComponentDataFromEntity<DeltaTime> SelfTimeComponents;
            
            public ComponentDataFromEntity<Attack> SelfAttackComponents;
            public ComponentDataFromEntity<AttackOverlapBox> SelfAttackOverlapBoxComponents;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref DamageDeal nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var attack = SelfAttackComponents[rootEntity];
                var overlapBox = SelfAttackOverlapBoxComponents[rootEntity];

                overlapBox.Enable = false;

                overlapBox.Width = nodeComponent.Width;
                overlapBox.Height = nodeComponent.Height;
                overlapBox.OffsetXY = nodeComponent.OffsetXY;

                attack.Cooldown = nodeComponent.Cooldown;
                attack.Damage = nodeComponent.Damage;
                
                nodeComponent.CurrentTime = 0;

                SelfAttackComponents[rootEntity] = attack;
                SelfAttackOverlapBoxComponents[rootEntity] = overlapBox;
                
                return NodeResult.Running;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref DamageDeal nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var time = SelfTimeComponents[rootEntity];
                
                var overlapBox = SelfAttackOverlapBoxComponents[rootEntity];

                nodeComponent.CurrentTime += time.Value;

                if (nodeComponent.CurrentTime >= nodeComponent.StopTime)
                {
                    SetActiveDamageDeal(false, rootEntity, ref overlapBox);
                
                    return NodeResult.Success; 
                }

                if (nodeComponent.CurrentTime >= nodeComponent.StartTime)
                {
                    if (overlapBox.Enable == false)
                    {
                        // SetActiveDamageDeal(true, rootEntity, ref overlapBox);
                    }

                    return NodeResult.Running;
                }

                return NodeResult.Running;
            }

            private void SetActiveDamageDeal(bool active, 
                Entity rootEntity, 
                ref AttackOverlapBox overlapBox)
            {
                overlapBox.Enable = active;
                
                SelfAttackOverlapBoxComponents[rootEntity] = overlapBox;
            }
        }
    }
}