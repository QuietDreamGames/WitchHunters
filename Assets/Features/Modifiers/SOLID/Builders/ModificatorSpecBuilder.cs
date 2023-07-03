using System.Diagnostics.Contracts;
using UnityEngine;

namespace Features.Modifiers.SOLID.Builders
{
    public static class ModificatorSpecBuilder
    {
        [Pure]
        public static float GetValue(float sourceValue, float modificatorValue, ModifierSpec spec)
        {
            switch (spec)
            {
                case ModifierSpec.RawAdditional:
                    return sourceValue + modificatorValue;
                case ModifierSpec.PercentageAdditional:
                    return sourceValue + (sourceValue * modificatorValue);
                case ModifierSpec.PercentageMultiplicative:
                    return sourceValue * modificatorValue;
                default:
                    Debug.LogError($"Unknown modifier spec: {spec}");
                    return sourceValue;
            }
        }
    }
}