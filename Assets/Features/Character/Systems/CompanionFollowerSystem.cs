using Features.Character.Components;
using Features.Character.Systems.SystemGroups;
using Unity.Entities;
using Unity.Transforms;

namespace Features.Character.Systems
{
    [UpdateInGroup(typeof(GameObjectSyncGroup))]
    [UpdateAfter(typeof(MovementSystem))]
    public partial class CompanionFollowerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Translation, CompanionFollower>()
                .ForEach((ref Translation translation, in CompanionFollower companion) => 
                { 
                    translation.Value = companion.Value.position;
                })
                .WithoutBurst()
                .Run();
        }
    }
}
