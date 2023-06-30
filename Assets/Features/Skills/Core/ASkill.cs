using Features.Modifiers;
using UnityEngine;

namespace Features.Skills.Core
{
    public abstract class ASkill : MonoBehaviour
    {
        public abstract void Cast(Vector3 direction, ModifiersController modifiersController);
    }
}