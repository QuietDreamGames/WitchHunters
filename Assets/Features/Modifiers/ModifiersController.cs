using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Modifiers
{
    public class ModifiersController : MonoBehaviour
    {
        private List<ModifierInfo> _modifiers = new List<ModifierInfo>();

        public Action<ModifierType> OnModifierChanged;
        
        public void AddModifier(ModifierInfo modifier)
        {
            _modifiers.Add(modifier);

            OnModifierChanged?.Invoke(modifier.Type);
        }
        
        public void RemoveModifier(ModifierInfo modifier)
        {
            _modifiers.Remove(modifier);
            OnModifierChanged?.Invoke(modifier.Type);
        }
        
        public float CalculateModifiedValue(ModifierType type)
        {
            float value = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].Type != type)
                {
                    continue;
                }
                if (_modifiers[i].Spec == ModifierSpec.RawAdditional)
                {
                    value += _modifiers[i].Value;
                }
            }
            
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].Type != type)
                {
                    continue;
                }
                if (_modifiers[i].Spec == ModifierSpec.PercentageAdditional)
                {
                    value += value * _modifiers[i].Value;
                }
            }
            
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].Type != type)
                {
                    continue;
                }
                if (_modifiers[i].Spec == ModifierSpec.PercentageMultiplicative)
                {
                    value *= _modifiers[i].Value;
                }
            }

            return value;
        }

        public ModifierInfo GetModifierInfo(ModifierType type)
        {
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].Type == type)
                {
                    return _modifiers[i];
                }
            }

            return null;
        }

        public void OnUpdate()
        {
            var modifiersToRemove = new List<ModifierInfo>();
            
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].TimeType != ModifierTimeType.Temporary) continue;
                
                _modifiers[i].Duration -= Time.deltaTime;
                if (_modifiers[i].Duration <= 0)
                {
                    modifiersToRemove.Add(_modifiers[i]);
                }
            }
            
            for (int i = 0; i < modifiersToRemove.Count; i++)
            {
                RemoveModifier(modifiersToRemove[i]);
            }
        }
    }
}