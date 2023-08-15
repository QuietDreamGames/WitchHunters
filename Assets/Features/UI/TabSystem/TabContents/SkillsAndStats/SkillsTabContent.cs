using Features.Character;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using Features.Stats;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.SkillsAndStats
{
    public class SkillsTabContent : TabContent
    {
        [SerializeField] private StatUpgradeUIController _strengthUpgradeUIControllers;
        [SerializeField] private StatUpgradeUIController _agilityUpgradeUIControllers;
        [SerializeField] private StatUpgradeUIController _intelligenceUpgradeUIControllers;
        
        [SerializeField] private Button _acceptButton;
        
        [SerializeField] private TextMeshProUGUI _unusedStatsPointsText;
        
        private CombatCharacterController _combatCharacterController;


        public StatsData BaseStatsData;
        public StatsData CurrentStatsData;
        

        public override void OnSelect()
        {
            base.OnSelect();

            var characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _combatCharacterController = characterHolder.CurrentCharacter;
            
            BaseStatsData = _combatCharacterController.StatsController.StatsData;
            CurrentStatsData = BaseStatsData.Clone();
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
            
            _strengthUpgradeUIControllers.Initiate(this);
            _agilityUpgradeUIControllers.Initiate(this);
            _intelligenceUpgradeUIControllers.Initiate(this);
            
            _acceptButton.gameObject.SetActive(false);
            _acceptButton.onClick.RemoveAllListeners();
            _acceptButton.onClick.AddListener(OnAccept);
        }

        public void OnAddPoint()
        {
            if (CurrentStatsData.unusedStatsPoints <= 0)
            {
                _strengthUpgradeUIControllers.SetAddPointsButton(false);
                _agilityUpgradeUIControllers.SetAddPointsButton(false);
                _intelligenceUpgradeUIControllers.SetAddPointsButton(false);
            }
            
            _acceptButton.gameObject.SetActive(true);
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }
        
        public void OnRemovePoint()
        {
            _strengthUpgradeUIControllers.SetAddPointsButton(true);
            _agilityUpgradeUIControllers.SetAddPointsButton(true);
            _intelligenceUpgradeUIControllers.SetAddPointsButton(true);
            
            if (CurrentStatsData.unusedStatsPoints >= BaseStatsData.unusedStatsPoints)
            {
                _acceptButton.gameObject.SetActive(false);
            }
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }

        public void OnAccept()
        {
            _combatCharacterController.StatsController.SetStatsData(CurrentStatsData);
            
            BaseStatsData = _combatCharacterController.StatsController.StatsData;
            CurrentStatsData = BaseStatsData.Clone();
            
            _strengthUpgradeUIControllers.Initiate(this);
            _agilityUpgradeUIControllers.Initiate(this);
            _intelligenceUpgradeUIControllers.Initiate(this);
            
            _acceptButton.gameObject.SetActive(false);
            
            _unusedStatsPointsText.text = CurrentStatsData.unusedStatsPoints.ToString();
        }
    }
}