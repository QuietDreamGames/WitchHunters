using Features.Localization;
using Features.Localization.Services;
using UnityEngine;

namespace Features.UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        
        public void ShowSettings()
        {
            _settingsPanel.SetActive(true);
        }

        public void HideSettings()
        {
            _settingsPanel.SetActive(false);
        }

        public void ChangeLanguage(int lang)
        {
            var localizationLang = (LocalizationLang)lang;
            LocalizationManager.Instance.ChangeLocalization(localizationLang);
        }
    }
}