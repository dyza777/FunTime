using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct ArrowCoolingData : IComponentData
    {
        public float CurrentValue;
        public float CoolingTime;
    }
}


