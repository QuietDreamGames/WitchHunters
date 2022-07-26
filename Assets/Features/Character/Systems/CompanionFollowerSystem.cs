using Features.Character.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Features.Character.Systems
{
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
