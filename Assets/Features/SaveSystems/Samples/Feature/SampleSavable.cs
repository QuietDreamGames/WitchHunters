using System;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.SaveSystems.Samples.Feature
{
    public class SampleSavable : MonoBehaviour, ISavable
    {
        public Data data1;
        
        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(data1);
        }

        public void Load(byte[] data)
        {
            data1 = Serializer.Deserialize<Data>(data);
        }

        [Serializable]
        public class Data
        {
            public int value;
        }
    }
}