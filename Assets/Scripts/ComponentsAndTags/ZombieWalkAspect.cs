using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRO<ZombieWalkProperties> _zombieWalkProperties;
        private readonly RefRO<ZombieHeading> _heading;
        private readonly RefRW<ZombieTimer> _walkTimer;

        private float WalkSpeed => _zombieWalkProperties.ValueRO.WalkSpeed;
        private float WalkAmplitude => _zombieWalkProperties.ValueRO.WalkAmplitude;
        private float WalkFrequency => _zombieWalkProperties.ValueRO.WalkFrequency;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        public void Move(float deltaTime)
        {
            WalkTimer += deltaTime;
            _transformAspect.LocalPosition += _transformAspect.Forward * WalkSpeed * deltaTime;

            var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
            _transformAspect.LocalRotation = quaternion.Euler(0, Heading, swayAngle);
        }

        public bool IsInStoppingRange(float3 playerPosition, float eatingRadiusSq)
        {
            return math.distancesq(playerPosition, _transformAspect.LocalPosition) <= eatingRadiusSq;
        }

        public bool IsHitWithArrow(float3 playerPosition)
        {
            return math.distancesq(playerPosition, _transformAspect.LocalPosition) <= 15;
        }

        public float3 Position => _transformAspect.LocalPosition;
    }

}

