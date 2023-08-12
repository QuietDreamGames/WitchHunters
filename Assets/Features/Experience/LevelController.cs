using System;
using UnityEngine;

namespace Features.Experience
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private ExperienceController _experienceController;        
        [SerializeField] private LevelsData _levelsData;
        
        private int _currentLevel;
        public int CurrentLevel => _currentLevel;
        
        public Action OnLevelUp;
        public Action OnAddExp;
        
        
        // Should be called after the initiation of the ExperienceController
        public void Initiate()
        {
            _experienceController.Initiate();
            _currentLevel = _levelsData.CalculateLevel(_experienceController.ExpAmount);
        }
        
        public void AddExperience(int amount)
        {
            _experienceController.AddExp(amount);
            var newLevel = _levelsData.CalculateLevel(_experienceController.ExpAmount);
            
            OnAddExp?.Invoke();
            
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                Debug.Log($"Level up! New level: {_currentLevel}");
                OnLevelUp?.Invoke();
            }
        }

        #region Debug

        public void SetExperience(int expAmount)
        {
            _experienceController.SetExp(expAmount);
        }

        #endregion
    }
}