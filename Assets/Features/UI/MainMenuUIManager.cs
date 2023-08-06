using Features.GameManagers;
using Features.ServiceLocators.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuPanel;

        [SerializeField] private MainMenuSettingsUIManager _settingsUIManager;

        [SerializeField] private Button _lastSelectedButton;
        
        private GameManager _gameManager;

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

        #region Monobehaviour

        private void Awake()
        {
            _gameManager = ServiceLocator.Resolve<GameManager>();
            
            ShowMainMenu();
        }

        #endregion

        #region Called by buttons

        public void PlayGame()
        {
            #if UNITY_EDITOR
            Debug.Log("Start game! (mockup for now)");
            #endif
            _gameManager.Restart();
        }
        
        public void ShowSettings()
        {
            HideMainMenu();
            _settingsUIManager.ShowSettings();
        }

        public void QuitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
            Debug.Log("Quit game! (does nothing in editor)");
            #endif
        }

        #endregion
        
        

    }
}