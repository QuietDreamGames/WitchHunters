using System.Collections;
using System.Collections.Generic;
using Features.Input;
using Features.ServiceLocators.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class TestData : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = ServiceLocator.Resolve<InputData>().playerInput;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_playerInput.actions["Move"].ReadValue<Vector2>());
        
    }
}
