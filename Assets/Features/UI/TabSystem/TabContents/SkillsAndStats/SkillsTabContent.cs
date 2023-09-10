using System.Collections.Generic;
using Features.Character;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using Features.Stats;
using Features.Talents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.SkillsAndStats
{
    public class SkillsTabContent : TabContent
    {
        [Header("Skills")] [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private TalentPanelUIController _talentPanelPrefab;
        [SerializeField] private TextMeshProUGUI _talentPointsText;
        
        [Header("Stats")]
        [SerializeField] private StatUpgradeUIController _strengthUpgradeUIControllers;
        [SerializeField] private StatUpgradeUIController _agilityUpgradeUIControllers;
        [SerializeField] private StatUpgradeUIController _intelligenceUpgradeUIControllers;
        
        [SerializeField] private Button _acceptStatsButton;
        
        [SerializeField] private TextMeshProUGUI _unusedStatsPointsText;
        
        private CombatCharacterController _combatCharacterController;
        
        public StatsData BaseStatsData;
        public StatsData CurrentStatsData;
        
        private List<TalentPanelUIController> _talentPanels;
        private TalentPanelUIController _lastTalentPanel;
        
        public override void OnSelect()
        {
            base.OnSelect();

            var characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _combatCharacterController = characterHolder.CurrentCharacter;

            InitiateStats();
            
            InitiateTalents();
        }

        #region Skills

        private void InitiateTalents()
        {
            var talentsList = _combatCharacterController.TalentsController.TalentsList;
            var currentTalents = _combatCharacterController.TalentsController.TalentsData;
            
            foreach (Transform child in _scrollViewContent) { Destroy(child.gameObject); }
            
            _talentPanels = new List<TalentPanelUIController>();
            _lastTalentPanel = Instantiate(_talentPanelPrefab, _scrollViewContent);
            _talentPanels.Add(_lastTalentPanel);
            
            foreach (var talent in talentsList.talents)
            {
                if (_lastTalentPanel.InitiateNextButton(this, talent, IsTalentLearned(talent))) continue;
                
                _lastTalentPanel = Instantiate(_talentPanelPrefab, _scrollViewContent);
                _talentPanels.Add(_lastTalentPanel);
                _lastTalentPanel.InitiateNextButton(this, talent, IsTalentLearned(talent));
            }
            
            _lastTalentPanel = null;
            
            foreach (var panel in _talentPanels)
            {
                panel.SetInteractable(currentTalents.talentPoints > 0);
            }
            
            _talentPointsText.text = currentTalents.talentPoints.ToString();
        }
        
        public void OnLearnTalent(TalentData talentData)
        {
            _combatCharacterController.TalentsController.LearnTalent(talentData);
            var currentTalents = _combatCharacterController.TalentsController.TalentsData;
            
            foreach (var panel in _talentPanels)
            {
                panel.SetInteractable(currentTalents.talentPoints > 0);
            }
            
            _talentPointsText.text = currentTalents.talentPoints.ToString();
        }

        private bool IsTalentLearned(TalentData talentData)
        {
            var learnedTalents = _combatCharacterController.TalentsController.TalentsData.learnedTalents;

            for (int i = 0; i < learnedTalents.Count; i++)
            {
                if (learnedTalents[i].talentId == talentData.talentId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Stats

        private void InitiateStats()
        {
            BaseStatsData = _combatCharacterController.StatsController.StatsData;
            CurrentStatsData = BaseStatsData.Clone();
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
            
            _strengthUpgradeUIControllers.Initiate(this);
            _agilityUpgradeUIControllers.Initiate(this);
            _intelligenceUpgradeUIControllers.Initiate(this);
            
            _acceptStatsButton.gameObject.SetActive(false);
            _acceptStatsButton.onClick.RemoveAllListeners();
            _acceptStatsButton.onClick.AddListener(OnAcceptStats);
        }

        public void OnAddStatPoint()
        {
            if (CurrentStatsData.unusedStatsPoints <= 0)
            {
                _strengthUpgradeUIControllers.SetAddPointsButton(false);
                _agilityUpgradeUIControllers.SetAddPointsButton(false);
                _intelligenceUpgradeUIControllers.SetAddPointsButton(false);
            }
            
            _acceptStatsButton.gameObject.SetActive(true);
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }
        
        public void OnRemoveStatPoint()
        {
            _strengthUpgradeUIControllers.SetAddPointsButton(true);
            _agilityUpgradeUIControllers.SetAddPointsButton(true);
            _intelligenceUpgradeUIControllers.SetAddPointsButton(true);
            
            if (CurrentStatsData.unusedStatsPoints >= BaseStatsData.unusedStatsPoints)
            {
                _acceptStatsButton.gameObject.SetActive(false);
            }
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }

        public void OnAcceptStats()
        {
            _combatCharacterController.StatsController.SetStatsData(CurrentStatsData);
            
            BaseStatsData = _combatCharacterController.StatsController.StatsData;
            CurrentStatsData = BaseStatsData.Clone();
            
            _strengthUpgradeUIControllers.Initiate(this);
            _agilityUpgradeUIControllers.Initiate(this);
            _intelligenceUpgradeUIControllers.Initiate(this);
            
            _acceptStatsButton.gameObject.SetActive(false);
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }

        #endregion
        
    }
}