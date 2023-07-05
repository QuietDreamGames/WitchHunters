using System.Collections.Generic;
using Features.ColliderController.Interfaces;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.Team;
using UnityEngine;

namespace Features.ColliderController.Core
{
    public class MeleeColliderController : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private MeleeColliderInfo[] _collidersInfo;
        
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        private APassiveController _passiveController;
        private IMeleeDamageProcessor _meleeDamageProcessor;
        
        private List<Collider2D> _collidersDamaged = new List<Collider2D>();
        
        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer, IMeleeDamageProcessor meleeDamageProcessor, APassiveController passiveController)
        {
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            _meleeDamageProcessor = meleeDamageProcessor;
            _passiveController = passiveController;
        }
        
        public void EnableCollider(MeleeColliderType meleeColliderType)
        {
            _collider.enabled = true;
            _collidersDamaged = new List<Collider2D>();
            
            for (int i = 0; i < _collidersInfo.Length; i++)
            {
                if (_collidersInfo[i].meleeColliderType == meleeColliderType)
                {
                    _collider.offset = _collidersInfo[i].offset;
                    _collider.size = _collidersInfo[i].size;
                }
            }
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        private void Update()
        {
            if (!_collider.enabled) return;
            
            ProcessDamage();
        }

        private void ProcessDamage()
        {
            _collidersDamaged.AddRange(_meleeDamageProcessor.ProcessMeleeDamage(
                _collider, _collidersDamaged, transform, _modifiersContainer, _baseModifiersContainer, _passiveController
                ));
        }
    }
}