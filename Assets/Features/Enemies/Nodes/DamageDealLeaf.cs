using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class DamageDealLeaf : LeafNode
    {
        [SerializeField] private LayerMask layerMask;
        
        private new Rigidbody2D rigidbody2D;
        private DamageDealer damageDealer;
        private UnitConfig unitConfig;
        
        private Transform origin;
        private float baseDamage;

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
            baseDamage = unitConfig.BaseDamage;
            
            damageDealer.Reset();
        }
                            
        protected override void OnExit()
        {
            
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            var damage = baseDamage;
            
            damageDealer.DealDamage(damage, origin, layerMask);

            return Status.Running;
        }
    }
}
