namespace Features.Skills.Interfaces
{
    public interface IShieldHealthController
    {
        public float GetHit(float damage);
        public void SetShieldActive(bool isActive);
    }
}