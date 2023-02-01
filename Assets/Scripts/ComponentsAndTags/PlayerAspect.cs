using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public readonly partial struct PlayerAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRW<PlayerHealth> _playerHealth;
        private readonly DynamicBuffer<PlayerDamageBufferElement> _playerDamageBuffer;
        private readonly RefRO<PlayerProperties> _playerProperties;
        private readonly RefRW<ArrowCoolingData> _arrowCoolingData;
        private readonly RefRW<BulletCoolingData> _bulletCoolingData;

        public Entity ArrowPrefab => _playerProperties.ValueRO.ArrowPrefab;
        public Entity BulletPrefab => _playerProperties.ValueRO.BulletPrefab;

        public float arrowCoolingTime
        {
            get => _arrowCoolingData.ValueRO.CoolingTime;
        }

        public float PlayerHealthCurrent
        {
            get => _playerHealth.ValueRO.Value;
            set => _playerHealth.ValueRW.Value = value;
        }

        public float PlayerHealthMax
        {
            get => _playerHealth.ValueRO.Max;
        }

        public float bulletCoolingTime
        {
            get => _bulletCoolingData.ValueRO.CoolingTime;
        }

        public float currentArrowCoolingValue
        {
            get => _arrowCoolingData.ValueRO.CurrentValue;
            set => _arrowCoolingData.ValueRW.CurrentValue = value;
        }

        public float currentBulletCoolingValue
        {
            get => _bulletCoolingData.ValueRO.CurrentValue;
            set => _bulletCoolingData.ValueRW.CurrentValue = value;
        }

        public void ResetPlayerData()
        {
            currentArrowCoolingValue = arrowCoolingTime;
            currentBulletCoolingValue = bulletCoolingTime;
            PlayerHealthCurrent = PlayerHealthMax;
        }

        public void DamageBrain()
        {
            foreach (var playerDamageBufferElements in _playerDamageBuffer)
            {
                _playerHealth.ValueRW.Value -= playerDamageBufferElements.Value;
            }
            _playerDamageBuffer.Clear();
        }

        public float3 Position => _transformAspect.LocalPosition;

        public void ShootArrow(EntityCommandBuffer ecb)
        {
            var arrow = ecb.Instantiate(ArrowPrefab);
            ecb.SetComponent(arrow, new LocalTransform { Position = _transformAspect.LocalPosition, Rotation = quaternion.identity });
        }

        public bool isArrowCooling => currentArrowCoolingValue < arrowCoolingTime;
        public bool isBulletCooling => currentBulletCoolingValue < bulletCoolingTime;

        public void ResetArrowCooling()
        {
            currentArrowCoolingValue = 0;
        }

        public void ResetBulletCooling()
        {
            currentBulletCoolingValue = 0;
        }

        public void CheckCoolings(float DeltaTime)
        {
            if (currentArrowCoolingValue < arrowCoolingTime) {
                currentArrowCoolingValue += DeltaTime;
            }

            if (currentBulletCoolingValue < bulletCoolingTime)
            {
                Debug.Log(currentBulletCoolingValue);
                currentBulletCoolingValue += DeltaTime;
            }
        }
    }
}
