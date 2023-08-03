namespace Features.Skills.Interfaces
{
    public interface IShieldHealthController
    {
        public float GetHit(float damage);
        public void SetShieldActive(bool isActive);
        public void GetShieldHealth(out float currentShieldHealth, out float maxShieldHealth);
        public void StopShieldUpdate();
        public void Restart();
    }
}