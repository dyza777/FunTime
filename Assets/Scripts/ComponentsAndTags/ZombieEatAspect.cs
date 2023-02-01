using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct ZombieEatAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transform;
        private readonly RefRW<ZombieTimer> _zombieTimer;
        private readonly RefRO<ZombieEatProperties> _eatProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float EatDamagePerSecond => _eatProperties.ValueRO.EatDamagePerSecond;
        private float EatAmplitude => _eatProperties.ValueRO.EatAmplitude;
        private float EatFrequency => _eatProperties.ValueRO.EatFrequency;
        private float Heading => _heading.ValueRO.Value;

        private float ZombieTimer
        {
            get => _zombieTimer.ValueRO.Value;
            set => _zombieTimer.ValueRW.Value = value;
        }

        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEntity)
        {
            ZombieTimer += deltaTime;

            var eatAngle = EatAmplitude * math.sin(EatFrequency * ZombieTimer);
            _transform.LocalRotation = quaternion.Euler(eatAngle, Heading, 0);

            var eatDamage = EatDamagePerSecond * deltaTime;
            var curBrainDamage = new PlayerDamageBufferElement { Value = eatDamage };
            ecb.AppendToBuffer(sortKey, brainEntity, curBrainDamage);
        }

        public bool IsInEatingRange(float3 playerPosition, float eatingRadiusSq)
        {
            return math.distancesq(playerPosition, _transform.LocalPosition) <= eatingRadiusSq - 1.5f;
        }
    }
}
