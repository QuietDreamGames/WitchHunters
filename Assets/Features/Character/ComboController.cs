using System.Collections;
using UnityEngine;

namespace Features.Character
{
    public class ComboController : MonoBehaviour
    {
        private float _timeRoomCombo = 0.5f;
        private float _comboTimer;
        private bool _shouldComboMelee;
        private int _attackCombos;
        private int _currentAttackComboNr;

        public void Initiate(int attackCombos)
        {
            _attackCombos = attackCombos;
            _currentAttackComboNr = 0;
        }
        
        protected IEnumerator ComboControllerOnAttack(CharacterView characterView)
        {
            yield return new WaitForEndOfFrame();
            var animLength = characterView.CurrentAnimationLengthWithSpeed();
            
            _comboTimer = _timeRoomCombo + animLength;
            _shouldComboMelee = true;
            
            _currentAttackComboNr++;
            if (_currentAttackComboNr >= _attackCombos)
            {
                _currentAttackComboNr = 0;
            }
        }

        public void OnAttack(CharacterView characterView)
        {
            StartCoroutine(ComboControllerOnAttack(characterView));
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_comboTimer <= 0) return;
            
            _comboTimer -= deltaTime;
            
            if (_comboTimer <= 0)
            {
                _currentAttackComboNr = 0;
            }
        }
        
        public int GetAttackComboNr()
        {
            return _currentAttackComboNr;
        }
        
    }
}