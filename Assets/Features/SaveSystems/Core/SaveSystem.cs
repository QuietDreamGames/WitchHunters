using System;
using System.Collections.Generic;
using Features.SaveSystems.Configs;
using Features.SaveSystems.Interfaces;
using Features.SaveSystems.Modules.PathBuilder;
using Features.SaveSystems.Modules.Serializer;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.SaveSystems.Core
{
    public class SaveSystem : MonoBehaviour
    {
        private readonly Dictionary<string, SavableData> _data = new();

        private SaveSettings _settings;
        
        private ISavableSerializer _serializer;
        private ISavablePathBuilder _pathBuilder;

        #region Constructor

        public void Construct()
        {
            _settings = SaveSettings.GetSettings();
            
            _serializer = ServiceLocator.Resolve<ISavableSerializer>();
            _pathBuilder = ServiceLocator.Resolve<ISavablePathBuilder>();
        }

        #endregion

        #region Subscriber

        public void Subscribe(string category, ISavableCollector collector)
        {
            var contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                savableData = new SavableData(_serializer, _pathBuilder);
                _data.Add(category, savableData);
            }
            
            savableData.AddCollector(collector);
        }
        
        public void Unsubscribe(string category, SavableCollector collector)
        {
            var contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                return;
            }
            
            savableData.RemoveCollector(collector);
        }

        #endregion

        #region Savable

        public void Save(string category)
        {
            var contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                return;
            }
            
            savableData.Save();
        }
        
        public void Load(string category)
        {
            var contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                return;
            }
            
            savableData.Load();
        }
        
        public void SaveToDisk(string category)
        {
            var contains = TryGetSaveCategory(category, out var saveCategory);
            if (!contains)
            {
                Debug.LogWarning($"Save category {category} not found");
                return;
            }
            
            contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                return;
            }

            savableData.SaveToDisk(saveCategory);
        }
        
        public void LoadFromDisk(string category)
        {
            var contains = TryGetSaveCategory(category, out var saveCategory);
            if (!contains)
            {
                Debug.LogWarning($"Save category {category} not found");
                return;
            }
            
            contains = _data.TryGetValue(category, out var savableData);
            if (!contains)
            {
                return;
            }

            savableData.LoadFromDisk(saveCategory);
        }

        public void SetIndex(int index)
        {
            _pathBuilder.Index = index;
        }
        
        #endregion

        #region Private methods

        private bool TryGetSaveCategory(string category, out SaveSettings.SaveCategory saveCategory)
        {
            saveCategory = null;
            for (var i = 0; i < _settings.SaveCategories.Length; i++)
            {
                if (_settings.SaveCategories[i].category == category)
                {
                    saveCategory = _settings.SaveCategories[i];
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}