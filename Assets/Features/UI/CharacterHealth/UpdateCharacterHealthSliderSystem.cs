using Unity.Entities;
using Unity.Transforms;

namespace Features.UI.CharacterHealth
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class UpdateCharacterHealthSliderSystem : SystemBase
    {
        
        
        protected override void OnUpdate()
        {
            // Entities.ForEach((HealthBarUIData healthBarUIData, in Translation))
        }
    }
}