using System.Collections.Generic;
using Features.Damage.Core;
using Features.Damage.Implementations;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class InquisitorUltimateMeteors : ASkill
    {
        [SerializeField] private MeteorController _meteorPrefab;
        private GameObjectPool<MeteorController> _meteorsPool;
        
        private DamageableCache _damageableCache;
        private LayerMask _hittableLayerMask;

        private bool _isCasting;
        private int _meteorsCasted;
        private float _meteorsToCast;
        private float _meteorSpawnInterval;
        private float _radius;
        private float _offsetForRandomPosition = 3;

        private float _timer;

        public override void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            base.Initiate(modifiersContainer, baseModifiersContainer);
            _meteorsPool = ServiceLocator.Resolve<GameObjectPool<MeteorController>>();
            
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();
            
            _hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
        }

        public override void Cast(Vector3 direction)
        {
            _isCasting = true;
            _meteorsCasted = 0;
            _meteorsToCast = ModifiersContainer.GetValue(ModifierType.UltimateBurstsAmount,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateBurstsAmount));
            _radius = ModifiersContainer.GetValue(ModifierType.UltimateRange,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateRange));
            var duration = ModifiersContainer.GetValue(ModifierType.UltimateDuration,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateDuration));
            _meteorSpawnInterval = duration / (_meteorsToCast - 1);
            _timer = _meteorSpawnInterval;
            
            _maxCooldown = ModifiersContainer.GetValue(ModifierType.UltimateCooldown,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateCooldown));
            _currentCooldown = _maxCooldown;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (IsOnCooldown) _currentCooldown -= deltaTime;
            
            if (_currentCooldown < 0f) _currentCooldown = 0f;
            
            if (!_isCasting) return;
            
            _timer -= deltaTime;

            if (_timer >= 0) return;
            
            _timer = _meteorSpawnInterval;
            _meteorsCasted++;

            var meteor = _meteorsPool.Spawn(_meteorPrefab.gameObject, null);
            meteor.Pool = _meteorsPool;
            meteor.Prefab = _meteorPrefab.gameObject;
            meteor.transform.position = FindFallPosition();
            
            var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");
            
            var damageInstance = new InqSolarUltDamageInstance(hittableLayerMask, obstacleLayerMask,
                ModifiersContainer, BaseModifiersContainer);
            
            meteor.Cast(damageInstance);
            
            if (_meteorsCasted >= _meteorsToCast)
            {
                _isCasting = false;
            }
        }
        
        private Vector3 FindFallPosition()
        {
            var (success, target) = FindTarget();

            return success ? FindRandomPosition(target.position, _offsetForRandomPosition) : FindRandomPosition(transform.position, _radius);
        }

        private static Vector3 FindRandomPosition(Vector3 point, float radius)
        {
            // return random position in radius around a certain point
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 randomPosition = point + new Vector3(randomCircle.x, randomCircle.y, 0);
            return randomPosition;
        }
        
        private (bool, Transform) FindTarget()
        {
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(_hittableLayerMask);
            int colliderCount = Physics2D.OverlapCircle(transform.position, _radius, contactFilter2D, colliders);

            var enemies = new List<Transform>();
            
            for (int j = 0; j < colliderCount; j++)
            {
                enemies.Add(colliders[j].transform);
            }
            
            if (enemies.Count <= 0) return (false, null);
            
            var randomIndex = Random.Range(0, enemies.Count);
            return (true, enemies[randomIndex]);
        }
    }
}