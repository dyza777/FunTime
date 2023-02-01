using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct ZombieSpawnSystem : ISystem
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
            var deltaTime = SystemAPI.Time.DeltaTime;
            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton;

            if (!SystemAPI.TryGetSingleton(out ecbSingleton))
            {
                return;
            }


            new SpawnEntityJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }


    public partial struct SpawnEntityJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        public const float ZOMBIE_OFFSET = 0.1f;
        private void Execute(GameFieldAspect gameField)
        {
            gameField.SpawnTimer -= DeltaTime;
            if (!gameField.TimeToSpawn  || gameField.ZombieSpawnPoints.Length <= 0) return;
            gameField.SpawnTimer = gameField.SpawnRate;
            var newEntity = ECB.Instantiate(gameField.ZombiePrefab);

            var newEntityTransform = gameField.GetRandomSpawnPoint();
            Debug.Log(newEntityTransform);
            ECB.SetComponent(newEntity, new LocalTransform { Position = newEntityTransform, Scale = 1f, Rotation = quaternion.identity });

            var zombieHeading = MathHelpers.GetHeading(newEntityTransform, gameField.Position);
            ECB.SetComponent(newEntity, new ZombieHeading { Value = zombieHeading });

        }
    }
}
