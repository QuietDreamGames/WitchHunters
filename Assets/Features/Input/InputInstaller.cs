using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input
{
    public class InputInstaller : ServiceInstaller
    {
        [SerializeField] private PlayerInput _playerInput;


        public override void Install()
        {
            Debug.Log(1);
            ServiceLocator.Register(_playerInput);
            Debug.Log(2);
        }
    }
}