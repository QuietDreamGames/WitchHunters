using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class DamageableObstacleSpawnLeaf : LeafNode
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float delay;
        [SerializeField] private LayerMask layerMask;
        
        private DamageableObstacleSpawner damageableObstacleSpawner;
        private TargetCollection targetCollection;
        private UnitConfig unitConfig;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            damageableObstacleSpawner = stateMachine.GetExtension<DamageableObstacleSpawner>();
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

            var damageableObstacle = damageableObstacleSpawner.Get();
            damageableObstacle.Damage = unitConfig.BaseDamage; 
            damageableObstacle.LifeTime = lifeTime;
            damageableObstacle.Delay = delay;
            damageableObstacle.LayerMask = layerMask;
            
            damageableObstacle.Spawn(target.position);
            
            return Status.Success;
        }
    }
}
