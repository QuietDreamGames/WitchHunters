using Features.SaveSystems.Configs;

namespace Features.SaveSystems.Interfaces
{
    public interface ISavablePathBuilder
    {
        public int SaveSlotIndex { get; set; }
        public int CharacterIndex { get; set; }
        
        public string GetDirectoryPath(SaveCategory saveCategory);
    }
}