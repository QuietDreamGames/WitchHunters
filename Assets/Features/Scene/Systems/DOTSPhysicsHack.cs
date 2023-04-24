using Features.SceneManagement.Components;
using Unity.Collections;
using Unity.Entities;

namespace Features.Scene.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true)]
    public partial class DOTSPhysicsHack : SystemBase
    {
        private const string PhysicsSystemRuntimeDataType = "Unity.Physics.Systems.PhysicsSystemRuntimeData";
        private const string PhysicsSystemRuntimeDataName = "PhysicsSystemRuntimeData";
    
        protected override void OnCreate()
        {
            base.OnCreate();
        
            var query = GetEntityQuery(typeof(Entity));
            var entities = query.ToEntityArray(Allocator.TempJob);
        
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var components = EntityManager.GetComponentTypes(entity, Allocator.TempJob);
            
                for (var j = 0; j < components.Length; j++)
                {
                    var componentType = components[j];
                
                    if (componentType.ToString() == PhysicsSystemRuntimeDataType)
                    {
                        EntityManager.SetName(entity, PhysicsSystemRuntimeDataName);
                        EntityManager.AddComponent<DontDestroyOnLoadFlag>(entity);
                    }
                }
            
                components.Dispose();
            }
        
            entities.Dispose();
        }

        protected override void OnUpdate()
        {
        
        }
    }
}
