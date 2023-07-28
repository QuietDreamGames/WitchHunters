using System;
using UnityEngine;

namespace Features.Activator
{
    public class GroupActivator : MonoBehaviour
    {
        [SerializeField] private ActivatorData[] activators;
        
        public void Activate()
        {
            for (var i = 0; i < activators.Length; i++)
            {
                var activator = activators[i];
                activator.Activate();
            }
        }
    }

    [Serializable]
    public struct ActivatorData
    {
        public MonoBehaviour activator;
        public ActivatorType type;
        
        public void Activate()
        {
            var activator = this.activator as IActivator;
            if (activator == null)
            {
                Debug.LogError($"Activator {activator} is not IActivator");
                return;
            }
            
            switch (type)
            {
                case ActivatorType.Activate:
                    activator.Activate();
                    break;
                case ActivatorType.Deactivate:
                    activator.Deactivate();
                    break;
                default:
                    return;
            }
        }
    }
}
