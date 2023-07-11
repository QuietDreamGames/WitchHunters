using System.IO;
using Features.SaveSystems.Configs;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.SaveSystems.Modules.PathBuilder
{
    public class SavablePathBuilder : ISavablePathBuilder
    {
        public int Index { get; set; } = 0;

        private readonly string _absolutePath;
        
        public SavablePathBuilder()
        {
            _absolutePath = Path.Combine(Application.persistentDataPath, "Storage");
        }

        public string GetDirectoryPath(SaveSettings.SaveCategory saveCategory)
        {
            var path = _absolutePath;
            if (saveCategory.useUser)
            {
                path = Path.Combine(path, "Player");
            }
            
            if (saveCategory.indexed)
            {
                path = Path.Combine(path, $"{Index}");
            }
            
            return path;
        }
    }
}