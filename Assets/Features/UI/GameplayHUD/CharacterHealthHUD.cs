using Features.Character;
using Features.Character.Spawn;
using Features.Health;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.GameplayHUD
{
    public class CharacterHealthHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image _healthSlider;
        
        private CharacterHolder _characterHolder;
        private HealthComponent _characterHealthComponent;
        
        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _characterHealthComponent = _characterHolder.CurrentCharacter.HealthComponent;
        }
        
        private void OnDisable()
        {
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            _characterHealthComponent = character.HealthComponent;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_characterHealthComponent == null) return;
            _healthSlider.fillAmount = _characterHealthComponent.CurrentHealth / _characterHealthComponent.MaxHealth;
        }
    }
}