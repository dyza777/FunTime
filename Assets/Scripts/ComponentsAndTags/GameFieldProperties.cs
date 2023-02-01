using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct GameFieldProperties : IComponentData
    {
        public int NumberOfSpawnPoints;
        public Entity SpawnPointPrefab;
        public float2 GameFieldDimensions;
        public Entity ZombiePrefab;
        public float SpawnRate;
    }
}


