using System;

namespace Features.Modifiers.SOLID.Core
{
    [Serializable]
    public struct ModificatorData
    {
        public float modificatorValue;
        public float duration;
        public ModifierSpec spec;
        
        public ModificatorData(float modificatorValue, float duration, ModifierSpec spec)
        {
            this.modificatorValue = modificatorValue;
            this.duration = duration;
            this.spec = spec;
        }
    }
}