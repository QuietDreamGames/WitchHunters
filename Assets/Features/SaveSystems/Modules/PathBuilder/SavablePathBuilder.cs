using System.IO;
using Features.SaveSystems.Configs;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.SaveSystems.Modules.PathBuilder
{
    public class SavablePathBuilder : ISavablePathBuilder
    {
        public int SaveSlotIndex { get; set; } = 0;
        public int CharacterIndex { get; set; } = 0;

        private readonly string _absolutePath;
        
        public SavablePathBuilder()
        {
            _absolutePath = Path.Combine(Application.persistentDataPath, "Storage");
        }

        public string GetDirectoryPath(SaveCategory saveCategory)
        {
            var path = _absolutePath;
            if (saveCategory.useUser)
            {
                path = Path.Combine(path, "PLAYER");
            }
            
            if (saveCategory.useSaveSlot)
            {
                path = Path.Combine(path, $"SAVE_SLOT_{SaveSlotIndex}");
            }
            
            if (saveCategory.useCharacter)
            {
                path = Path.Combine(path, $"CHARACTER_{CharacterIndex}");
            }
            
            return path;
        }
    }
}