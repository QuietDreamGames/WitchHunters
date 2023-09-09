﻿namespace Features.Modifiers
{
    public enum ModifierType : byte
    {
        AttackDamage = 0,
        AttackSpeed = 1,
        MaximumHealth = 3,
        MaximumShieldHealth = 4,
        Armor = 8,
        CriticalChance = 9,
        CriticalDamage = 10,
        Lifesteal = 11,
        HealthRegeneration = 12,
        KnockbackResistance = 14,
        KnockbackForce = 15,
        UltimateDamage = 16,
        UltimateCooldown = 17,
        UltimateDuration = 18,
        UltimateBurstsAmount = 19,
        UltimateAmountInBurst = 19,
        UltimateRange = 20,
        PassiveDelayDecay = 22,
        PassiveChargedTime = 23,
        PassiveChargePerHit = 24,
        PassiveChargedAttackDamage = 25,
        PassiveAmountToCharge = 26,
        ShieldMaxHealth = 27,
        ShieldRegenRate = 28,
        ShieldRegenDelay = 29,
        SecondaryCooldown = 31,
        SecondarySkillRange = 32,
        SecondarySkillDamage = 33,
        SecondarySkillSpeed = 34,
        SecondarySkillLifetime = 35,
        SecondarySkillMaxSize = 36,
        MoveSpeed = 37,
        CastSpeed = 38,
        MaxWeight = 39,
    }
}