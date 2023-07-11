using Features.SaveSystems.Core;
using Features.SaveSystems.Editor;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.SaveSystems.Samples.Feature
{
    public class SampleSaver : MonoBehaviour
    {
        [SerializeField][SaveCategory] private string saveCategory;
        
        [ContextMenu("Save")]
        public void Save()
        {
            var saveSystem = ServiceLocator.Resolve<SaveSystem>();
            saveSystem.Save(saveCategory);
            saveSystem.SaveToDisk(saveCategory);
        }
        
        [ContextMenu("Load")]
        public void Load()
        {
            var saveSystem = ServiceLocator.Resolve<SaveSystem>();
            saveSystem.LoadFromDisk(saveCategory);
            saveSystem.Load(saveCategory);
        }
    }
}