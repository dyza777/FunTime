using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Elpy.FunTime
{
    public class PlayerMono : MonoBehaviour
    {
        public float Health;
        public GameObject ArrowPrefab;
        public GameObject BulletPrefab;
        public float ArrowCoolingTime;
        public float BulletCoolingTime;
    }

    public class PlayerBaker : Baker<PlayerMono>
    {
        public override void Bake(PlayerMono authoring)
        {
            AddComponent<PlayerTag>();
            AddComponent(new PlayerHealth { Value = authoring.Health, Max = authoring.Health });
            AddBuffer<PlayerDamageBufferElement>();
            AddComponent(new PlayerProperties
            {
                ArrowPrefab = GetEntity(authoring.ArrowPrefab),
                BulletPrefab = GetEntity(authoring.BulletPrefab),
            });
            AddComponent(new ArrowCoolingData { CurrentValue = 100, CoolingTime = authoring.ArrowCoolingTime });
            AddComponent(new BulletCoolingData { CurrentValue = 100, CoolingTime = authoring.BulletCoolingTime });
        }
    }
}
