using Features.Activator;
using Features.Activator.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Progression
{
    public class LevelCompleteInvoker : MonoBehaviour, IActivator
    {
        public void Activate()
        {
            var gameProgression = ServiceLocator.Resolve<GameProgression>();
            gameProgression.InvokeComplete();
        }

        public void Deactivate()
        {
            
        }
    }
}