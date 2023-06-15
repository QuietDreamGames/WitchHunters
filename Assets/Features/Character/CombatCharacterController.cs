using Features.Character.States;
using Features.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public class CombatCharacterController : MonoBehaviour
    {
        [SerializeField] private Collider2D _attackCollider;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CharacterView _characterView;

        private StateMachine _stateMachine;
        
        private void Start()
        {
            _stateMachine = new StateMachine();
            
            _stateMachine.AddExtension(_playerInput);
            _stateMachine.AddExtension(_characterView);
            _stateMachine.AddExtension(transform);
            _stateMachine.AddExtension(_attackCollider);
            
            _stateMachine.AddState("IdleCombatState", new IdleCombatState(_stateMachine));
            _stateMachine.AddState("MoveState", new MoveState(_stateMachine));
            _stateMachine.AddState("MeleeEntryState", new MeleeEntryState(_stateMachine));
            _stateMachine.AddState("MeleeComboEntryState", new MeleeComboEntryState(_stateMachine));
            _stateMachine.AddState("MeleeComboState", new MeleeComboState(_stateMachine));
            _stateMachine.AddState("MeleeFinisherState", new MeleeFinisherState(_stateMachine));
            
            _stateMachine.ChangeState("IdleCombatState");
        }

        private void Update()
        {
            _stateMachine.OnUpdate(Time.deltaTime);
            
        }

        private void FixedUpdate()
        {
            _stateMachine.OnFixedUpdate(Time.fixedDeltaTime);
        }
        
        private void LateUpdate()
        {
            _stateMachine.OnLateUpdate(Time.deltaTime);
        }
    }
}