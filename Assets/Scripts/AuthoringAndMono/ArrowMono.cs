using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Elpy.FunTime
{
    public class ArrowMono : MonoBehaviour
    {
        public float FlySpeed;
    }

    public class ArrowBaker : Baker<ArrowMono>
    {
        public override void Bake(ArrowMono authoring)
        {
            AddComponent(new ArrowFlyProperties
            {
                FlySpeed = authoring.FlySpeed
            });
            AddComponent<ProjectileHeading>();
        }
    }
}
