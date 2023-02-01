using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Elpy.FunTime
{
    public partial class UpdateArrowSliderSystem : SystemBase
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
                .ForEach((Slider slider, ArrowCoolingSliderData coolingData) => {
                    slider.value = playerAspect.currentArrowCoolingValue / playerAspect.arrowCoolingTime;
                })
                .WithoutBurst()
                .Run();
        }
    }
}