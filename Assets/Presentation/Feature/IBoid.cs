namespace Presentation.Feature
{
    public interface IBoid
    {
        FlockEntitySpawner Flock { get; set; }
        
        void Spawn();
    }
}