using System;

namespace Features.Stats
{
    [Serializable]
    public class StatsData
    {
        public int strength;
        public int agility;
        public int intelligence;
        
        public int unusedStatsPoints;

        public StatsData Clone()
        {
            return new StatsData
            {
                strength = strength,
                agility = agility,
                intelligence = intelligence,
                unusedStatsPoints = unusedStatsPoints
            };
        }
    }
}