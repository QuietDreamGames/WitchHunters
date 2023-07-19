using Features.ColliderController.Core;
using Features.Damage.Core;
using Features.FiniteStateMachine;
using Features.Health;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public abstract class CombatCharacterController : MonoBehaviour, IUpdateHandler, IFixedUpdateHandler, ILateUpdateHandler
    {
        [SerializeField] protected Collider2D _attackCollider;

        [SerializeField] protected PlayerInput _playerInput;
        [SerializeField] protected CharacterView _characterView;
        [SerializeField] protected BaseModifiersContainer _baseModifiersContainer;
        [SerializeField] protected SkillsController _skillsController;
        [SerializeField] protected CharacterDamageController _damageController;
        [SerializeField] protected MeleeColliderController _meleeColliderController;
        [SerializeField] protected APassiveController _passiveController;
        
        [SerializeField] protected ShieldEffectController _shieldEffectController;
        
        protected ModifiersContainer modifiersContainer;
        protected StateMachine stateMachine;
        protected HealthComponent healthComponent;
        protected ShieldHealthController shieldHealthController;
        
        protected virtual void Start()
        {
            stateMachine = new StateMachine();
            modifiersContainer = new ModifiersContainer();
            healthComponent = new HealthComponent(modifiersContainer, _baseModifiersContainer);
            shieldHealthController = new ShieldHealthController(modifiersContainer, _baseModifiersContainer);
            
            _damageController.Initiate(modifiersContainer, _baseModifiersContainer, healthComponent, shieldHealthController);
            _damageController.SetActive(true);

            stateMachine.AddExtension(_playerInput);
            stateMachine.AddExtension(_characterView);
            stateMachine.AddExtension(transform);
            stateMachine.AddExtension(_attackCollider);
            stateMachine.AddExtension(modifiersContainer);
            stateMachine.AddExtension(_baseModifiersContainer);
            stateMachine.AddExtension(healthComponent);
            stateMachine.AddExtension(shieldHealthController);
            

            _skillsController.Initiate(modifiersContainer, _baseModifiersContainer, _characterView);
            stateMachine.AddExtension(_skillsController);
            
            _passiveController.Initiate(modifiersContainer, _baseModifiersContainer);
            stateMachine.AddExtension(_passiveController);
            
            _shieldEffectController.Initiate();
            stateMachine.AddExtension(_shieldEffectController);
        }

        public void OnUpdate(float deltaTime)
        {
            stateMachine.OnUpdate(deltaTime);
            modifiersContainer.OnUpdate(deltaTime);
            _passiveController.OnUpdate(deltaTime);
            _shieldEffectController.OnUpdate(deltaTime);
            shieldHealthController.OnUpdate(deltaTime);
            _damageController.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            stateMachine.OnFixedUpdate(deltaTime);
            _meleeColliderController.OnFixedUpdate(deltaTime);
        }

        public void OnLateUpdate(float deltaTime)
        {
            stateMachine.OnLateUpdate(deltaTime);
        }
    }
}