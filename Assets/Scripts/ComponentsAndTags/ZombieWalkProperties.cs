using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct ZombieWalkProperties : IComponentData, IEnableableComponent
    {
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
    }
}


