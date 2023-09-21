using Features.Modifiers.SOLID.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Enemies.Extensions
{
    public class UnitConfig : MonoBehaviour
    {
        [Space]
        [SerializeField] private string horizontalMoveParam = "Horizontal";
        [SerializeField] private string verticalMoveParam = "Vertical";
        
        [Space]
        [SerializeField] private bool useMovementValueParam = false;
        [SerializeField] private string horizontalValueParam = "HorizontalValue";
        [SerializeField] private string verticalValueParam = "VerticalValue";
        
        [Space]
        [SerializeField] private string attackParam = "Attack";
        [SerializeField] private string attackIDParam = "AttackID";
        
        [Space]
        [SerializeField] private float baseSpeed;
        [SerializeField] private float speedMultiplier = 100;
        
        [Space]
        [SerializeField] private float baseDamage;
        
        
        private readonly ModifiersContainer _modifiersController = new();
        
        
        public ModifiersContainer ModifiersController => _modifiersController;
        
        public string HorizontalMoveParam => horizontalMoveParam;
        public string VerticalMoveParam => verticalMoveParam;
        
        public bool UseMovementValueParam => useMovementValueParam;
        public string HorizontalValueParam => horizontalValueParam;
        public string VerticalValueParam => verticalValueParam;
        
        public string AttackParam => attackParam;
        public string AttackIDParam => attackIDParam;
        
        public float BaseSpeed => baseSpeed * speedMultiplier;
        public float SpeedMultiplier => speedMultiplier;
        
        public float BaseDamage => baseDamage;
    }
}
