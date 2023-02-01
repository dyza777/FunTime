using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct BulletFlySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton;
            Entity player;

            if (!SystemAPI.TryGetSingleton(out ecbSingleton) || !SystemAPI.TryGetSingletonEntity<PlayerTag>(out player))
            {
                return;
            }

            new BulletFlyJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.Run();
        }
    }

    public partial struct BulletFlyJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public float eatingRange;
        public float3 playerPosition;
        private void Execute(BulletFlyAspect bullet, [EntityIndexInQuery] int sortKey)
        {
            bullet.Fly(DeltaTime);
        }
    }
}
