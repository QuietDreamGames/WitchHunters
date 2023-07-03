using System;
using System.Collections.Generic;

namespace Features.Modifiers.SOLID.Core
{
    public class ModificatorContainer
    {
        private readonly Dictionary<ModifierType, Modificator> modificators = new();
        
        private void GetModificator(ModifierType type, out Modificator modificator)
        {
            if (!modificators.TryGetValue(type, out modificator))
            {
                modificator = new Modificator();
                modificators.Add(type, modificator);
            }
        }

        public void Add(ModifierType type, ModifierSpec spec, float duration, float modificatorValue)
        {
            GetModificator(type, out var modificator);
            modificator.Add(duration, modificatorValue, spec);
        }
        
        public float GetValue(ModifierType type, float sourceValue)
        { 
            GetModificator(type, out var modificator);
            return modificator.GetValue(sourceValue);
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var (_, value) in modificators)
            {
                value.OnUpdate(deltaTime);
            }
        }
    }
}