using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using Unity.Scenes;
using Unity.Collections;

namespace Elpy.FunTime
{
    public class ReloadManagement : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverScreen;
        public void OnPressTryAgain()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var disposeEntitiesQueryDesc = new EntityQueryDesc
            {
                Any = new ComponentType[]
                { 
                    typeof(ArrowFlyProperties),
                    typeof(BulletFlyProperties),
                    typeof(ZombieHeading),
                    typeof(SpawnerPointTag)
                }
            };
            var disposeEntitiesQuery = entityManager.CreateEntityQuery(disposeEntitiesQueryDesc);
            var entitiesArray = disposeEntitiesQuery.ToEntityArray(Allocator.Temp);
            World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(entitiesArray);

            var playerQuery = entityManager.CreateEntityQuery(new ComponentType[] { typeof(PlayerTag) });
            playerQuery.TryGetSingletonEntity<PlayerTag>(out Entity player);
            var playerAspect = entityManager.GetAspect<PlayerAspect>(player);
            playerAspect.ResetPlayerData();

            World.DefaultGameObjectInjectionWorld.Unmanaged.GetExistingSystemState<CreateSpawnPointsSystem>().Enabled = true;
            var playerDieSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerDieSystem>();
            playerDieSystem.Enabled = true;

            GameOverScreen.SetActive(false);
        }
    }
}
