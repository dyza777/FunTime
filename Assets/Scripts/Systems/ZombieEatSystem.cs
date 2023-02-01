using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct ZombieEatSystem : ISystem
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
            var playerAspect = SystemAPI.GetAspectRO<PlayerAspect>(player);


            new ZombieEatJob
            {
                DeltaTime = deltaTime,
                ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                playerEntity = player,
                playerPosition = playerAspect.Position,
                EatingRadiusSq = 5f,
                zombiePushedLookup = SystemAPI.GetComponentLookup<ZombiePushed>(),
                zombieKilledLookup = SystemAPI.GetComponentLookup<ZombieKilled>()
            }.Run();
        }
    }

    public partial struct ZombieEatJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public Entity playerEntity;
        public float3 playerPosition;
        public float EatingRadiusSq;

        [ReadOnly]
        public ComponentLookup<ZombiePushed> zombiePushedLookup;

        [ReadOnly]
        public ComponentLookup<ZombieKilled> zombieKilledLookup;
        private void Execute(ZombieEatAspect zombie, [EntityIndexInQuery] int sortKey)
        {

            if (zombiePushedLookup.HasComponent(zombie.entity))
            {
                ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, false);
                ecb.SetComponentEnabled<ZombiePushBackProperties>(sortKey, zombie.entity, true);
            } else if (zombieKilledLookup.HasComponent(zombie.entity))
            {
                ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, false);
                ecb.SetComponentEnabled<ZombieKillProperties>(sortKey, zombie.entity, true);
            }
            if (zombie.IsInEatingRange(playerPosition, EatingRadiusSq))
            {
                zombie.Eat(DeltaTime, ecb, sortKey, playerEntity);
            }
            else
            {
                ecb.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.entity, false);
                ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, true);
            }
        }
    }
}
