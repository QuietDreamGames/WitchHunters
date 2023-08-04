using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.SaveSystems.Configs;
using Features.SaveSystems.Interfaces;
using Features.SaveSystems.Modules.PathBuilder;
using Features.Structs;
using UnityEngine;

namespace Features.SaveSystems.Core
{
    [Serializable]
    public class SavableStorage
    {
        private SerializableDictionary<string, ArrayCollection<byte>> _storage;

        private readonly ISavablePathBuilder _pathBuilder;
        private readonly ISavableSerializer _serializer;
        
        public SavableStorage(
            ISavableSerializer serializer, 
            ISavablePathBuilder pathBuilder)
        {
            _storage = new SerializableDictionary<string, ArrayCollection<byte>>();

            _pathBuilder = pathBuilder;
            _serializer = serializer;
        }
        
        public void Save(string id, byte[] data)
        {
            var contains = _storage.ContainsKey(id);
            if (contains)
            {
                _storage[id] = new ArrayCollection<byte>(data);
            }
            else
            {
                _storage.Add(id, new ArrayCollection<byte>(data));
            }
        }
        
        public byte[] Load(string id)
        {
            var contains = _storage.ContainsKey(id);
            if (!contains)
            {
                return null;
            }
            
            return _storage[id].value;
        }

        public void SaveToDisk(SaveCategory saveCategory)
        {
            var directoryPath = _pathBuilder.GetDirectoryPath(saveCategory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            var extension = saveCategory.extension.TrimStart('.');
            var filePath = Path.Combine(directoryPath, $"{saveCategory.category}.{extension}");
            
            var data= _serializer.Serialize(_storage);
            File.WriteAllBytes(filePath, data);
        }
        
        public void LoadFromDisk(SaveCategory saveCategory)
        {
            _storage.Clear();
            var directoryPath = _pathBuilder.GetDirectoryPath(saveCategory); 
            if (!Directory.Exists(directoryPath))
            {
                Debug.LogWarning($"Directory {directoryPath} not found");
                return;
            }
            
            var extension = saveCategory.extension.TrimStart('.');
            var filePath = Path.Combine(directoryPath, $"{saveCategory.category}.{extension}");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"File {filePath} not found");
                return;
            }
            
            var data = File.ReadAllBytes(filePath);
            _storage = _serializer.Deserialize<SerializableDictionary<string, ArrayCollection<byte>>>(data);
        }

        [Serializable]
        public class ArrayCollection<T>
        {
            public T[] value;
            
            public ArrayCollection(T[] value)
            {
                this.value = value;
            }
        }
    }
}