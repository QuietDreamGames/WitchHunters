using Features.InputSystem.Services;
using Unity.Entities;

namespace Features.InputSystem.Components
{
    public class CharacterInput : IComponentData
    {
        public InputInterpreter Value;

        public CharacterInput()
        {
            Value = new InputInterpreter();
        }

        public CharacterInput(InputInterpreter interpreter)
        {
            Value = interpreter;
        }
    }
}