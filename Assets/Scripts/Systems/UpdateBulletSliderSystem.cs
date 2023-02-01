using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Elpy.FunTime
{
    public partial class UpdateBulletSliderSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
        }

        protected override void OnUpdate()
        {
            Entity player;

            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out player))
            {
                return;
            }
            var playerAspect = SystemAPI.GetAspectRO<PlayerAspect>(player);

            Entities
                .ForEach((Slider slider, BulletCoolingSliderData coolingData) => {
                    slider.value = playerAspect.currentBulletCoolingValue / playerAspect.bulletCoolingTime;
                })
                .WithoutBurst()
                .Run();
        }
    }
}