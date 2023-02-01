using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
   // [DisableAutoCreation]
    [BurstCompile]
    public partial struct ZombieWalkSystem : ISystem
    {
        private ComponentLookup<ZombiePushed> _zombiePushedLookup;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _zombiePushedLookup = SystemAPI.GetComponentLookup<ZombiePushed>();   
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
            var eatingRange = 5f;
            var playerAspect = SystemAPI.GetAspectRO<PlayerAspect>(player);


            new ZombieWalkJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                eatingRange = eatingRange,
                playerPosition = playerAspect.Position,
                zombiePushedLookup = SystemAPI.GetComponentLookup<ZombiePushed>(),
                zombieKilledLookup = SystemAPI.GetComponentLookup<ZombieKilled>()
            }.Run();
        }
    }

    public partial struct ZombieWalkJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public float eatingRange;
        public float3 playerPosition;

        [ReadOnly]
        public ComponentLookup<ZombiePushed> zombiePushedLookup;

        [ReadOnly]
        public ComponentLookup<ZombieKilled> zombieKilledLookup;

        private void Execute(ZombieWalkAspect zombie, [EntityIndexInQuery] int sortKey)
        {
            zombie.Move(DeltaTime);

            if (zombie.IsInStoppingRange(playerPosition, eatingRange))
            {
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, false);
                ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.entity, true);
            }

            if (zombiePushedLookup.HasComponent(zombie.entity)) {
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, false);
                ECB.SetComponentEnabled<ZombiePushBackProperties>(sortKey, zombie.entity, true);
            }

            if (zombieKilledLookup.HasComponent(zombie.entity))
            {
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, false);
                ECB.SetComponentEnabled<ZombieKillProperties>(sortKey, zombie.entity, true);
            }
        }
    }
}
