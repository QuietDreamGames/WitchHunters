using System;
using Features.GameManagers;
using Features.Input;
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
        private GameManager _gameManager;
        private GameStateManager _gameStateManager;
        
        private void Start()
        {
            _gameManager = ServiceLocator.Resolve<GameManager>();
            _gameStateManager = ServiceLocator.Resolve<GameStateManager>();
            
            var inputData = ServiceLocator.Resolve<InputData>();
            _playerInput = inputData.playerInput;
        }

        public void ShowEscapeMenu()
        {
            _escapeMenuPanel.SetActive(true);
            _firstButton.Select();
            
            _gameStateManager.SetGameState(GameStates.Menu);
        }
        
        public void HideEscapeMenu()
        {
            _escapeMenuPanel.SetActive(false);
            
            _gameStateManager.SetGameState(GameStates.Gameplay);
        }
        
        public void ExitToMainMenu()
        {
            _gameManager.StartMainMenu();
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