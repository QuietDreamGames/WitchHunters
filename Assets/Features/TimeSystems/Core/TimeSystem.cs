using System.Collections.Generic;
using Features.ServiceLocators.Core.Service;
using Features.TimeSystems.Interfaces;
using UnityEngine;

namespace Features.TimeSystems.Core
{
    public class TimeSystem : MonoBehaviour
    {
        #region Private fields

        private readonly List<ITimeCollector> _collectors = new();
        private readonly Dictionary<string, float> _categories = new();

        private float _globalTimeScale = 1;

        #endregion

        #region Public methonds

        public void Subscribe(ITimeCollector collector)
        {
            var contains = _collectors.Contains(collector);
            if (contains)
            {
                return;
            }
            
            var category = collector.Category;
            FillCategory(category);

            _collectors.Add(collector);
        }
        
        public void Unsubscribe(ITimeCollector collector)
        {
            var contains = _collectors.Contains(collector);
            if (!contains)
            {
                return;
            }
            
            _collectors.Remove(collector);
        }
        
        public void SetGlobalTimeScale(float timeScale)
        {
            _globalTimeScale = timeScale;
        }
        
        public void SetCategoryTimeScale(string category, float timeScale)
        {
            FillCategory(category);
            
            _categories[category] = timeScale;
        }

        #endregion

        #region MonoBehaviour

        private void Update()
        {
            for (var i = 0; i < _collectors.Count; i++)
            {
                var configurator = _collectors[i];
                var handlers = configurator.UpdateHandlers;
                if (handlers.Count == 0)
                {
                    continue;
                }
                
                var categoryDeltaTime = _categories[configurator.Category];
                var deltaTime = Time.deltaTime * categoryDeltaTime * _globalTimeScale;
                if (deltaTime < float.Epsilon)
                {
                    continue;
                }
                
                for (var j = 0; j < handlers.Count; j++)
                {
                    var updateHandler = handlers[j];
                    
                    updateHandler.OnUpdate(deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < _collectors.Count; i++)
            {
                var configurator = _collectors[i];
                var handlers = configurator.FixedUpdateHandlers;
                if (handlers.Count == 0)
                {
                    continue;
                }
                
                var categoryDeltaTime = _categories[configurator.Category];
                var deltaTime = Time.fixedDeltaTime * categoryDeltaTime * _globalTimeScale;
                if (deltaTime < float.Epsilon)
                {
                    continue;
                }
                
                for (var j = 0; j < handlers.Count; j++)
                {
                    var updateHandler = handlers[j];
                    
                    updateHandler.OnFixedUpdate(deltaTime);
                }
            } 
        }

        private void LateUpdate()
        {
            for (var i = 0; i < _collectors.Count; i++)
            {
                var configurator = _collectors[i];
                var handlers = configurator.LateUpdateHandlers;
                if (handlers.Count == 0)
                {
                    continue;
                }
                
                var categoryDeltaTime = _categories[configurator.Category];
                var deltaTime = Time.deltaTime * categoryDeltaTime * _globalTimeScale;
                if (deltaTime < float.Epsilon)
                {
                    continue;
                }
                
                for (var j = 0; j < handlers.Count; j++)
                {
                    var updateHandler = handlers[j];
                    
                    updateHandler.OnLateUpdate(deltaTime);
                }
            }
        }

        #endregion

        #region Private methods

        private void FillCategory(string category)
        {
            var containsCategory = _categories.ContainsKey(category);
            if (!containsCategory)
            {
                _categories.Add(category, 1f);
            }
        }

        #endregion
    }
}