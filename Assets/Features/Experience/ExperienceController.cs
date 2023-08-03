using System;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Experience
{
    public class ExperienceController : MonoBehaviour, ISavable
    {
        [SerializeField] private ExpData _expData;
        
        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_expData);
        }

        public void Load(byte[] data)
        {
            _expData = Serializer.Deserialize<ExpData>(data);
        }
    }

    [Serializable]
    public class ExpData
    {
        public int expAmount;
    }
}