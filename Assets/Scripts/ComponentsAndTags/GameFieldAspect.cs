using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct GameFieldAspect : IAspect
    {
        public readonly Entity entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRO<GameFieldProperties> _gameFieldProperties;
        private readonly RefRW<Randomizer> _randomizer;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<SpawnTimer> _spawnTimer;
        public int NumberOfSpawnPoints => _gameFieldProperties.ValueRO.NumberOfSpawnPoints;
        public float SpawnTimer
        {
            get => _spawnTimer.ValueRO.Value;
            set => _spawnTimer.ValueRW.Value = value;
        }

        public bool TimeToSpawn => SpawnTimer <= 0f;
        public float SpawnRate => _gameFieldProperties.ValueRO.SpawnRate;
        public float3 Position => _transformAspect.LocalPosition;
        public Entity ZombiePrefab => _gameFieldProperties.ValueRO.ZombiePrefab;

        public NativeArray<float3> ZombieSpawnPoints
        {
            get => _zombieSpawnPoints.ValueRO.Value;
            set => _zombieSpawnPoints.ValueRW.Value = value;
        }

        
        public Entity SpawnPointPrefab => _gameFieldProperties.ValueRO.SpawnPointPrefab;

        public float3 GetRandomSpawnPointPosition()
        {
            return GetRandomPosition();
        }
        private float3 GetRandomPosition()
        {
            float3 randomPosition;

            do { randomPosition = _randomizer.ValueRW.Value.NextFloat3(MinCorner, MaxCorner); }
            while (math.distancesq(_transformAspect.LocalPosition, randomPosition) <= SPAWN_RADIUS_SQ);
            return randomPosition;
        }

        public float3 GetRandomSpawnPoint()
        {
            var spawnPointIndex = _randomizer.ValueRW.Value.NextInt(ZombieSpawnPoints.Length > 1 ? ZombieSpawnPoints.Length : ZombieSpawnPoints.Length);
            return ZombieSpawnPoints[spawnPointIndex];
        }

        public float3 GetSpawnPointTransform()
        {
            return GetRandomSpawnPoint();
        }

        private const float SPAWN_RADIUS_SQ = 15f;

        private float3 MinCorner => _transformAspect.LocalPosition - HalfDimensions;
        private float3 MaxCorner => _transformAspect.LocalPosition + HalfDimensions;

        private float3 HalfDimensions => new()
        {
            x = _gameFieldProperties.ValueRO.GameFieldDimensions.x * 0.5f,
            y = 0,
            z = _gameFieldProperties.ValueRO.GameFieldDimensions.y * 0.5f
        };
    }

}

