using Features.Character.Spawn;
using Features.Experience;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Test
{
    public class CharacterExperienceTestSetter : MonoBehaviour
    {
        [SerializeField] private int expAmount;

        public void OnAddExp()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            
            var experienceController = character.ExperienceController;
            
            experienceController.AddExp(expAmount);
        }
        
        public void OnResetExp()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            
            var experienceController = character.ExperienceController;
            
            experienceController.SetExp(0);
        }
    }
}