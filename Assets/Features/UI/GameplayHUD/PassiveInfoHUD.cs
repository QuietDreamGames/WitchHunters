using Features.Character;
using Features.Character.Spawn;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ServiceLocators.Core;
using Features.Skills.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.GameplayHUD
{
    public class PassiveInfoHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image _passiveSlider;
        
        private APassiveController _passiveController;
        private BaseModifiersContainer _baseModifiersContainer;
        private ModifiersContainer _modifiersContainer;
        
        private CharacterHolder _characterHolder;
        
        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _passiveController = _characterHolder.CurrentCharacter.PassiveController;
            _baseModifiersContainer = _characterHolder.CurrentCharacter.BaseModifiersContainer;
            _modifiersContainer = _characterHolder.CurrentCharacter.ModifiersContainer;
        }
        
        private void OnDisable()
        {
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            _passiveController = character.PassiveController;
            _baseModifiersContainer = character.BaseModifiersContainer;
            _modifiersContainer = character.ModifiersContainer;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_passiveController == null) return;

            if (_passiveController.CurrentPassiveInfo.IsCharged)
            {
                _passiveSlider.fillAmount = _passiveController.CurrentPassiveInfo.CurrentTimer /
                                            _modifiersContainer.GetValue(ModifierType.PassiveChargedTime,
                                                _baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargedTime));
                return;
            }
            _passiveSlider.fillAmount = _passiveController.CurrentPassiveInfo.CurrentCharge /
                                        _modifiersContainer.GetValue(ModifierType.PassiveAmountToCharge,
                                            _baseModifiersContainer.GetBaseValue(ModifierType.PassiveAmountToCharge));
        }
    }
}