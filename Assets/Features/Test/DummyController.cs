using Features.Modifiers;
using UnityEngine;

namespace Features.Test
{
    public class DummyController : MonoBehaviour
    {
        [SerializeField] protected ModifiersController _modifiersController;
        [SerializeField] private ModifierInfo[] _baseModifiersList;
        
        private void Awake()
        {
            for (int i = 0; i < _baseModifiersList.Length; i++)
            {
                _modifiersController.AddModifier(_baseModifiersList[i]);    
            }
        }
    }
}
