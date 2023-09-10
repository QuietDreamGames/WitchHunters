namespace Features.CameraShakes.Core
{
    public interface IShakeDirector
    {
        public void RegisterShakeData(CameraShakeData data);
        public void UnregisterShakeData(CameraShakeData data);
    }
}