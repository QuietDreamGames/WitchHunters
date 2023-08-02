using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Skills.Core
{
    public abstract class ASkill : MonoBehaviour, IUpdateHandler, IFixedUpdateHandler
    {
        protected ModifiersContainer ModifiersContainer;
        protected BaseModifiersContainer BaseModifiersContainer;
        
        protected float _currentCooldown;
        protected float _maxCooldown;
        
        public float CurrentCooldown => _currentCooldown;
        public float MaxCooldown => _maxCooldown;
        public bool IsOnCooldown => _currentCooldown > 0f;
        
        public virtual void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            ModifiersContainer = modifiersContainer;
            BaseModifiersContainer = baseModifiersContainer;
        }
        
        public abstract void Cast(Vector3 direction);

        public virtual void OnUpdate(float deltaTime)
        {
            
        }
        
        public virtual void OnFixedUpdate(float deltaTime)
        {
            
        }
    }
}