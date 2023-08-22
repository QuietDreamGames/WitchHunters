using System.Collections.Generic;
using Features.Talents;
using UnityEngine;

namespace Features.UI.TabSystem.TabContents.SkillsAndStats
{
    public class TalentPanelUIController : MonoBehaviour
    {
        [SerializeField] private TalentButton _talentButtonPrefab;
        [SerializeField] private int _capacity = 4;
        
        private List<TalentButton> _talentButtons;
        
        public bool InitiateNextButton(SkillsTabContent skillsTabContent, TalentData talentData, bool isLearned)
        {
            _talentButtons ??= new List<TalentButton>();
            
            if (_talentButtons.Count >= _capacity)
            {
                return false;
            }
            
            var talentButton = Instantiate(_talentButtonPrefab, transform);
            talentButton.Initiate(skillsTabContent, talentData, isLearned);
            _talentButtons.Add(talentButton);

            return true;
        }
        
        public void SetInteractable(bool isInteractable)
        {
            foreach (var talentButton in _talentButtons)
            {
                talentButton.SetInteractable(isInteractable);
            }
        }
    }
}