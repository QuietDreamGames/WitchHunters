using System;
using Features.Character.Spawn;
using Features.Modifiers;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Test
{
    public class ModifiersTest : MonoBehaviour
    {
        private void Start()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            character.ModifiersContainer.Add(ModifierType.MoveSpeed, ModifierSpec.PercentageAdditional,
                float.PositiveInfinity, 2f);
        }
    }
}