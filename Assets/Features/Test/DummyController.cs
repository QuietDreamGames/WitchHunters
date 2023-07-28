using Features.Damage.Core;
using Features.Health;
using Features.Knockback;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Test
{
    public class DummyController : MonoBehaviour
    {
        [SerializeField] protected BaseModifiersContainer _baseModifiersContainer;
        [SerializeField] protected DamageController _damageController;
        [SerializeField] protected KnockbackController _knockbackController;
        
        private ModifiersContainer _modifiersContainer;
        private HealthComponent _healthComponent;
        

        private void Awake()
        {
            _modifiersContainer = new ModifiersContainer();
            _healthComponent = new HealthComponent(_modifiersContainer, _baseModifiersContainer);
            _damageController.Initiate(_modifiersContainer, _baseModifiersContainer, _healthComponent);
            _knockbackController.Initiate(_modifiersContainer, _baseModifiersContainer);
        }
    }
}
