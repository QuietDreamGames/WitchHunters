namespace Features.SaveSystems.Interfaces
{
    public interface ISavableSerializer
    {
        byte[] Serialize<T>(T data) where T : class;
        T Deserialize<T>(byte[] byteArray) where T : class;
    }
}