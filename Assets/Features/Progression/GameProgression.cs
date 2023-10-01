using System;
using Edgar.Unity;
using UnityEngine;
using DungeonInfo = Features.Progression.DungeonProgression.DungeonInfo;

namespace Features.Progression
{
    public class GameProgression : MonoBehaviour
    {
        [SerializeField] private DungeonProgression[] dungeons;
        
        [Space]
        [SerializeField] private int currentDungeonIndex;
        
        [Header("Dependencies")]
        [SerializeField] private GameGraph gameGraph;

        public LevelGraph GetLevelGraph()
        {
            return dungeons[currentDungeonIndex].GetLevelGraph();
        }
        
        public GameInfo GetInfo()
        {
            var dungeonsInfo = new DungeonInfo[dungeons.Length];
            for (var i = 0; i < dungeons.Length; ++i)
            {
                dungeonsInfo[i] = dungeons[i].GetInfo();
            }
            
            var info = new GameInfo
            {
                dungeonsInfo = dungeonsInfo,
            };
            
            return info;
        }
        
        public (int, int) GetCurrent()
        {
            return (currentDungeonIndex, dungeons[currentDungeonIndex].GetCurrent());
        }
        
        public void SetCurrent(int dungeonIndex, int floorIndex)
        {
            currentDungeonIndex = dungeonIndex;
            dungeons[currentDungeonIndex].SetCurrent(floorIndex);
        }
        
        public bool InvokeComplete()
        {
            var isComplete = dungeons[currentDungeonIndex].InvokeComplete();
            if (!isComplete)
            {
                return false;
            }

            dungeons[currentDungeonIndex].SetCompleted();
            var reached = gameGraph.GetReached(currentDungeonIndex);
            for (var i = 0; i < reached.Count; i++)
            {
                var index = reached[i];
                dungeons[index].SetReached();
            }

            return true;
        }
        
        [Serializable]
        public class GameInfo
        {
            public DungeonInfo[] dungeonsInfo;
        }
    }
}
