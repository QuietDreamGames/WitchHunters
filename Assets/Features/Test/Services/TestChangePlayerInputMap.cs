using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Test.Services
{
    public class TestChangePlayerInputMap : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;

        private void Start()
        {
            InvokeRepeating(nameof(ChangePlayerInputMap), 0, 5f);
        }

        private void ChangePlayerInputMap()
        {
            _playerInput.SwitchCurrentActionMap(_playerInput.currentActionMap.name == "Player" ? "UI" : "Player");
        }
    }
}
