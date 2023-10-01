using Features.Activator;
using Features.Activator.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Progression
{
    public class NextLevelSetter : MonoBehaviour, IActivator
    {
        public void Activate()
        {
            var gameProgression = ServiceLocator.Resolve<GameProgression>();
            var (dungeonID, floorID) = gameProgression.GetCurrent();
            gameProgression.SetCurrent(dungeonID, floorID + 1);
        }

        public void Deactivate()
        {
            
        }
    }
}
