using System.Collections.Generic;
using Features.ColliderController.Interfaces;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.Team;
using UnityEngine;

namespace Features.ColliderController.Implementations
{
    public class InqMeleeDamageProcessor : IMeleeDamageProcessor
    {
        public List<Collider2D> ProcessMeleeDamage(Collider2D collider2D, List<Collider2D> collidersDamaged, Transform transform, ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, APassiveController passiveController)
        {
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = collider2D.OverlapCollider(contactFilter2D, colliders);
            
            var tempCollidersDamaged = new List<Collider2D>();

            for (int j = 0; j < colliderCount; j++)
            {
                if (collidersDamaged.Contains(colliders[j])) continue;
                
                var damageable = colliders[j].GetComponent<IDamageable>();
                if (damageable == null) continue;
                if (damageable.TeamIndex != TeamIndex.Enemy) continue;

                var knockbackDirection = colliders[j].transform.position - transform.position;
                knockbackDirection.Normalize();

                if (passiveController.CurrentPassiveInfo.IsCharged)
                {
                    var chargedDamage = modifiersContainer.GetValue(ModifierType.PassiveChargedAttackDamage,
                        baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargedAttackDamage));
                    damageable.TakeDamage(chargedDamage);
                }
                
                var damage = modifiersContainer.GetValue(ModifierType.AttackDamage,
                    baseModifiersContainer.GetBaseValue(ModifierType.AttackDamage));
                
                var knockbackForce = knockbackDirection * modifiersContainer.GetValue(ModifierType.KnockbackForce,
                    baseModifiersContainer.GetBaseValue(ModifierType.KnockbackForce));
                damageable.TakeDamage(damage, knockbackForce);
                
                passiveController.OnHit();
                
                tempCollidersDamaged.Add(colliders[j]);
            }

            return tempCollidersDamaged;
        }
    }
}