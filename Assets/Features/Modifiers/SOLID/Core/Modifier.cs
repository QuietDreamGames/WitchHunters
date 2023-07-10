using System;
using System.Collections.Generic;
using Features.Modifiers.SOLID.Builders;
using UnityEngine;

namespace Features.Modifiers.SOLID.Core
{
    public class Modifier
    {
        private readonly List<ModifierData> data = new();
        
        public float GetValue(float sourceValue)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentData = data[i];
                sourceValue = ModificatorSpecBuilder.GetValue(
                    sourceValue,
                    currentData.modificatorValue,
                    currentData.spec);
            }

            return sourceValue;
        }
        
        public void Add(float duration, float modificatorValue, ModifierSpec spec)
        {
            data.Add(new ModifierData(modificatorValue, duration, spec));
        }
        
        public bool OnUpdate(float deltaTime)
        {
            var isDirty = false;
            for (var i = 0; i < data.Count; i++)
            {
                var currentData = data[i];
                currentData.duration -= deltaTime;

                if (currentData.duration <= 0)
                {
                    data.RemoveAt(i);
                    i--;
                    isDirty = true;
                }
            }

            return isDirty;
        }
    }
}