﻿using System;
using UnityEngine;

namespace Features.Experience
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Data/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] private LevelData[] _levelsData;
        
        public LevelData[] GetLevelsData()
        {
            return _levelsData;
        }
        
        public int CalculateLevel(int expAmount)
        {
            int level = 0;
            for (int i = 0; i < _levelsData.Length; i++)
            {
                if (expAmount >= _levelsData[i].expAmount)
                {
                    level = _levelsData[i].level;
                }
            }

            return level;
        }
        
        public int GetExpForLevel(int level)
        {
            for (int i = 0; i < _levelsData.Length; i++)
            {
                if (_levelsData[i].level == level)
                {
                    return _levelsData[i].expAmount;
                }
            }

            return 0;
        }
    }
    
    [Serializable]
    public class LevelData 
    {
        public int level;
        public int expAmount;
    }
}