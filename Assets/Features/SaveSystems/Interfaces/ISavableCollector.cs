namespace Features.SaveSystems.Interfaces
{
    public interface ISavableCollector
    {
        string ID { get; }
        
        byte[] Save();
        void Load(byte[] value);
    }
}