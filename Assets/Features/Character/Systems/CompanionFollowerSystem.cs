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
                .ForEach((in Translation translation, in CompanionFollower companion) => 
                { 
                    companion.Value.position = translation.Value;
                })
                .WithoutBurst()
                .Run();
        }
    }
}
