using System;
using System.Collections.Generic;
using Features.Character.States;
using Features.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public class CombatCharacterController : MonoBehaviour
    {
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private Collider2D _attackCollider;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CharacterView _characterView;

        private void Start()
        {
            var extensions = new Dictionary<Type, Component>
            {
                {typeof(PlayerInput), _playerInput},
                {typeof(CharacterView), _characterView},
                {typeof(Transform), transform},
                {typeof(Collider2D), _attackCollider}
            };
            _stateMachine.Initialize(new IdleCombatState(), extensions);
        }

        private void Update()
        {
            if (_playerInput.actions["Attack"].IsPressed())
            {
                if (_stateMachine.CurrentState is IdleCombatState or MoveState)
                {
                    _stateMachine.ChangeNextState(new MeleeEntryState());
                }
            }
            
            if (_stateMachine.CurrentState is not IdleCombatState) return;
            
            if (_playerInput.actions["Move"].IsPressed())
            {
                _stateMachine.ChangeNextState(new MoveState());
            }
            
        }
    }
}