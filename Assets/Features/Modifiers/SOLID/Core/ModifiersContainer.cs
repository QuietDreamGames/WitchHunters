using System;
using System.Collections.Generic;
using Features.Modifiers.SOLID.Helpers;

namespace Features.Modifiers.SOLID.Core
{
    public class ModifiersContainer
    {
        public Action<ModifierType> OnUpdateModifier;
        private readonly Dictionary<ModifierType, Modifier> modificators = new();
        
        private void GetModificator(ModifierType type, out Modifier modifier)
        {
            if (!modificators.TryGetValue(type, out modifier))
            {
                modifier = new Modifier();
                modificators.Add(type, modifier);
            }
        }

        public void Add(ModifierType type, ModifierSpec spec, float duration, float modificatorValue)
        {
            GetModificator(type, out var modificator);
            modificator.Add(duration, modificatorValue, spec);
            OnUpdateModifier?.Invoke(type);
        }
        
        public void Add(ModifierInfo modInfo)
        {
            Add(modInfo.type, modInfo.data.spec, modInfo.data.duration, modInfo.data.modificatorValue);
        }
        
        public float GetValue(ModifierType type, float sourceValue)
        { 
            GetModificator(type, out var modificator);
            return modificator.GetValue(sourceValue);
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var (type, value) in modificators)
            {
                if (value.OnUpdate(deltaTime))
                    OnUpdateModifier?.Invoke(type);
            }
        }
    }
}