using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct PlayerProperties : IComponentData
    {
        public Entity ArrowPrefab;
        public Entity BulletPrefab;
    }
}


