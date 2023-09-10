using Features.Character;
using Features.Character.Spawn;
using Features.Experience;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Gameplay.GameplayHUD
{
    public class CharacterXPHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image _xpBarSlider; 
        [SerializeField] private float _startValue = 0.113f;
        [SerializeField] private float _endValue = 0.889f;
        
        private CharacterHolder _characterHolder;
        private LevelController _levelController;
    
        
        public void UpdateXpBar(float value)
        {
            _xpBarSlider.fillAmount = Mathf.Lerp(_startValue, _endValue, value);
        }
        
        
        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _levelController = _characterHolder.CurrentCharacter.LevelController;
        }
        
        private void OnDisable()
        {
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            _levelController = character.LevelController;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_levelController == null) return;
            UpdateXpBar(_levelController.GetExpPercentage());
        }
    }
}