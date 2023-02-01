using Unity.Entities;
using UnityEngine;

namespace Elpy.FunTime
{
    public class ZombieSpawnerMono : MonoBehaviour
    {
        public GameObject EntityPrefab;
        public float SpawnRate;
    }

    public class EntitySpawnerBaker : Baker<ZombieSpawnerMono>
    {
        public override void Bake(ZombieSpawnerMono authoring)
        {
            AddComponent(new SpawnZombieData
            {
                EntityPrefab = GetEntity(authoring.EntityPrefab),
                SpawnRate = authoring.SpawnRate
            });
            AddComponent<SpawnerPointTag>();
        }
    }
}
