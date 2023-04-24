using System;
using UnityEngine;

namespace Features.Scene.Services
{
    public class EnemyPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private Transform[] _enemySpawnPoints;
        
        [SerializeField] private Vector2Int _enemyRange = new(1, 3);

        private void Start()
        {
            var enemyCount = UnityEngine.Random.Range(_enemyRange.x, _enemyRange.y + 1);
            var spawnPoints = Shuffle(_enemySpawnPoints);
            var count = spawnPoints.Length > enemyCount ? enemyCount : spawnPoints.Length;

            for (var i = 0; i < count; i++)
            {
                var enemyPrefab = _enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)];
                var enemySpawnPoint = spawnPoints[i];

                Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            }
        }
        
        public T[] Shuffle<T>(T[] source) 
        {
            var destination = new T[source.Length];
            Array.Copy(source, destination, source.Length);
            
            for (var i = 0; i < destination.Length; i++) 
            {
                var rnd = UnityEngine.Random.Range(0, destination.Length);
                (destination[rnd], destination[i]) = (destination[i], destination[rnd]);
            }

            return destination;
        }
    }
}
