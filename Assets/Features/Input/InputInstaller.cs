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
            var existInputData = ServiceLocator.Resolve<InputData>() != null;
            if (existInputData)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ServiceLocator.Register<InputData>(inputData);
            
            // TODO: Fix this hack
            inputData.playerInput.enabled = false;
            inputData.playerInput.enabled = true;
            
            inputData.transform.parent = null;
            DontDestroyOnLoad(inputData);
        }
    }
}