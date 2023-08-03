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
    public class SkillCooldownsHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image _ultimateSlider;
        [SerializeField] private Image _secondarySlider;

        private CharacterHolder _characterHolder;
        private SkillsController _skillsController;

        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _skillsController = _characterHolder.CurrentCharacter.SkillsController;
        }
        
        private void OnDisable()
        {
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            _skillsController = _characterHolder.CurrentCharacter.SkillsController;
        }
        
        
        public void OnUpdate(float deltaTime)
        {
            if (_skillsController == null) return;
            
            if (_skillsController.Ultimate.IsOnCooldown)
            {
                _ultimateSlider.fillAmount = 1f -
                                             _skillsController.Ultimate.CurrentCooldown /
                                             _skillsController.Ultimate.MaxCooldown;
            }
            else
            {
                _ultimateSlider.fillAmount = 1f;
            }
            
            if (_skillsController.Secondary.IsOnCooldown)
            {
                _secondarySlider.fillAmount = 1f -
                                              _skillsController.Secondary.CurrentCooldown /
                                              _skillsController.Secondary.MaxCooldown;
            }
            else
            {
                _secondarySlider.fillAmount = 1f;
            }
        }
    }
}