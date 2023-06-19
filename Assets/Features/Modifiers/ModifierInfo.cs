using System;

namespace Features.Modifiers
{
    [Serializable]
    public class ModifierInfo
    {
        public ModifierType Type;
        public ModifierSpec Spec;
        public ModifierTimeType TimeType;
        public float Value;
        public float Duration;
        public float MaxDuration; // for UI, should be the same as Duration but should not change
    }
}