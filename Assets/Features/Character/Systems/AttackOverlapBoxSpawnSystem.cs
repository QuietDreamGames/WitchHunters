using Features.Character.Components;
using Unity.Entities;

namespace Features.Character.Systems
{
    [UpdateAfter(typeof(AttackSystem))]
    public partial class AttackOverlapBoxSpawnSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            var autoAttackQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadWrite<Autoattacks>(),
                ComponentType.ReadWrite<Attack>());
        }
    }
}