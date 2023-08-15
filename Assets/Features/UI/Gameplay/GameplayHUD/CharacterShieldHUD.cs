using Features.Character;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using Features.Skills.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Gameplay.GameplayHUD
{
    public class CharacterShieldHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image _healthSlider;
        
        private CharacterHolder _characterHolder;
        private ShieldHealthController _characterShieldHealthComponent;
        
        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _characterShieldHealthComponent = _characterHolder.CurrentCharacter.ShieldHealthController;
        }
        
        private void OnDisable()
        {
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            _characterShieldHealthComponent = character.ShieldHealthController;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_characterShieldHealthComponent == null) return;
            _healthSlider.fillAmount = _characterShieldHealthComponent.CurrentShieldHealth /
                                       _characterShieldHealthComponent.MaxShieldHealth;
        }
    }
}