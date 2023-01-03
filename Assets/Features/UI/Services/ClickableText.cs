using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Services
{
    public class ClickableText : Selectable
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Color _downColor;

        [Header("Navigation")]
        [SerializeField] private Selectable _upSelectable;

        [SerializeField] private Selectable _downSelectable;

        [SerializeField] private Selectable _leftSelectable;

        [SerializeField] private Selectable _rightSelectable;
        
        public Action OnClick;
        
        public override Selectable FindSelectableOnUp()
        {
            return _upSelectable != null ? _upSelectable : base.FindSelectableOnUp();
        }

        public override Selectable FindSelectableOnDown()
        {
            return _downSelectable != null ? _downSelectable : base.FindSelectableOnDown();
        }

        public override Selectable FindSelectableOnLeft()
        {
            return _leftSelectable != null ? _leftSelectable : base.FindSelectableOnLeft();
        }

        public override Selectable FindSelectableOnRight()
        {
            return _rightSelectable != null ? _rightSelectable : base.FindSelectableOnRight();
        }
    }
}