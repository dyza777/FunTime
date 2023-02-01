using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Elpy.FunTime
{
    public readonly partial struct ArrowFlyAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transform;
        private readonly RefRW<ArrowFlyProperties> _arrowFlySpeed;
        private readonly RefRO<ProjectileHeading> _heading;

        private float FlySpeed => _arrowFlySpeed.ValueRO.FlySpeed;
        private float Heading => _heading.ValueRO.Value;

        public void Fly(float deltaTime)
        {
            _transform.LocalPosition += _transform.Forward * FlySpeed * deltaTime;
        }

        public bool IsInEatingRange(float3 playerPosition, float eatingRadiusSq)
        {
            return math.distancesq(playerPosition, _transform.LocalPosition) <= eatingRadiusSq - 1;
        }
    }
}
