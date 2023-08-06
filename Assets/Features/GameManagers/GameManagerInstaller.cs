using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.GameManagers
{
    public class GameManagerInstaller : ServiceInstaller
    {
        [SerializeField] private GameManager gameManager;
        
        public override void Install()
        {
            var exist = ServiceLocator.Resolve<GameManager>() != null;
            if (exist)
            {
                return;
            }
            
            ServiceLocator.Register<GameManager>(gameManager);
            
            gameManager.transform.parent = null;
            DontDestroyOnLoad(gameManager);
        }
    }
}
