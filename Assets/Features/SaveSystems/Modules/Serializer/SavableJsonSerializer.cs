using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.SaveSystems.Modules.Serializer
{
    public class SavableJsonSerializer : ISavableSerializer
    {
        public byte[] Serialize<T>(T data) where T : class 
        {
            var json = JsonUtility.ToJson(data);
            return System.Text.Encoding.ASCII.GetBytes(json); 
        }

        public T Deserialize<T>(byte[] byteArray) where T : class
        {
            var json = System.Text.Encoding.ASCII.GetString(byteArray);
            return JsonUtility.FromJson<T>(json);
        }
    }
}