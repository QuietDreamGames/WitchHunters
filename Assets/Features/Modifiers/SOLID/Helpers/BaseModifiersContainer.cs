using UnityEngine;

namespace Features.Modifiers.SOLID.Helpers
{
    [CreateAssetMenu(fileName = "BaseModifiersContainer", menuName = "Modifiers/BaseModifiersContainer")]
    public class BaseModifiersContainer : ScriptableObject 
    {
        public ModifierInfo[] baseModifiers;

        public float GetBaseValue(ModifierType type)
        {
            for (int i = 0; i < baseModifiers.Length; i++)
            {
                if (baseModifiers[i].type == type)
                {
                    return baseModifiers[i].data.modificatorValue;
                }
            }

            Debug.LogError("No base value of type: " + type);
            return 0;
        }
    }
}