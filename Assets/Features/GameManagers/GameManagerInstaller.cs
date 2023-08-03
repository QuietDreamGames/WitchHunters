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
            ServiceLocator.Register<GameManager>(gameManager);
        }
    }
}
