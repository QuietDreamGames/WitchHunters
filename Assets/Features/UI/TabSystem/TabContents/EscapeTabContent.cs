using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents
{
    public class EscapeTabContent : TabContent
    {
        [SerializeField] private Button _firstButton;
        
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        
        private float _lastGameplayTimeScale;

        private bool _isInEscapeMenu;
        
        public override void OnSelect()
        {
            base.OnSelect();
            _firstButton.Select();
        }
        
        public void ExitToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        // public void OnUpdate(float deltaTime)
        // {
        //     if (!_playerInput.actions["Escape"].WasPressedThisFrame()) return;
        //
        //     if (_playerInput.currentActionMap.name == _playerActionMapName)
        //     {
        //         ShowEscapeMenu();
        //         
        //     }
        //     else
        //     {
        //         HideEscapeMenu();
        //         
        //     }
        // }
    }
}