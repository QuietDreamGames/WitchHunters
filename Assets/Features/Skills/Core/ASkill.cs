using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine;

namespace Features.Skills.Core
{
    public abstract class ASkill : MonoBehaviour
    {
        protected ModifiersContainer ModifiersContainer;
        protected BaseModifiersContainer BaseModifiersContainer;
        
        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            ModifiersContainer = modifiersContainer;
            BaseModifiersContainer = baseModifiersContainer;
        }
        
        public abstract void Cast(Vector3 direction);
    }
}