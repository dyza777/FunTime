using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public partial class PlayerDieSystem : SystemBase
    {
        private GameOverScreenTag GameOverScreen;
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            GameOverScreen = GameObject.FindObjectOfType<GameOverScreenTag>(true);
        }

        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity player))
            {
                return;
            }



            var playerAspect = SystemAPI.GetAspectRO<PlayerAspect>(player);

            if (playerAspect.PlayerHealthCurrent <= 0)
            {
                GameOverScreen.gameObject.SetActive(true);
                World.Unmanaged.GetExistingSystemState<CreateSpawnPointsSystem>().Enabled = false;

                this.Enabled = false;
                
            }
        }
    }
}