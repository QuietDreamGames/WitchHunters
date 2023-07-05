using Features.Damage.Core;
using Features.Health;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Team;
using UnityEngine;

namespace Features.Test
{
    public class DummyController : MonoBehaviour
    {
        [SerializeField] protected BaseModifiersContainer _baseModifiersContainer;
        [SerializeField] protected DamageController _damageController;
        
        private ModifiersContainer _modifiersContainer;
        private HealthComponent _healthComponent;
        

        private void Awake()
        {
            _modifiersContainer = new ModifiersContainer();
            ApplyBaseModifiers();
            _healthComponent = new HealthComponent(_modifiersContainer, _baseModifiersContainer);
            _damageController.Initiate(_modifiersContainer, _baseModifiersContainer, _healthComponent, TeamIndex.Enemy);
        }
        
        private void ApplyBaseModifiers()
        {
            var baseModifiers = _baseModifiersContainer.baseModifiers;
            foreach (var baseModifier in baseModifiers)
            {
                _modifiersContainer.Add(baseModifier);
            }
        }
    }
}
