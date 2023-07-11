using System.Collections.Generic;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Editor;
using Features.TimeSystems.Interfaces;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.TimeSystems.Core
{
    public class TimeCollector : MonoBehaviour, ITimeCollector
    {
        [SerializeField] private bool includeChildren;
        [SerializeField][TimeCategory] private string category;

        private TimeSystem _timeSystem;
        
        private bool _isConfigured;
        
        public List<IUpdateHandler> UpdateHandlers { get; } = new();
        public List<IFixedUpdateHandler> FixedUpdateHandlers { get; } = new();
        public List<ILateUpdateHandler> LateUpdateHandlers { get; } = new();

        public string Category => category;

        private void OnEnable()
        {
            if (_isConfigured)
            {
                return;
            }
            
            Collect();
            _timeSystem = ServiceLocator.Resolve<TimeSystem>();
            _timeSystem.Subscribe(this);
            
            _isConfigured = true;
        }
        
        private void OnDisable()
        {
            if (!_isConfigured)
            {
                return;
            }
            
            _timeSystem.Unsubscribe(this);
            
            _isConfigured = false;
        }

        private void Collect()
        {
            if (includeChildren)
            {
                GetComponentsInChildren(UpdateHandlers);
                GetComponentsInChildren(FixedUpdateHandlers);
                GetComponentsInChildren(LateUpdateHandlers);
            }
            else
            {
                GetComponents(UpdateHandlers);
                GetComponents(FixedUpdateHandlers);
                GetComponents(LateUpdateHandlers);
            }
        }
    }
}