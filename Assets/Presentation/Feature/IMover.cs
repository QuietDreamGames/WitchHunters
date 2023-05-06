using UnityEngine;

namespace Presentation.Feature
{
    public interface IMover
    {
        Transform Center { get; set; }
        Vector2 Size { get; set; }
        
        Vector2 WaitRange { get; set; }
    
        float Speed { get; set; }
        
        void Spawn();
    }
}
