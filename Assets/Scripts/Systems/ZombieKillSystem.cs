using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct ZombieKillSystem : ISystem
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
            if (!SystemAPI.TryGetSingleton(out ecbSingleton))
            {
                return;
            }

            new ZombieKillJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            }.Run();
        }
    }


    public partial struct ZombieKillJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        private void Execute(ZombieKillAspect zombie, [EntityIndexInQuery] int sortKey)
        {
            if (zombie.isFallingCompleted)
            {
                ECB.DestroyEntity(sortKey, zombie.entity);
            } else
            {
                zombie.Fall(DeltaTime);
            }
        }
    }
}
