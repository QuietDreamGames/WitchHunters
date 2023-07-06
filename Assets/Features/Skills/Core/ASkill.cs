using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine;

namespace Features.Skills.Core
{
    public abstract class ASkill : MonoBehaviour
    {
        public abstract void Cast(Vector3 direction, ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer);
    }
}