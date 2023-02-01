using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct BulletCoolingData : IComponentData
    {
        public float CurrentValue;
        public float CoolingTime;
    }
}


