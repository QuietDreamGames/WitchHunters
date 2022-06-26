using Features.Character.Components;
using Features.HealthSystem.Components;
using Unity.Entities;

namespace Features.UI.CharacterHealth
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class UpdateCharacterHUDHealthSliderSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            UpdateHUDHealthBarData();
        }

        private void UpdateHUDHealthBarData()
        {
            RequireSingletonForUpdate<HUDHealthBarUIData>();
            
            var hudHealthBarUIEntity = GetSingletonEntity<HUDHealthBarUIData>();
            var hudHealthBarUIData = EntityManager.GetComponentData<HUDHealthBarUIData>(hudHealthBarUIEntity);
            
            Entities.WithAll<PlayerTag, Health>().ForEach((in Health health) =>
            {
                hudHealthBarUIData.Image.fillAmount = health.Value / health.MaxValue;
                hudHealthBarUIData.Text.text = $"{(int)health.Value}/{(int)health.MaxValue}";
                
            }).WithoutBurst().Run();
        }
    }
}