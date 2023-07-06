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

        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        private CharacterView _characterView;

        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer, CharacterView characterView)
        {
            _characterView = characterView;
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
        }
        
        public void CastUltimate()
        {
            var direction = _characterView.GetLastMovementDirection();
            // var ultimate = Instantiate(_ultimatePrefab, transform.position, Quaternion.identity);
            _ultimate.Cast(direction, _modifiersContainer, _baseModifiersContainer);
        }

        public void CastSecondary()
        {
            var direction = _characterView.GetLastMovementDirection();
            _secondary.Cast(direction, _modifiersContainer, _baseModifiersContainer);
        }
    }
}