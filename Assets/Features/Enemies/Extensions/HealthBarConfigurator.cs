using Features.Damage.Core;
using Features.ServiceLocators.Core;
using Features.UI.Gameplay;
using Features.UI.Gameplay.GameplayHUD;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class HealthBarConfigurator : MonoBehaviour
    {
        [SerializeField] private UnitBehaviour unit;
        [SerializeField] private DamageController damageController;

        [Space] 
        [SerializeField] private string nameId;
        
        private BossHealthHUD _healthBarHUD;

        private void Awake()
        {
            var gameplayCanvasContainer = ServiceLocator.Resolve<GameplayCanvasContainer>();
            _healthBarHUD = gameplayCanvasContainer.bossHealthHUD;
            
            unit.OnSpawn += OnSpawnHandler;
            unit.OnDespawn += OnDespawnHandler;
        }

        private void OnDestroy()
        {
            unit.OnSpawn -= OnSpawnHandler;
            unit.OnDespawn -= OnDespawnHandler;
        }
        
        private void OnSpawnHandler()
        {
            _healthBarHUD.Subscribe(damageController.HealthComponent, nameId);
        }
        
        private void OnDespawnHandler()
        {
            _healthBarHUD.Unsubscribe(damageController.HealthComponent);
        }
    }
}
