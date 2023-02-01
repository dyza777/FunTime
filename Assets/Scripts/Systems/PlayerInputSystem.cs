using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Scenes;

namespace Elpy.FunTime
{
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
        }

        const float ARROW_LEFT_SHIFT = 0.00f;
        const float BULLET_RIGHT_SHIFT = 0.00f;

        protected override void OnUpdate()
        {
            Entity player;

            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out player))
            {
                return;
            }
            var playerAspect = SystemAPI.GetAspectRW<PlayerAspect>(player);
            var cameraSingleton = CameraSingleton.Instance;




            if (Input.GetMouseButtonDown(0) && !playerAspect.isArrowCooling) {
                var arrow = EntityManager.Instantiate(playerAspect.ArrowPrefab);
                playerAspect.ResetArrowCooling();
                EntityManager.SetComponentData(arrow, new LocalTransform { Position = playerAspect.Position - new float3(ARROW_LEFT_SHIFT, 0, 0), Rotation = cameraSingleton.transform.rotation, Scale = 0.5f });
            } else if (Input.GetMouseButtonDown(1) && !playerAspect.isBulletCooling)
            {
                var bullet = EntityManager.Instantiate(playerAspect.BulletPrefab);
                var bulletRotation = cameraSingleton.transform.rotation;
                playerAspect.ResetBulletCooling();
                EntityManager.SetComponentData(bullet, new LocalTransform { Position = playerAspect.Position + new float3(BULLET_RIGHT_SHIFT, 0, 0), Rotation = bulletRotation, Scale = 0.5f });
            }
        }
    }
}