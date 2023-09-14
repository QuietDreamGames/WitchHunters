using System.Collections.Generic;
using Features.Character;
using Features.Character.Spawn;
using Features.Drop;
using Features.Input;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.UI.Gameplay.Loot
{
    public class LootPanelController : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private GameObject _lootPanel;
        [SerializeField] private LootItemController _lootItemPrefab;
        [SerializeField] private GameObject _lootItemParent;
        
        private List<LootItemController> _lootItems = new List<LootItemController>();
        private LootItemController _selectedLootItem;

        private PlayerInput _playerInput;
        private CharacterHolder _characterHolder;
        
        private DropDetector _dropDetector;
        private DropInstance _lastDropInstance;

        private bool _isAllowedShowLootPanel;
        // private bool _isShowLootPanel;
        

        public void OnUpdate(float deltaTime)
        {
            if (!_isAllowedShowLootPanel)
            {
                _lootPanel.SetActive(false);
                return;
            }

            if (_playerInput.actions["Interact"].WasPressedThisFrame())
            {

                if (_lootPanel.activeSelf)
                {
                    ProcessPickup();
                    return;
                }

                ShowLootPanel();
            }
            
            var scrollInput = _playerInput.actions["ScrollWheel"].ReadValue<Vector2>();
            
            if (scrollInput.normalized.magnitude > 0.1f)
            {
                var scrollValue = scrollInput.y;
                var index = _lootItems.IndexOf(_selectedLootItem);
                index += scrollValue > 0 ? -1 : 1;
                index = Mathf.Clamp(index, 0, _lootItems.Count - 1);
                
                _selectedLootItem = _lootItems[index];
                _selectedLootItem.Select();

                for (int i = 0; i < _lootItems.Count; i++)
                {
                    if (_selectedLootItem != _lootItems[i]) 
                        _lootItems[i].Deselect();
                }
            }
        }
        
        public void ShowLootPanel()
        {
            var (drops, money) = _lastDropInstance.GetDrops();

            for (int i = 0; i < _lootItems.Count; i++)
            {
                Destroy(_lootItems[i].gameObject);
            }
            
            _lootItems = new List<LootItemController>();
            
            for (int i = 0; i < drops.Count; i++)
            {
                _lootItems.Add(Instantiate(_lootItemPrefab, _lootItemParent.transform));
                _lootItems[i].Initiate(drops[i]);
            }
            
            if (money > 0)
            {
                _lootItems.Add(Instantiate(_lootItemPrefab, _lootItemParent.transform));
                _lootItems[^1].Initiate(null, money);
            }
            
            _selectedLootItem = _lootItems[0];
            _selectedLootItem.Select();
            for (int i = 0; i < _lootItems.Count; i++)
            {
                if (_selectedLootItem != _lootItems[i]) 
                    _lootItems[i].Deselect();
            }
            
            _lootPanel.SetActive(true);
        }

        private void ProcessPickup()
        {
            if (_selectedLootItem.currency != 0)
            {
                _lastDropInstance.OnGrabCurrency();
                _characterHolder.CurrentCharacter.InventoryController.AddCurrency(_selectedLootItem.currency);
            }
            else
            {
                _lastDropInstance.OnGrabItem(_selectedLootItem.item);
                _characterHolder.CurrentCharacter.InventoryController.AddItem(_selectedLootItem.item);
            }
            
            var index = _lootItems.IndexOf(_selectedLootItem);
            _lootItems.Remove(_selectedLootItem);
            Destroy(_selectedLootItem.gameObject);
            
            if (_lootItems.Count == 0)
            {
                _lootPanel.SetActive(false);
                return;
            }
            
            _selectedLootItem = _lootItems.Count > index ? _lootItems[index] : _lootItems[index - 1];
            
            _selectedLootItem.Select();

            for (int i = 0; i < _lootItems.Count; i++)
            {
                if (_selectedLootItem != _lootItems[i]) 
                    _lootItems[i].Deselect();
            }
        }

        private void OnDropDetected(bool isDetected, DropInstance dropInstance)
        {
            _isAllowedShowLootPanel = isDetected;
            if (_lastDropInstance != dropInstance)
                _lootPanel.SetActive(false);
            _lastDropInstance = dropInstance;
        }

        // private void OnScrollMove(InputAction.CallbackContext callbackContext)
        // {
        //     Debug.Log("?");
        //     if (!_lootPanel.activeSelf) return;
        //     
        //     var scrollValue = _playerInput.actions["ScrollWheel"].ReadValue<float>();
        //     var index = _lootItems.IndexOf(_selectedLootItem);
        //     index += scrollValue > 0 ? 1 : -1;
        //     index = Mathf.Clamp(index, 0, _lootItems.Count - 1);
        //         
        //     _selectedLootItem = _lootItems[index];
        //     _selectedLootItem.Select();
        //
        //     for (int i = 0; i < _lootItems.Count; i++)
        //     {
        //         if (_selectedLootItem != _lootItems[i]) 
        //             _lootItems[i].Deselect();
        //     }
        // }
        
        private void OnEnable()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _playerInput = ServiceLocator.Resolve<InputData>().playerInput;
            
            _characterHolder.OnCharacterChanged += OnCharacterChanged;
            
            if (_characterHolder.CurrentCharacter == null) return;
            _dropDetector = _characterHolder.CurrentCharacter.DropDetector;
            _dropDetector.OnDropDetected += OnDropDetected;
        }
        
        private void OnDisable()
        {
            // _playerInput.actions["ScrollWheel"].performed -= OnScrollMove;
            _characterHolder.OnCharacterChanged -= OnCharacterChanged;
            if (_characterHolder.CurrentCharacter == null) return;
            _dropDetector.OnDropDetected -= OnDropDetected;
        }
        
        private void OnCharacterChanged(CombatCharacterController character)
        {
            if (_dropDetector != null)
                _dropDetector.OnDropDetected -= OnDropDetected;    
            
            _dropDetector = character.DropDetector;
            _dropDetector.OnDropDetected += OnDropDetected;
        }
    }
}