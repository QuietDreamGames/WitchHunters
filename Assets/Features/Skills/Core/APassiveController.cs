using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine;

namespace Features.Skills.Core
{
    public abstract class APassiveController : MonoBehaviour
    {
        protected BaseModifiersContainer baseModifiersContainer;
        protected ModifiersContainer modifiersContainer;

		public CurrentPassiveInfo CurrentPassiveInfo { get; protected set; }

        public virtual void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            this.modifiersContainer = modifiersContainer;
            this.baseModifiersContainer = baseModifiersContainer;

            CurrentPassiveInfo = new CurrentPassiveInfo();
        }

		public virtual void OnUpdate(float deltaTime){}

		public virtual void OnHit(){}
    }
}