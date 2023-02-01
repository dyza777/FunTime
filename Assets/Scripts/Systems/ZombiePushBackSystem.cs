using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct ZombiePushBackSystem : ISystem
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

            new ZombiePushBackJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            }.Run();
        }
    }

    public partial struct ZombiePushBackJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        private void Execute(ZombiePushBackAspect zombie, [EntityIndexInQuery] int sortKey)
        {
            if (zombie.isPushBackCompleted)
            {
                ECB.RemoveComponent<ZombiePushed>(sortKey, zombie.entity);
                zombie.ResetCurrentDistance();
                ECB.SetComponentEnabled<ZombiePushBackProperties>(sortKey, zombie.entity, false);
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, true);
            } else
            {
                zombie.PushBack(DeltaTime);
            }
        }
    }
}
