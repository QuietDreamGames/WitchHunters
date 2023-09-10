using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Test
{
    public class TestSkillPoints : MonoBehaviour
    {
        public void AddSkillPoints()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            
            character.StatsController.AddUnusedStatsPoints(3);
        }
        
        public void AddTalentPoints()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            
            character.TalentsController.AddTalentPoint();
        }
        
        public void AddExp()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            
            character.LevelController.AddExperience(50);
        }
    }
}