using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    [BurstCompile]
    public partial struct CollisionEventSystem : ISystem
    {
        private ComponentLookup<ArrowFlyProperties> _arrowFlyPropertiesLookup;
        private ComponentLookup<ZombieHeading> _entityHeading;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
           // state.RequireForUpdate<ArrowFlyProperties>();
            _arrowFlyPropertiesLookup = SystemAPI.GetComponentLookup<ArrowFlyProperties>(true);
            _entityHeading = SystemAPI.GetComponentLookup<ZombieHeading>(true);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

      // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
          //  _arrowFlyPropertiesLookup.Update(ref state);
            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton;
            SimulationSingleton simulationSingleton;

            if (!SystemAPI.TryGetSingleton(out ecbSingleton) || !SystemAPI.TryGetSingleton(out simulationSingleton))
            {
                return;
            }


            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);


            var collisionJob = new CollisionEventJob { ecb = ecb };

            collisionJob.arrowFlyLookup = SystemAPI.GetComponentLookup<ArrowFlyProperties>(true);
            collisionJob.bulletFlyLookup = SystemAPI.GetComponentLookup<BulletFlyProperties>(true);
            collisionJob.entityHeading = SystemAPI.GetComponentLookup<ZombieHeading>(true);

            state.Dependency = collisionJob.Schedule(simulationSingleton, state.Dependency);
        }
    }

    public partial struct CollisionEventJob : ICollisionEventsJob
    {
        [ReadOnly]
        public ComponentLookup<ArrowFlyProperties> arrowFlyLookup;

        [ReadOnly]
        public ComponentLookup<BulletFlyProperties> bulletFlyLookup;

        [ReadOnly]
        public ComponentLookup<ZombieHeading> entityHeading;

        public EntityCommandBuffer ecb;
        public void Execute(CollisionEvent collisionEvent)
        {
            bool firstEntityIsArrow = arrowFlyLookup.HasComponent(collisionEvent.EntityA);
            bool secondEntityIsArrow = arrowFlyLookup.HasComponent(collisionEvent.EntityB);
            bool firstEntityIsBullet = bulletFlyLookup.HasComponent(collisionEvent.EntityA);
            bool secondEntityIsBullet = bulletFlyLookup.HasComponent(collisionEvent.EntityB);
            bool firstEntityIsZombie = entityHeading.HasComponent(collisionEvent.EntityA);
            bool secondEntityIsZombie= entityHeading.HasComponent(collisionEvent.EntityB);

            bool isArrowHit = firstEntityIsArrow && secondEntityIsZombie || firstEntityIsZombie && secondEntityIsArrow;
            bool isBulletHit = firstEntityIsBullet && secondEntityIsZombie || firstEntityIsZombie && secondEntityIsBullet;

            if (isArrowHit)
            {
                if (firstEntityIsZombie)
                {
                    ecb.AddComponent<ZombiePushed>(collisionEvent.EntityA);

                }
                else
                {
                    ecb.AddComponent<ZombiePushed>(collisionEvent.EntityB);
                }
            }
            else if (isBulletHit) {
                if (firstEntityIsZombie)
                {
                    ecb.AddComponent<ZombieKilled>(collisionEvent.EntityA);
                    ecb.AddComponent(collisionEvent.EntityA, new URPMaterialPropertyBaseColor { Value = new float4(1, 1, 1, 1) });
                    ecb.DestroyEntity(collisionEvent.EntityB);
                }
                else
                {
                    ecb.AddComponent<ZombieKilled>(collisionEvent.EntityB);
                    ecb.AddComponent(collisionEvent.EntityB, new URPMaterialPropertyBaseColor { Value = new float4(1, 1, 1, 1) });
                    ecb.DestroyEntity(collisionEvent.EntityA);
                }
            }
            else
            {
                if (firstEntityIsArrow)
                {
                    ecb.DestroyEntity(collisionEvent.EntityA);
                }

                if (secondEntityIsArrow)
                {
                    ecb.DestroyEntity(collisionEvent.EntityB);
                }

                if (secondEntityIsBullet)
                {
                    ecb.DestroyEntity(collisionEvent.EntityB);
                }

                if (firstEntityIsBullet)
                {
                    ecb.DestroyEntity(collisionEvent.EntityA);
                }
            }
        }
    }
}
