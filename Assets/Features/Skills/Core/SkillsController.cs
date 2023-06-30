using Features.Character;
using Features.Modifiers;
using UnityEngine;

namespace Features.Skills.Core
{
    public class SkillsController : MonoBehaviour
    {
        [SerializeField] private ASkill _ultimate;
        [SerializeField] private ASkill _secondary;

        private ModifiersController _modifiersController;
        private CharacterView _characterView;

        public void Initiate(ModifiersController modifiersController, CharacterView characterView)
        {
            _characterView = characterView;
            _modifiersController = modifiersController;
        }
        
        public void CastUltimate()
        {
            var direction = _characterView.GetLastMovementDirection();
            // var ultimate = Instantiate(_ultimatePrefab, transform.position, Quaternion.identity);
            _ultimate.Cast(direction, _modifiersController);
        }

        public void CastSecondary()
        {
            var direction = _characterView.GetLastMovementDirection();
            _secondary.Cast(direction, _modifiersController);
        }
    }
}