using System.Collections.Generic;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;

namespace Features.ColliderController.Interfaces
{
    public interface IMeleeDamageProcessor
    {
        List<Collider2D> ProcessMeleeDamage(Collider2D collider2D, List<Collider2D> collidersDamaged, Transform transform,  ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, APassiveController passiveController);
    }
}