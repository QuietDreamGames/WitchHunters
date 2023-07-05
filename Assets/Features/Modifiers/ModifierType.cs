namespace Features.Modifiers
{
    public enum ModifierType : byte
    {
        AttackDamage = 0,
        AttackSpeed = 1,
        MovementSpeed = 2,
        MaximumHealth = 3,
        Armor = 8,
        CriticalChance = 9,
        CriticalDamage = 10,
        Lifesteal = 11,
        HealthRegeneration = 12,
        AttackDamagePercentage = 13,
        KnockbackResistance = 14,
        KnockbackForce = 15,
        UltimateDamage = 16,
        UltimateCooldown = 17,
        UltimateDuration = 18,
        UltimateBurstsAmount = 19,
        UltimateAmountInBurst = 19,
        UltimateRange = 20,
        UltimateCurrentCooldown = 21,
        PassiveDelayDecay = 22,
        PassiveChargedTime = 23,
        PassiveChargePerHit = 24,
        PassiveChargedAttackDamage = 25,
        PassiveAmountToCharge = 26,
    }
}