using Features.Character;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine;

namespace Features.Skills.Core
{
    public class SkillsController : MonoBehaviour
    {
        [SerializeField] private ASkill _ultimate;
        [SerializeField] private ASkill _secondary;
        
        public ASkill Ultimate => _ultimate;
        public ASkill Secondary => _secondary;

        private CharacterView _characterView;

        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer, CharacterView characterView)
        {
            _characterView = characterView;
            _ultimate.Initiate(modifiersContainer, baseModifiersContainer);
            _secondary.Initiate(modifiersContainer, baseModifiersContainer);
        }
        
        public void CastUltimate()
        {
            var direction = _characterView.GetLastMovementDirection();
            _ultimate.Cast(direction);
        }

        public void CastSecondary()
        {
            var direction = _characterView.GetLastMovementDirection();
            _secondary.Cast(direction);
        }
    }
}