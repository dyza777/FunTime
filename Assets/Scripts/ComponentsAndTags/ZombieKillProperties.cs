using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct ZombieKillProperties : IComponentData, IEnableableComponent
    {
        public float KillFallSpeed;
    }

    public struct ZombieKillCurrentData : IComponentData
    {
        public float currentFallAngle;
    }
}


