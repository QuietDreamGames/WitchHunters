using System;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.UI.Gameplay.EscapeMenu
{
    public class EscapeMenuManager : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private GameObject _escapeMenuPanel;
        [SerializeField] private Button _firstButton;
        
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        
        [SerializeField] private string _playerActionMapName = "Player";
        [SerializeField] private string _uiActionMapName = "UI";

        private PlayerInput _playerInput;
        private float _lastGameplayTimeScale;

        private bool _isInEscapeMenu;
        
        private void Start()
        {
            _playerInput = ServiceLocator.Resolve<PlayerInput>();
        }

        public void ShowEscapeMenu()
        {
            _escapeMenuPanel.SetActive(true);
            _firstButton.Select();
            ServiceLocator.Resolve<TimeSystem>().SetCategoryTimeScale("GamePlay", 0f);
            
            _isInEscapeMenu = true;
            _playerInput.SwitchCurrentActionMap(_uiActionMapName);
        }
        
        public void HideEscapeMenu()
        {
            _escapeMenuPanel.SetActive(false);
            ServiceLocator.Resolve<TimeSystem>().SetCategoryTimeScale("GamePlay", 1f);
            
            _isInEscapeMenu = false;
            _playerInput.SwitchCurrentActionMap(_playerActionMapName);
        }
        
        public void ExitToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_playerInput.actions["Escape"].WasPressedThisFrame()) return;

            if (_playerInput.currentActionMap.name == _playerActionMapName)
            {
                ShowEscapeMenu();
                
            }
            else
            {
                HideEscapeMenu();
                
            }
        }
    }
}