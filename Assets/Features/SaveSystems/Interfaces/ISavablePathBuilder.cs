using Features.SaveSystems.Configs;

namespace Features.SaveSystems.Interfaces
{
    public interface ISavablePathBuilder
    {
        public int Index { get; set; }
        
        public string GetDirectoryPath(SaveCategory saveCategory);
    }
}