using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Enemies.Extensions
{
    public class UnitConfig : MonoBehaviour
    {
        [SerializeField] private string horizontalMoveParam = "Horizontal";
        [SerializeField] private string verticalMoveParam = "Vertical";
        
        [SerializeField] private string attackParam = "Attack";
        [SerializeField] private string attackIDParam = "AttackID";
        
        [Space]
        [SerializeField] private float baseSpeed;
        
        [Space]
        [SerializeField] private float baseDamage;
        
        
        
        private const float SpeedMultiplier = 100;
        
        
        
        public string HorizontalMoveParam => horizontalMoveParam;
        public string VerticalMoveParam => verticalMoveParam;
        
        public string AttackParam => attackParam;
        public string AttackIDParam => attackIDParam;
        
        public float BaseSpeed => baseSpeed * SpeedMultiplier;
        
        public float BaseDamage => baseDamage;
    }
}
