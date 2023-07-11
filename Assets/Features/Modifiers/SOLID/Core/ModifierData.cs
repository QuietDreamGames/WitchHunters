using System;

namespace Features.Modifiers.SOLID.Core
{
    [Serializable]
    public class ModifierData
    {
        public float modificatorValue;
        public float duration;
        public ModifierSpec spec;
        
        public ModifierData(float modificatorValue, float duration, ModifierSpec spec)
        {
            this.modificatorValue = modificatorValue;
            this.duration = duration;
            this.spec = spec;
        }
    }
}