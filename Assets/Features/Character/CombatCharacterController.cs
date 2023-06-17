using Features.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public abstract class CombatCharacterController : MonoBehaviour
    {
        [SerializeField] protected Collider2D _attackCollider;

        [SerializeField] protected PlayerInput _playerInput;
        [SerializeField] protected CharacterView _characterView;

        protected StateMachine _stateMachine;
        
        protected virtual void Start()
        {
            _stateMachine = new StateMachine();
            
            _stateMachine.AddExtension(_playerInput);
            _stateMachine.AddExtension(_characterView);
            _stateMachine.AddExtension(transform);
            _stateMachine.AddExtension(_attackCollider);
            
            
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