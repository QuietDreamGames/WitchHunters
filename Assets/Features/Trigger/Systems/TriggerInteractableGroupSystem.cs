using Features.Trigger.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Features.Trigger.Systems
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public partial class TriggerInteractableGroupSystem : SystemBase
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        private EntityQueryDesc _interactablesGroupQueryDesc;
        private EntityQueryDesc _triggerListenersGroupQueryDesc;

        protected override void OnCreate()
        {
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

            _interactablesGroupQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {typeof(TriggerInteractable)}
            };
            
            _triggerListenersGroupQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {typeof(InTrigger)}
            };
        }
        
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            this.RegisterPhysicsRuntimeSystemReadOnly();
        }

        protected override void OnUpdate()
        {
            // var jobHandle = new TriggerInteractableJob
            // {
            //     TriggersGroup = GetEntityQuery(_interactablesGroupQueryDesc),
            //     TriggerListenersGroup = GetEntityQuery(_triggerListenersGroupQueryDesc)
            // };
            //
            // Dependency = jobHandle.Schedule( _stepPhysicsWorld.Simulation , Dependency );

        }

        [BurstCompile]
        private struct TriggerInteractableJob : ITriggerEventsJob
        {
            public EntityQuery TriggersGroup;
            public EntityQuery TriggerListenersGroup;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                var entityA = triggerEvent.EntityA;
                var entityB = triggerEvent.EntityB;

                // var isBodyATrigger = TriggersGroup.;
            }
        }
    }
}