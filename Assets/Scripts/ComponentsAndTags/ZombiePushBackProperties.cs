using Unity.Entities;
using Unity.Mathematics;

namespace Elpy.FunTime
{
    public struct ZombiePushBackProperties : IComponentData, IEnableableComponent
    {
        public float PushBackFullDistance;
        public float PushBackSpeed;
        public float PushBackHeight;
    }

    public struct ZombiePushBackCurrentData : IComponentData, IEnableableComponent
    {
        public float PushBackCurrentDistance;
    }
}


