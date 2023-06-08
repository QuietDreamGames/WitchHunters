using System;
using Features.Localization;
using Features.Localization.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI
{
    public class MainMenuSettingsUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        
        [SerializeField] private MainMenuUIManager _mainMenuUIManager;

        [SerializeField] private Button _firstButton;

        public void ShowSettings()
        {
            _settingsPanel.SetActive(true);
            _firstButton.Select();
        }
        
        public void HideSettings()
        {
            _settingsPanel.SetActive(false);
            _mainMenuUIManager.ShowMainMenu();
        }

        public void ChangeLanguage(int lang)
        {
            var localizationLang = (LocalizationLang)lang;
            LocalizationManager.Instance.ChangeLocalization(localizationLang);
        }
        
        
    }
}