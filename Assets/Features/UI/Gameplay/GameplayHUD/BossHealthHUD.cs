using System.Collections.Generic;
using Features.Health;
using Features.Localization;
using Features.TimeSystems.Interfaces.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Gameplay.GameplayHUD
{
    public class BossHealthHUD : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private GameObject _heathBarParent;
        [SerializeField] private Image _healthSliderLeftHalf;
        [SerializeField] private Image _healthSliderRightHalf;
        [SerializeField] private LocalizedText _bossNameText;
        
        private List<HealthComponent> _healthComponents = new List<HealthComponent>();
        
        public void Subscribe(HealthComponent healthComponent, string nameId)
        {
            _healthComponents.Add(healthComponent);
            _heathBarParent.SetActive(true);
            _bossNameText.UpdateText(nameId);
        }
        
        public void Unsubscribe(HealthComponent healthComponent)
        {
            _healthComponents.Remove(healthComponent);
            if (_healthComponents.Count == 0)
                _heathBarParent.SetActive(false);
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_healthComponents.Count == 0) return;
            
            _healthSliderLeftHalf.fillAmount = _healthComponents[0].CurrentHealth / _healthComponents[0].MaxHealth;
            _healthSliderRightHalf.fillAmount = _healthComponents[0].CurrentHealth / _healthComponents[0].MaxHealth;
        }
    }
}