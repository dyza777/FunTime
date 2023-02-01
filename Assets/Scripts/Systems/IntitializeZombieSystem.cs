using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [BurstCompile]
    public partial struct IntitializeZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameFieldProperties>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var zombie in SystemAPI.Query<ZombiePushBackAspect>().WithAll<NewZombieTag>())
            {
                ecb.RemoveComponent<NewZombieTag>(zombie.entity);
                ecb.SetComponentEnabled<ZombieEatProperties>(zombie.entity, false);
                ecb.SetComponentEnabled<ZombiePushBackProperties>(zombie.entity, false);
                ecb.SetComponentEnabled<ZombieKillProperties>(zombie.entity, false);
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
