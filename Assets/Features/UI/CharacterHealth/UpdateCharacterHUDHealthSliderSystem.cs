using Features.Character.Components;
using Features.HealthSystem.Components;
using Unity.Entities;

namespace Features.UI.CharacterHealth
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class UpdateCharacterHUDHealthSliderSystem : SystemBase
    {
        private Entity _hudHealthBarUIEntity;
        private HUDHealthBarUIData _hudHealthBarUIData;
        
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<HUDHealthBarUIData>();

            _hudHealthBarUIEntity = GetSingletonEntity<HUDHealthBarUIData>();
            _hudHealthBarUIData = EntityManager.GetComponentData<HUDHealthBarUIData>(_hudHealthBarUIEntity);
            
            UpdateHUDHealthBarData();
        }

        protected override void OnUpdate()
        {
            UpdateHUDHealthBarData();
        }

        private void UpdateHUDHealthBarData()
        {
            Entities.WithAll<PlayerTag, Health>().ForEach((in Health health) =>
            {
                _hudHealthBarUIData.Image.fillAmount = health.Value / health.MaxValue;
                _hudHealthBarUIData.Text.text = $"{(int)health.Value}/{(int)health.MaxValue}";
                
            }).WithoutBurst().Run();
        }
    }
}