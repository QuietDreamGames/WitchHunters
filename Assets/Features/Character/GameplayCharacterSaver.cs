using Features.SaveSystems.Core;
using Features.SaveSystems.Editor;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Character
{
    public class GameplayCharacterSaver : MonoBehaviour
    {
        [SerializeField][SaveCategory] private string saveCategory;
        
        public void Save()
        {
            var saveSystem = ServiceLocator.Resolve<SaveSystem>();
            saveSystem.Save(saveCategory);
            saveSystem.SaveToDisk(saveCategory);
        }
        
        public void Load()
        {
            var saveSystem = ServiceLocator.Resolve<SaveSystem>();
            saveSystem.LoadFromDisk(saveCategory);
            saveSystem.Load(saveCategory);
        }
    }
}