using Features.Talents;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.SkillsAndStats
{
    public class TalentButton : MonoBehaviour
    {
        [SerializeField] private Image _learnedEdge;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        
        private SkillsTabContent _skillsTabContent;
        private TalentData _talentData;

        private bool _isLearned;
        
        public void Initiate(SkillsTabContent skillsTabContent, TalentData talentData, bool isLearned)
        {
            _skillsTabContent = skillsTabContent;
            
            _talentData = talentData;
            _icon.sprite = _talentData.icon;
            
            _isLearned = isLearned;
            
            _learnedEdge.gameObject.SetActive(_isLearned);
            _button.interactable = !_isLearned;
        }
        
        public void SetInteractable(bool isInteractable)
        {
            if (_isLearned) return;
            _button.interactable = isInteractable;
        }
        
        public void OnClick()
        {
            _button.interactable = false;
            _learnedEdge.gameObject.SetActive(true);
            _skillsTabContent.OnLearnTalent(_talentData);
            _isLearned = true;
        }
    }
}