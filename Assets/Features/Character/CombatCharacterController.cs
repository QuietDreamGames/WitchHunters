using Features.FiniteStateMachine;
using Features.Modifiers;
using Features.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public abstract class CombatCharacterController : MonoBehaviour
    {
        [SerializeField] protected Collider2D _attackCollider;

        [SerializeField] protected PlayerInput _playerInput;
        [SerializeField] protected CharacterView _characterView;
        [SerializeField] protected ModifiersController _modifiersController;
        [SerializeField] protected SkillsController _skillsController;
        [SerializeField] private ModifierInfo[] _baseModifiersList;

        protected StateMachine _stateMachine;
        
        protected virtual void Start()
        {
            _stateMachine = new StateMachine();
            
            _stateMachine.AddExtension(_playerInput);
            _stateMachine.AddExtension(_characterView);
            _stateMachine.AddExtension(transform);
            _stateMachine.AddExtension(_attackCollider);
            _stateMachine.AddExtension(_modifiersController);

            for (int i = 0; i < _baseModifiersList.Length; i++)
            {
                _modifiersController.AddModifier(_baseModifiersList[i]);    
            }
            
            _skillsController.Initiate(_modifiersController, _characterView);
            _stateMachine.AddExtension(_skillsController);
        }

        private void Update()
        {
            _stateMachine.OnUpdate(Time.deltaTime);
            _modifiersController.OnUpdate();
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