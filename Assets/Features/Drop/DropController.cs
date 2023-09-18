using Features.Character.Spawn;
using Features.Enemies;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Drop
{
    public class DropController : MonoBehaviour
    {
        [SerializeField] private UnitBehaviour _unitBehaviour;
        [SerializeField] private DropData _dropData;
        [SerializeField] private DropInstance _dropInstancePrefab;

        private void OnEnable()
        {
            _unitBehaviour.OnDeath += OnDeath;
        }
        
        private void OnDisable()
        {
            _unitBehaviour.OnDeath -= OnDeath;
        }
        
        private void OnDeath()
        {
            var characterController = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            characterController.LevelController.AddExperience(_dropData.GetExperience());
            
            var items = _dropData.GetDrops();
            var currency = _dropData.GetCurrency();
            
            if (items.Count == 0 && currency == 0) return;
            
            var dropInstance = Instantiate(_dropInstancePrefab, transform.position, Quaternion.identity);
            dropInstance.Configure(items, currency);
        }
    }
    
}