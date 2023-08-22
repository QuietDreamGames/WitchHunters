using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.SkillsAndStats
{
    public class StatUpgradeUIController : MonoBehaviour
    {
        [SerializeField] private Button _addPointButton;        
        [SerializeField] private Button _removePointButton;        
        
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private Color _normalTextColor;
        [SerializeField] private Color _addedPointsTextColor;

        [SerializeField] private int _statType;

        private int _addedPoints;
        
        private SkillsTabContent _skillsTabContent;

        public void Initiate(SkillsTabContent skillsTabContent)
        {
            _skillsTabContent = skillsTabContent;

            switch (_statType)
            {
                case 0:
                    _pointsText.text = _skillsTabContent.CurrentStatsData.strength.ToString();
                    break;
                case 1:
                    _pointsText.text = _skillsTabContent.CurrentStatsData.agility.ToString();
                    break;
                case 2:
                    _pointsText.text = _skillsTabContent.CurrentStatsData.intelligence.ToString();
                    break;
            }
            
            _addedPoints = 0;
            
            Reset();

            SetAddPointsButton(_skillsTabContent.CurrentStatsData.unusedStatsPoints != 0);
        }
        
        public void OnAddPoint()
        {
            switch (_statType)
            {
                case 0:
                    _skillsTabContent.CurrentStatsData.strength++;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.strength.ToString();
                    break;
                case 1:
                    _skillsTabContent.CurrentStatsData.agility++;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.agility.ToString();
                    break;
                case 2:
                    _skillsTabContent.CurrentStatsData.intelligence++;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.intelligence.ToString();
                    break;
            }
            
            _skillsTabContent.CurrentStatsData.unusedStatsPoints--;
            _addedPoints++;
            _pointsText.color = _addedPointsTextColor;
            SetRemovePointsButton(true);
            
            _skillsTabContent.OnAddStatPoint();
        }
        
        public void OnRemovePoint()
        {
            switch (_statType)
            {
                case 0:
                    _skillsTabContent.CurrentStatsData.strength--;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.strength.ToString();
                    break;
                case 1:
                    _skillsTabContent.CurrentStatsData.agility--;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.agility.ToString();
                    break;
                case 2:
                    _skillsTabContent.CurrentStatsData.intelligence--;
                    _pointsText.text = _skillsTabContent.CurrentStatsData.intelligence.ToString();
                    break;
            }
            
            _addedPoints--;
            _skillsTabContent.CurrentStatsData.unusedStatsPoints++;
            
            if (_addedPoints == 0)
            {
                _pointsText.color = _normalTextColor;
                SetRemovePointsButton(false);
            }
            
            _skillsTabContent.OnRemoveStatPoint();
        }
        
        public void SetAddPointsButton(bool value)
        {
            _addPointButton.gameObject.SetActive(value);
        }
        
        public void SetRemovePointsButton(bool value)
        {
            _removePointButton.gameObject.SetActive(value);
        }
        
        public void Reset()
        {
            _addPointButton.gameObject.SetActive(false);
            _removePointButton.gameObject.SetActive(false);
            
            _pointsText.color = _normalTextColor;
        }
    }
}