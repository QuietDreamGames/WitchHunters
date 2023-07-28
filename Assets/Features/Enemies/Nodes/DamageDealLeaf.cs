using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.Modifiers;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class DamageDealLeaf : LeafNode
    {
        [SerializeField] private LayerMask layerMask;
        
        [Header("Parameters")]
        [SerializeField] private float damage = 1;
        [SerializeField] private float knockbackForce = 1;
        
        private new Rigidbody2D rigidbody2D;
        private DamageDealer damageDealer;
        private UnitConfig unitConfig;
        
        private Transform origin;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            damageDealer = stateMachine.GetExtension<DamageDealer>();
            unitConfig = stateMachine.GetExtension<UnitConfig>();
        }
                                    
        protected override void OnEnter()
        {
            origin = rigidbody2D.transform;
            
            damageDealer.Reset();
            damageDealer.Damage = unitConfig.ModifiersController.GetValue(ModifierType.AttackDamage, damage);
            damageDealer.KnockbackForce = unitConfig.ModifiersController.GetValue(ModifierType.KnockbackForce, knockbackForce);
        }
                            
        protected override void OnExit()
        {
            
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            damageDealer.DealDamage(origin, layerMask);

            return Status.Running;
        }
    }
}
