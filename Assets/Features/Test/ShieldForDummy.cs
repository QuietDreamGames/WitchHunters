using Features.Skills.Interfaces;

namespace Features.Test
{
    public class ShieldForDummy : IShieldHealthController
    {
        public float GetHit(float damage)
        {
            return damage;
        }

        public void SetShieldActive(bool isActive)
        {
            throw new System.NotImplementedException();
        }
    }
}