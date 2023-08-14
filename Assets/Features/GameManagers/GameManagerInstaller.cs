using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.GameManagers
{
    public class GameManagerInstaller : ServiceInstaller
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private CursorManager cursorManager;
        
        [Header("Parameters")]
        [SerializeField] private GameStates defaultState;
        
        public override void Install()
        {
            var existCursorManager = ServiceLocator.Resolve<CursorManager>() != null;
            var existGameStateManager = ServiceLocator.Resolve<GameStateManager>() != null;
            var existGameManager = ServiceLocator.Resolve<GameManager>() != null;
            
            if (existCursorManager && existGameStateManager && existGameManager)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ServiceLocator.Register<CursorManager>(cursorManager);
            
            ServiceLocator.Register<GameStateManager>(gameStateManager);
            gameStateManager.Construct();
            gameStateManager.SetGameState(defaultState);
        
            ServiceLocator.Register<GameManager>(gameManager);
        
            gameManager.transform.parent = null;
            DontDestroyOnLoad(gameManager);
        }
    }
}
