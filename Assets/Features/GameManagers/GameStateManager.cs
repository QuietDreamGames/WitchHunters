using System;
using Features.Input;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Core;
using UnityEngine;

namespace Features.GameManagers
{
    public class GameStateManager : MonoBehaviour
    {
        private TimeSystem _timeSystem;
        private InputData _inputData;
        private CursorManager _cursorManager;
        
        private GameStates _currentGameState;
        private GameStates _previousGameState;

        public void Construct()
        {
            _timeSystem = ServiceLocator.Resolve<TimeSystem>();
            _inputData = ServiceLocator.Resolve<InputData>();
            _cursorManager = ServiceLocator.Resolve<CursorManager>();
            
            SetGameState(GameStates.None);
        }
        
        public void SetGameState(GameStates gameState)
        {
            switch (gameState)
            {
                case GameStates.None:
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.Gameplay, 0);
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.UI, 0);
                    
                    _inputData.playerInput.SwitchCurrentActionMap(Constants.InputActionMap.Gameplay);
                    
                    _cursorManager.SetVisible(false);   
                    break;
                case GameStates.Gameplay:
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.Gameplay, 1);
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.UI, 1);
                    
                    _inputData.playerInput.SwitchCurrentActionMap(Constants.InputActionMap.Gameplay);
                    
                    _cursorManager.SetVisible(false);   
                    break;
                case GameStates.Menu:
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.Gameplay, 0);
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.UI, 1);
                    
                    _inputData.playerInput.SwitchCurrentActionMap(Constants.InputActionMap.UI);
                    
                    _cursorManager.SetVisible(true);
                    break;
                case GameStates.Dialog:
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.Gameplay, 1);
                    _timeSystem.SetCategoryTimeScale(Constants.TimeCategory.UI, 1);
                    
                    _inputData.playerInput.SwitchCurrentActionMap(Constants.InputActionMap.UI);
                    
                    _cursorManager.SetVisible(true);
                    break;
                default:
                    Debug.LogError($"Unknown game state: {gameState}");
                    break;
            }
            
            _previousGameState = _currentGameState;
            _currentGameState = gameState;
        }
        
        public void SetPreviousGameState()
        {
            SetGameState(_previousGameState);
        }
    }
}
