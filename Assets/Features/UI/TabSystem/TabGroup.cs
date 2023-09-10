using System;
using Features.GameManagers;
using Features.Input;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.UI.TabSystem
{
    public class TabGroup : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private GameObject _tabGroupParent;
        [SerializeField] private GameObject _hudParent;
        [SerializeField] private TabButton[] _tabButtons;
        [SerializeField] private TabContent[] _tabContents;
        
        private TabButton _selectedTabButton;
        private PlayerInput _playerInput;
        private GameStateManager _gameStateManager;
        
        
        private void Start()
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.Initiate(this);
            }
            
            var inputData = ServiceLocator.Resolve<InputData>();
            _playerInput = inputData.playerInput;
            _gameStateManager = ServiceLocator.Resolve<GameStateManager>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["NextTab"].WasPressedThisFrame())
            {
                NextTab();
            }
            else if (_playerInput.actions["PreviousTab"].WasPressedThisFrame())
            {
                PreviousTab();
            }
            
            
            if (_playerInput.actions["Character"].WasPressedThisFrame())
            {
                if (_selectedTabButton != null)
                {
                    if (_selectedTabButton.TabType == TabType.Character)
                    {
                        CloseTabGroup();
                        return;
                    }
                }
                OpenCertainTab(TabType.Character);
            }
            if (_playerInput.actions["Inventory"].WasPressedThisFrame())
            {
                if (_selectedTabButton != null)
                {
                    if (_selectedTabButton.TabType == TabType.Inventory)
                    {
                        CloseTabGroup();
                        return;
                    }
                }
                OpenCertainTab(TabType.Inventory);
            }
            else if (_playerInput.actions["Skills"].WasPressedThisFrame())
            {
                if (_selectedTabButton != null)
                {
                    if (_selectedTabButton.TabType == TabType.SkillsAndStats)
                    {
                        CloseTabGroup();
                        return;
                    }
                }

                OpenCertainTab(TabType.SkillsAndStats);
            }
            else if (_playerInput.actions["Map"].WasPressedThisFrame())
            {
                if (_selectedTabButton != null)
                {
                    if (_selectedTabButton.TabType == TabType.Map)
                    {
                        CloseTabGroup();
                        return;
                    }
                }

                OpenCertainTab(TabType.Map);
            }
            else if (_playerInput.actions["Escape"].WasPressedThisFrame())
            {
                if (_selectedTabButton != null)
                {
                    if (_selectedTabButton != null)
                    {
                        CloseTabGroup();
                        return;
                    }
                }

                OpenCertainTab(TabType.Escape);
            }
        }
        
        private void CloseTabGroup()
        {
            _tabGroupParent.SetActive(false);
            _selectedTabButton = null;
            
            _gameStateManager.SetGameState(GameStates.Gameplay);
            _hudParent.SetActive(true);
        }
        
        private void OpenSelectedTab()
        {
            _tabGroupParent.SetActive(true);
            
            var selectedTabType = _selectedTabButton.TabType;
            for (var index = 0; index < _tabContents.Length; index++)
            {
                var tabContent = _tabContents[index];
                if (tabContent.TabType == selectedTabType)
                {
                    tabContent.OnSelect();
                }
                else
                {
                    tabContent.OnDeselect();
                }
            }
            
            _gameStateManager.SetGameState(GameStates.Menu);
            _hudParent.SetActive(false);
        }

        #region Called by buttons

        public void OnTabSelectedWithClick(TabButton tabButton)
        {
            foreach (var button in _tabButtons)
            {
                button.OnDeselect();
            }
            
            _selectedTabButton = tabButton;
            tabButton.OnSelect();
            OpenSelectedTab();
        }

        #endregion

        #region Called by next/previous buttons
        
        private void PreviousTab()
        {
            var index = Array.IndexOf(_tabButtons, _selectedTabButton);

            TabButton nextButton;
            
            if (index == _tabButtons.Length - 1)
            {
                nextButton = _tabButtons[0];
            }
            else
            {
                nextButton = _tabButtons[index + 1];
            }
            
            OpenCertainTab(nextButton.TabType);
        }
        
        private void NextTab()
        {
            var index = Array.IndexOf(_tabButtons, _selectedTabButton);
            TabButton nextButton;
            
            if (index == 0)
            {
                nextButton = _tabButtons[^1];
            }
            else
            {
                nextButton = _tabButtons[index - 1];
            }
            
            OpenCertainTab(nextButton.TabType);
        }

        #endregion

        #region Called by certain buttons

        private void OpenCertainTab(TabType tabType)
        {
            for (var i = 0; i < _tabButtons.Length; i++)
            {
                var button = _tabButtons[i];
                if (button.TabType == tabType)
                {
                    button.OnSelect();
                    _selectedTabButton = button;
                }
                else
                {
                    button.OnDeselect();
                }
            }

            OpenSelectedTab();
        }

        #endregion
    }
}