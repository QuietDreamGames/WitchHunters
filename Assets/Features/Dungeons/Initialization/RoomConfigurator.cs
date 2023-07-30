using UnityEngine;

namespace Features.Dungeons.Initialization
{
    public class RoomConfigurator : MonoBehaviour
    {
        [SerializeField] private Transform generatedLevel;
        
        [Header("Parameters")]
        [SerializeField] private string tilemapsName = "Tilemaps";
        
        [Space]
        [SerializeField] private string floorName = "Floor";
        [SerializeField] private LayerMask floorLayer;
        
        [Space]
        [SerializeField] private string wallsName = "Walls";
        [SerializeField] private LayerMask wallsLayer;
        
        [ContextMenu("Configure room")]
        public void ConfigureRoom()
        {
            var tilemaps = generatedLevel.Find(tilemapsName);
            
            var floor = tilemaps.Find(floorName);
            floor.gameObject.layer = GetLayerNumber(floorLayer);
            
            var walls = tilemaps.Find(wallsName);
            walls.gameObject.layer = GetLayerNumber(wallsLayer);
        }
        
        private static int GetLayerNumber(LayerMask mask)
        {
            var bitmask = mask.value;
            var result = bitmask > 0 ? 0 : 31;
            while (bitmask > 1)
            {
                bitmask = bitmask >> 1;
                result++;
            }
            return result;
        }
    }
}
