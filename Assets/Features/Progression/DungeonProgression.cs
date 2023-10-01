using System;
using Edgar.Unity;
using Features.SaveSystems.Interfaces;
using UnityEngine;
using FloorData = Features.Progression.FloorProgression.FloorData;

namespace Features.Progression
{
    public class DungeonProgression : MonoBehaviour, ISavable
    {
        [SerializeField] private FloorProgression[] floors;
        [SerializeField] private DungeonData dungeonData;
        
        [Space]
        [SerializeField] private int currentFloorIndex;

        public void SetReached()
        {
            dungeonData.isReached = true;
        }
        
        public void SetCompleted()
        {
            dungeonData.isCompleted = true;
        }
        
        public LevelGraph GetLevelGraph()
        {
            return floors[currentFloorIndex].GetLevelGraph();
        }
        
        public DungeonInfo GetInfo()
        {
            var floorsInfo = new FloorData[floors.Length];
            for (var i = 0; i < floors.Length; ++i)
            {
                floorsInfo[i] = floors[i].GetInfo();
            }
            
            var info = new DungeonInfo
            {
                dungeonData = dungeonData,
                floorsData = floorsInfo,
            };
            
            return info;
        }
        
        public int GetCurrent()
        {
            return currentFloorIndex;
        }
        
        public void SetCurrent(int index)
        {
            currentFloorIndex = index;
        }

        public bool InvokeComplete()
        {
            floors[currentFloorIndex].SetCompleted();

            ++currentFloorIndex;
            if (currentFloorIndex < floors.Length)
            {
                floors[currentFloorIndex].SetReached();
                return false;
            }

            return true;
        }
        
        #region ISavable implementation
        
        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(dungeonData);
        }

        public void Load(byte[] data)
        {
            dungeonData = Serializer.Deserialize<DungeonData>(data);
        }
        
        #endregion
        
        [Serializable]
        public class DungeonData
        {
            public bool isReached;
            public bool isCompleted;
        }

        [Serializable]
        public class DungeonInfo
        {
            public DungeonData dungeonData;
            public FloorData[] floorsData;
        }
    }
}
