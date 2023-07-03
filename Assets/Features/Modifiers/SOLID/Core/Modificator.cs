using System.Collections.Generic;
using Features.Modifiers.SOLID.Builders;

namespace Features.Modifiers.SOLID.Core
{
    public class Modificator
    {
        private readonly List<ModificatorData> data = new();

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
            data.Add(new ModificatorData(modificatorValue, duration, spec));
        }
        
        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentData = data[i];
                currentData.duration -= deltaTime;
                if (currentData.duration <= 0)
                {
                    data.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}