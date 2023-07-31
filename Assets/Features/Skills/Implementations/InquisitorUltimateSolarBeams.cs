using System.Collections.Generic;
using Features.Damage.Implementations;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class InquisitorUltimateSolarBeams : ASkill
    {
        [SerializeField] private ParticleSystem _abmientParticles;
        [SerializeField] private AutoBeamController beamControllerPrefab;

        private float _beamsAmount;
        private float _range;
        
        private float _timer;
        private float _timeBetweenBeams;
        private int _beamsCasted;
        
        private bool _isCasting;

        public override void Cast(Vector3 direction)
        {
            var duration = ModifiersContainer.GetValue(ModifierType.UltimateDuration,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateDuration));
            var main = _abmientParticles.main;
            main.duration = duration;
            _abmientParticles.Play();
            
            _beamsAmount = ModifiersContainer.GetValue(ModifierType.UltimateBurstsAmount,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateBurstsAmount));
            _range = ModifiersContainer.GetValue(ModifierType.UltimateRange,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateRange)); 
            _timeBetweenBeams = duration / _beamsAmount;
            _beamsCasted = 0;
            _timer = 0f;
            _isCasting = true;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_isCasting) return;
            
            if (_beamsCasted >= _beamsAmount)
            {
                _isCasting = false;
            }
            else
            {
                _timer += Time.deltaTime;
                if (!(_timer > _timeBetweenBeams)) return;
                var (success, target) = FindTarget();
                if (success)
                {
                    var beamController = Instantiate(beamControllerPrefab, target.position, Quaternion.identity);
                    
                    var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
                    var obstacleLayerMask = LayerMask.GetMask("Obstacle");

                    beamController.Initiate(new InqSolarUltDamageInstance(hittableLayerMask, obstacleLayerMask,
                        ModifiersContainer, BaseModifiersContainer));
                    beamController.Cast();
                }
                _beamsCasted++;
                _timer = 0f;
            }
        }

        private (bool, Transform) FindTarget()
        {
            var colliders = new Collider2D[20];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = Physics2D.OverlapCircle(transform.position, _range, contactFilter2D, colliders);

            var enemies = new List<Transform>();
            
            for (int j = 0; j < colliderCount; j++)
            {
                if (colliders[j].gameObject.CompareTag("Enemy"))
                {
                    enemies.Add(colliders[j].transform);
                }
            }

            if (enemies.Count <= 0) return (false, null);
            
            var randomIndex = Random.Range(0, enemies.Count);
            return (true, enemies[randomIndex]);
        }
    }
}