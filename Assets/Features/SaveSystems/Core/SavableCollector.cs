using Features.SaveSystems.Editor;
using Features.SaveSystems.Interfaces;
using Features.ServiceLocators.Core;
using Features.Structs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.SaveSystems.Core
{
    [RequireComponent(typeof(UniqueID))]
    public class SavableCollector : MonoBehaviour, ISavableCollector
    {
        [SerializeField][SaveCategory] private string category;
        [SerializeField] private UniqueID uniqueID;
        
        [Header("Dependencies")]
        [SerializeField] private Object customSavable;

        private SaveSystem _saveSystem;
        
        private ISavable _savable;

        private void Awake()
        {
            if (customSavable != null)
            {
                _savable = customSavable as ISavable;
                if (_savable == null)
                {
                    Debug.LogWarning($"Custom savable from {name} must implement ISavable interface", this);
                }
            }
            else
            {
                _savable = GetComponent<ISavable>();
            }
            
            if (_savable == null)
            {
                Debug.LogWarning($"Savable from {name} not found", this);
                return;
            }
            
            if (uniqueID == null)
            {
                uniqueID = GetComponent<UniqueID>();
            }

            _savable.Serializer = ServiceLocator.Resolve<ISavableSerializer>();
        }
        
        private void OnEnable()
        {
            if (_savable == null)
            {
                return;
            }

            _saveSystem = ServiceLocator.Resolve<SaveSystem>();
            _saveSystem.Subscribe(category, this);
        }
        
        private void OnDisable()
        {
            if (_savable == null)
            {
                return;
            }

            _saveSystem.Unsubscribe(category, this);
        }

        public string ID => uniqueID.ID;
        
        public byte[] Save()
        {
            return _savable.Save();
        }

        public void Load(byte[] value)
        {
            _savable.Load(value);
        }
    }
}