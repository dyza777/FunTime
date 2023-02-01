using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Elpy.FunTime
{
    public class BulletMono : MonoBehaviour
    {
        public float FlySpeed;
    }

    public class BulletBaker : Baker<BulletMono>
    {
        public override void Bake(BulletMono authoring)
        {
            AddComponent(new BulletFlyProperties
            {
                FlySpeed = authoring.FlySpeed
            });
            AddComponent<ProjectileHeading>();
        }
    }
}
