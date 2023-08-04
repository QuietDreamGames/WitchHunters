﻿using Features.Damage.Implementations;
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

        private bool _isCasting;
        private int _meteorsCasted;
        private float _meteorsToCast;
        private float _meteorSpawnInterval;
        private float _radius;

        private float _timer;

        public override void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            base.Initiate(modifiersContainer, baseModifiersContainer);
            _meteorsPool = ServiceLocator.Resolve<GameObjectPool<MeteorController>>();
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
            meteor.transform.position = FindRandomPosition(_radius);
            
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

        private Vector3 FindRandomPosition(float radius)
        {
            // return random position in radius around transform.position
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 randomPosition = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
            return randomPosition;
        }
    }
}