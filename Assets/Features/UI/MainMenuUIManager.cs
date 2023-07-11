using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuPanel;

        [SerializeField] private MainMenuSettingsUIManager _settingsUIManager;

        [SerializeField] private Button _lastSelectedButton;

        #region Called by scripts

        public void ShowMainMenu()
        {
            _mainMenuPanel.SetActive(true);
            _lastSelectedButton.Select();
        }

        public void HideMainMenu()
        {
            _lastSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            _mainMenuPanel.SetActive(false);
        }

        #endregion

        #region Called by buttons

        public void PlayGame()
        {
            Debug.Log("Start game! (mockup for now)");
        }
        
        public void ShowSettings()
        {
            HideMainMenu();
            _settingsUIManager.ShowSettings();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        #endregion
        
        

    }
}