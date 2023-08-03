using Features.ColliderController.Core;
using Features.Damage.Core;
using Features.FiniteStateMachine;
using Features.Health;
using Features.Input;
using Features.Knockback;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ServiceLocators.Core;
using Features.Skills.Core;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX;
using Features.VFX.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public abstract class CombatCharacterController : MonoBehaviour, IUpdateHandler, IFixedUpdateHandler, ILateUpdateHandler
    {
        [SerializeField] protected Collider2D _attackCollider;
        [SerializeField] protected Rigidbody2D _rigidbody;

        
        [SerializeField] protected CharacterView _characterView;
        [SerializeField] protected BaseModifiersContainer _baseModifiersContainer;
        [SerializeField] protected SkillsController _skillsController;
        [SerializeField] protected CharacterDamageController _damageController;
        [SerializeField] protected MeleeColliderController _meleeColliderController;
        [SerializeField] protected APassiveController _passiveController;
        [SerializeField] protected KnockbackController _knockbackController;

        [SerializeField] protected ShieldEffectController _shieldEffectController;
        
        public ModifiersContainer ModifiersContainer { get; protected set; }
        public HealthComponent HealthComponent { get; protected set; }
        public ShieldHealthController ShieldHealthController { get; protected set; }
        
        public BaseModifiersContainer BaseModifiersContainer => _baseModifiersContainer;
        public APassiveController PassiveController => _passiveController;
        public SkillsController SkillsController => _skillsController;
        
        protected PlayerInput _playerInput;
        protected StateMachine stateMachine;
        
        public virtual void Initiate()
        {
            var inputData = ServiceLocator.Resolve<InputData>();
            _playerInput = inputData.playerInput;
            stateMachine = new StateMachine();
            ModifiersContainer = new ModifiersContainer();
            HealthComponent = new HealthComponent(ModifiersContainer, _baseModifiersContainer);
            ShieldHealthController = new ShieldHealthController(ModifiersContainer, _baseModifiersContainer);
            _knockbackController.Initiate(ModifiersContainer, _baseModifiersContainer);
            
            _damageController.Initiate(ModifiersContainer,  _baseModifiersContainer, HealthComponent, stateMachine, ShieldHealthController);
            _damageController.SetActive(true);

            stateMachine.AddExtension(_playerInput);
            stateMachine.AddExtension(_characterView);
            stateMachine.AddExtension(_rigidbody);
            stateMachine.AddExtension(_attackCollider);
            stateMachine.AddExtension(ModifiersContainer);
            stateMachine.AddExtension(_baseModifiersContainer);
            stateMachine.AddExtension(HealthComponent);
            stateMachine.AddExtension(ShieldHealthController);
            

            _skillsController.Initiate(ModifiersContainer, _baseModifiersContainer, _characterView);
            stateMachine.AddExtension(_skillsController);
            
            _passiveController.Initiate(ModifiersContainer, _baseModifiersContainer);
            stateMachine.AddExtension(_passiveController);
            
            _shieldEffectController.Initiate();
            stateMachine.AddExtension(_shieldEffectController);
        }

        public void Restart()
        {
            _damageController.Restart();
        }

        public void OnUpdate(float deltaTime)
        {
            stateMachine.OnUpdate(deltaTime);
            ModifiersContainer.OnUpdate(deltaTime);
            _passiveController.OnUpdate(deltaTime);
            _shieldEffectController.OnUpdate(deltaTime);
            ShieldHealthController.OnUpdate(deltaTime);
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