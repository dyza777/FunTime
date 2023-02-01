using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct ZombiePushBackAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRO<ZombiePushBackProperties> _zombiePushBackProperties;
        private readonly RefRW<ZombiePushBackCurrentData> _zombiePushBackCurrentData;
        private readonly RefRO<ZombieHeading> _heading;
        private readonly RefRW<ZombieTimer> _walkTimer;

        private float PushBackFullDistance => _zombiePushBackProperties.ValueRO.PushBackFullDistance;
        private float PushBackSpeed => _zombiePushBackProperties.ValueRO.PushBackSpeed;
        private float PushBackHeight => _zombiePushBackProperties.ValueRO.PushBackHeight;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        private float PushBackCurrentDistance
        {
            get => _zombiePushBackCurrentData.ValueRW.PushBackCurrentDistance;
            set => _zombiePushBackCurrentData.ValueRW.PushBackCurrentDistance = value;
        }

        public void PushBack(float deltaTime)
        {
            WalkTimer += deltaTime;
            float moveDistance = PushBackSpeed * deltaTime;
            PushBackCurrentDistance += moveDistance;
            float currentPushBackPercent = PushBackCurrentDistance / PushBackFullDistance;

            if (currentPushBackPercent > 0.5f)
            {
                currentPushBackPercent = currentPushBackPercent - 1;
            }

            float zombieCurrentHeight = PushBackHeight * currentPushBackPercent;

            float3 newPosition = new float3(
                _transformAspect.Position.x - _transformAspect.Forward.x * moveDistance,
                _transformAspect.Position.y + zombieCurrentHeight,
                _transformAspect.Position.z - _transformAspect.Forward.z * moveDistance);

            _transformAspect.Position = newPosition;
            _transformAspect.Rotation = quaternion.Euler(0, Heading, 0);
        }

        public void ResetCurrentDistance ()
        {
            PushBackCurrentDistance = 0;
        }

        public bool isPushBackCompleted => PushBackCurrentDistance >= PushBackFullDistance;
    }

}

