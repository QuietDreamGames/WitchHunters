using System;
using Features.Damage.Core;
using Features.Health;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ObjectPools.Core;
using UnityEngine;

namespace Features.Enemies
{
    public class UnitBehaviour : MonoBehaviour
    {
        [Header("State Machine")] 
        [SerializeField] private UnitStateMachine stateMachine;
        
        [Header("Shader")]
        [SerializeField] private UnitShaderController shaderController;

        [Header("Damageable")]
        [SerializeField] protected BaseModifiersContainer baseModifiersContainer;
        [SerializeField] protected DamageController damageController;
        
        [Header("Type")]
        [SerializeField] private UnitType type;
        [SerializeField] private int level;

        [Header("DEBUG")] 
        [SerializeField] private bool spawnOnStart;

        private ModifiersContainer _modifiersContainer;
        private HealthComponent _healthComponent;
        
        public GameObjectPool<UnitBehaviour> Pool { get; set; }
        public GameObject Prefab { get; set; }
        
        public UnitType Type => type;
        public int Level => level;
        
        public bool IsEnabled => gameObject.activeSelf;

        public Action OnDeath { get; private set; }

        private void Start()
        {
            if (spawnOnStart)
            {
                Spawn();
            }
        }

        public void Spawn()
        {
            ConfigureShader();

            ConfigureDamageable();
            
            ConfigureStateMachine();
        }

        public void Despawn()
        {
            shaderController.FadeOut(true);
            
            damageController.SetActive(false);

            stateMachine.SetActive(false);

            if (Pool != null)
            {
                Pool.Despawn(Prefab, this);
            }
            
            OnDeath = null;
        }
        
        private void ConfigureShader()
        {
            shaderController.FadeOut(true);
            shaderController.FadeIn();
        }

        private void ConfigureDamageable()
        {
            _modifiersContainer = new ModifiersContainer();
            _healthComponent = new HealthComponent(_modifiersContainer, baseModifiersContainer);
            
            damageController.Initiate(_modifiersContainer, baseModifiersContainer, _healthComponent);
            damageController.SetActive(true);
            damageController.Restart();

            _healthComponent.OnHit = OnHitHandle;
            _healthComponent.OnDeath = OnDeathHandle;
        }

        private void ConfigureStateMachine()
        {
            stateMachine.SetActive(true);
            stateMachine.SetPresentationState();

            stateMachine.OnDeathExitHandle = Despawn;
        }

        private void OnHitHandle()
        {
            shaderController.Hit();
        }

        private void OnDeathHandle()
        {
            stateMachine.SetDeathState();
            
            shaderController.FadeOut();
            
            damageController.SetActive(false);
            
            OnDeath?.Invoke();
        }
    }
}
