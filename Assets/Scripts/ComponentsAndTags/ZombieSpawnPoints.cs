using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    [ChunkSerializable]
    public struct ZombieSpawnPoints : IComponentData
    {
        public NativeArray<float3> Value;
    }
}


