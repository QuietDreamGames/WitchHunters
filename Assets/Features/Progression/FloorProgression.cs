using System;
using Edgar.Unity;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Progression
{
    public class FloorProgression : MonoBehaviour, ISavable
    {
        [SerializeField] private LevelGraph levelGraph;
        [SerializeField] private FloorData floorData;

        public void SetReached()
        {
            floorData.isReached = true;
        }
        
        public void SetCompleted()
        {
            floorData.isCompleted = true;
        }
        
        public LevelGraph GetLevelGraph()
        {
            return levelGraph;
        }
        
        public FloorData GetInfo()
        {
            return floorData;
        }

        #region ISavable implementation

        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(floorData);
        }

        public void Load(byte[] data)
        {
            floorData = Serializer.Deserialize<FloorData>(data);
        }

        #endregion

        [Serializable]
        public class FloorData
        {
            public bool isReached;
            public bool isCompleted;
        }
    }
}
