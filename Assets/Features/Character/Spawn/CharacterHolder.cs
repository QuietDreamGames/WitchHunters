using System;

namespace Features.Character.Spawn
{
    public class CharacterHolder
    {
        public Action<CombatCharacterController> OnCharacterChanged;
        public CombatCharacterController CurrentCharacter { get; private set; }
        
        public void SetCurrentCharacter(CombatCharacterController character)
        {
            CurrentCharacter = character;
            OnCharacterChanged?.Invoke(character);
        }
    }
}