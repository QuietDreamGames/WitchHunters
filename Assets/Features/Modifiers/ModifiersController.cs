using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Modifiers
{
    public class ModifiersController : MonoBehaviour
    {
        private List<ModifierInfo> _modifiers = new List<ModifierInfo>();
        private List<(ModifierInfo, float)> _timedModifiers = new List<(ModifierInfo, float)>();
        
        public Action<ModifierType> OnModifierChanged;
        
        public void AddModifier(ModifierInfo modifier)
        {
            _modifiers.Add(modifier);
            
            if (modifier.TimeType == ModifierTimeType.Temporary)
            {
                _timedModifiers.Add((modifier, modifier.Duration));
            }
            
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

        public void OnUpdate()
        {
            for (int i = 0; i < _timedModifiers.Count; i++)
            {
                _timedModifiers[i] = (_timedModifiers[i].Item1, _timedModifiers[i].Item2 - Time.deltaTime);
            }

            for (int i = 0; i < _timedModifiers.Count; i++)
            {
                if (_timedModifiers[i].Item2 <= 0)
                {
                    _modifiers.Remove(_timedModifiers[i].Item1);
                    _timedModifiers.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}