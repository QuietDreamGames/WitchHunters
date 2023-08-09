using UnityEngine;

namespace Features.Experience
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelsData _levelsData;
        
        private int _currentLevel;
        
        private ExperienceController _experienceController;
        
        // Should be called after the initiation of the ExperienceController
        public void Initiate(ExperienceController experienceController)
        {
            _experienceController = experienceController;
            _currentLevel = _levelsData.CalculateLevel(_experienceController.ExpAmount);
        }
        
        
    }
}