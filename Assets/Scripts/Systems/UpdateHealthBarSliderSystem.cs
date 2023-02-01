using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Elpy.FunTime
{
    public partial class UpdateHealthBarSliderSystem : SystemBase
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
                .ForEach((Slider slider, HealthBarSliderData sliderData) => {
                    slider.value = playerAspect.PlayerHealthCurrent / playerAspect.PlayerHealthMax;
                })
                .WithoutBurst()
                .Run();
        }
    }
}