using Features.Activator.Core;
using Features.GameManagers;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Activator
{
    public class GameplaySceneActivator : MonoBehaviour, IActivator
    {
        [SerializeField] private string sceneName;
        
        public void Activate()
        {
            var gameManager = ServiceLocator.Resolve<GameManager>();
            gameManager.StartGameplayScene(sceneName);
        }

        public void Deactivate()
        {
            
        }
    }
}
