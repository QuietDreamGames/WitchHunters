namespace Features.SaveSystems.Interfaces
{
    public interface ISavable
    {
        ISavableSerializer Serializer { get; set; }
        
        byte[] Save();
        void Load(byte[] data);
    }
}
