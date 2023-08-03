using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input
{
    public class InputInstaller : ServiceInstaller
    {
        [SerializeField] private InputData inputData;
        
        public override void Install()
        {
            ServiceLocator.Register<InputData>(inputData);
        }
    }
}