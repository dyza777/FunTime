using Unity.Mathematics;

namespace Elpy.FunTime
{
    public static class MathHelpers
    {
        public static float GetHeading(float3 objectPosition, float3 targetPosition)
        {
            var x = objectPosition.x - targetPosition.x;
            var y = objectPosition.z - targetPosition.z;
            return math.atan2(x, y) + math.PI;
        }

        public static float GetHeadingNew(float4 objectPosition, float4 targetPosition)
        {
            var x = objectPosition.x - targetPosition.x;
            var y = objectPosition.z - targetPosition.z;
            return math.atan2(x, y) + math.PI;
        }
    }
}