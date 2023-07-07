using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class ProjectileSpawnLeaf : LeafNode
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        
        private ProjectileSpawner projectileSpawner;
        private TargetCollection targetCollection;
        private UnitConfig unitConfig;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            projectileSpawner = stateMachine.GetExtension<ProjectileSpawner>();
            targetCollection = stateMachine.GetExtension<TargetCollection>();
            unitConfig = stateMachine.GetExtension<UnitConfig>();
        }
                                    
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            var target = targetCollection.GetClosestTarget();
            if (target == null)
            {
                return Status.Failure;
            }

            var projectile = projectileSpawner.Get();
            projectile.Damage = unitConfig.BaseDamage;
            projectile.Speed = speed;
            projectile.Lifetime = lifeTime;

            projectile.Spawn(target.position);
            
            return Status.Success;
        }
    }
}
