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
    }
}