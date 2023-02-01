using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Elpy.FunTime
{
    public class GameFieldMono : MonoBehaviour
    {
        public int NumberOfSpawnPoints;
        public float2 GameFieldDimensions;
        public uint RandomSeed;
        public float SpawnRate;
        public GameObject SpawnPointPrefab;
        public GameObject ZombiePrefab;
    }

    public class GameFieldBaker : Baker<GameFieldMono>
    {
        public override void Bake(GameFieldMono authoring)
        {
            AddComponent(new GameFieldProperties
            {
                NumberOfSpawnPoints = authoring.NumberOfSpawnPoints,
                GameFieldDimensions = authoring.GameFieldDimensions,
                SpawnRate = authoring.SpawnRate,
                SpawnPointPrefab = GetEntity(authoring.SpawnPointPrefab),
                ZombiePrefab = GetEntity(authoring.ZombiePrefab)
            });
            AddComponent(new Randomizer
            {
                Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
            });
            AddComponent<ZombieSpawnPoints>();
            AddComponent<SpawnTimer>();
        }
    }
}
