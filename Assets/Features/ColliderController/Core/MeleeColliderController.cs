using System.Collections.Generic;
using Features.Damage.Core;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;

namespace Features.ColliderController.Core
{
    public class MeleeColliderController : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private MeleeColliderInfo[] _collidersInfo;
        [SerializeField] private SkillMeleeColliderInfo[] _skillCollidersInfo;
        
        private AColliderDamageProcessor _attackDamageProcessor;
        private AColliderDamageProcessor _secondaryDamageProcessor;
        private AColliderDamageProcessor _ultDamageProcessor;
        private APassiveController _passiveController;

        private byte _currentCollider;

        public void Initiate(AColliderDamageProcessor attackDamageProcessor = null, AColliderDamageProcessor secondaryDamageProcessor = null,
            AColliderDamageProcessor ultDamageProcessor = null,
            APassiveController passiveController = null)
        {
            _passiveController = passiveController;
            _attackDamageProcessor = attackDamageProcessor;
            _secondaryDamageProcessor = secondaryDamageProcessor;
            _ultDamageProcessor = ultDamageProcessor;
            
            _attackDamageProcessor?.SetCollider(_collider);
            _secondaryDamageProcessor?.SetCollider(_collider);
            _ultDamageProcessor?.SetCollider(_collider);
        }

        public void EnableAttackCollider(MeleeColliderType meleeColliderType)
        {
            _collider.enabled = true;
            
            for (int i = 0; i < _collidersInfo.Length; i++)
            {
                if (_collidersInfo[i].meleeColliderType == meleeColliderType)
                {
                    _collider.offset = _collidersInfo[i].offset;
                    _collider.size = _collidersInfo[i].size;
                }
            }
            _attackDamageProcessor.Start();
            _currentCollider = 0;
        }
        
        public void EnableSecondaryCollider(SkillMeleeColliderType meleeColliderType)
        {
            _collider.enabled = true;
            
            for (int i = 0; i < _skillCollidersInfo.Length; i++)
            {
                if (_skillCollidersInfo[i].meleeColliderType == meleeColliderType)
                {
                    _collider.offset = _skillCollidersInfo[i].offset;
                    _collider.size = _skillCollidersInfo[i].size;
                }
            }
            _secondaryDamageProcessor.Start();
            _currentCollider = 1;
        }
        
        // public void EnableUltCollider(SkillMeleeColliderType meleeColliderType)
        // {
        //     // _collider.enabled = true;
        //     // _collidersDamaged = new List<Collider2D>();
        //     //
        //     // for (int i = 0; i < _skillCollidersInfo.Length; i++)
        //     // {
        //     //     if (_skillCollidersInfo[i].meleeColliderType == meleeColliderType)
        //     //     {
        //     //         _collider.offset = _skillCollidersInfo[i].offset;
        //     //         _collider.size = _skillCollidersInfo[i].size;
        //     //     }
        //     // }
        //     _currentCollider = 2;
        // }

        public void DisableCollider()
        {
            _collider.enabled = false;

            _attackDamageProcessor?.Stop();
            _secondaryDamageProcessor?.Stop();
            _ultDamageProcessor?.Stop();
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_collider.enabled == false) return;

            switch (_currentCollider)
            {
                case 0:
                    _attackDamageProcessor.OnFixedUpdate(fixedDeltaTime);
                    break;
                case 1:
                    _secondaryDamageProcessor.OnFixedUpdate(fixedDeltaTime);
                    break;
                case 2:
                    _ultDamageProcessor.OnFixedUpdate(fixedDeltaTime);
                    break;
            }
        }
    }
}