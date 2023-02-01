using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct ZombieKillAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRO<ZombieKillProperties> _zombieKillProperties;
        private readonly RefRW<ZombieKillCurrentData> _zombieKillCurrentData;
        private readonly RefRO<ZombieHeading> _heading;
        private readonly RefRW<ZombieTimer> _walkTimer;

        private float KillFallSpeed => _zombieKillProperties.ValueRO.KillFallSpeed;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        private float currentAngle
        {
            get => _zombieKillCurrentData.ValueRO.currentFallAngle;
            set => _zombieKillCurrentData.ValueRW.currentFallAngle = value;
        }

        public void Fall(float deltaTime)
        {
            Debug.Log("falling");
            WalkTimer += deltaTime;

            currentAngle -= KillFallSpeed * deltaTime;
            Debug.Log(currentAngle);
            _transformAspect.LocalRotation = quaternion.Euler(currentAngle, Heading, 0);
        }

        public bool isFallingCompleted => currentAngle <= -1;
    }

}

