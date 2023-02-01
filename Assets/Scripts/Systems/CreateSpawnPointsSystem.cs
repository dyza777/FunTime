using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct CreateSpawnPointsSystem : ISystem
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
            state.Enabled = false;
            Entity gameFieldEntity;

            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton;

            if (!SystemAPI.TryGetSingletonEntity<GameFieldProperties>(out gameFieldEntity))
            {
                return;
            }

            var gameField = SystemAPI.GetAspectRW<GameFieldAspect>(gameFieldEntity);

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var spawnPoints = new NativeList<float3>(Allocator.Temp);


            for (int i = 0; i < gameField.NumberOfSpawnPoints; i++)
            {
                var newSpawnPoint = ecb.Instantiate(gameField.SpawnPointPrefab);
                var newSpawnPointPosition = gameField.GetRandomSpawnPointPosition();
             
                ecb.SetComponent(newSpawnPoint, new LocalTransform { Position = newSpawnPointPosition, Scale = 0.1f });
                spawnPoints.Add(newSpawnPointPosition);
            }
            gameField.ZombieSpawnPoints = spawnPoints.ToArray(Allocator.Persistent);

            ecb.Playback(state.EntityManager);
        }
    }
}
