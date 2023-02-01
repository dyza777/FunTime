using Unity.Entities;

namespace Elpy.FunTime
{
    public struct SpawnZombieData : IComponentData
    {
        public Entity EntityPrefab;
        public float SpawnRate;
    }
}


