using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Elpy.FunTime
{
    public class ZombieMono : MonoBehaviour
    {
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;

        public float EatDamage;
        public float EatAmplitude;
        public float EatFrequency;

        public float PushBackFullDistance;
        public float PushBackSpeed;
        public float PushBackHeight;

        public float KillFallSpeed;
    }

    public class EntityBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            AddComponent(new ZombieWalkProperties
            {
                WalkSpeed = authoring.WalkSpeed,
                WalkAmplitude = authoring.WalkAmplitude,
                WalkFrequency = authoring.WalkFrequency
            });
            AddComponent(new ZombieEatProperties
            {
                EatDamagePerSecond = authoring.EatDamage,
                EatAmplitude = authoring.EatAmplitude,
                EatFrequency = authoring.EatFrequency
            });
            AddComponent(new ZombiePushBackProperties
            {
                PushBackFullDistance = authoring.PushBackFullDistance,
                PushBackSpeed = authoring.PushBackSpeed,
                PushBackHeight = authoring.PushBackHeight
            });
            AddComponent(new ZombieKillProperties
            {
                KillFallSpeed = authoring.KillFallSpeed
            });
            AddComponent(new ZombiePushBackCurrentData
            {
                PushBackCurrentDistance = 0
            });
            AddComponent(new ZombieKillCurrentData
            {
                currentFallAngle = 0
            });
            AddComponent<ZombieHeading>();
            AddComponent<NewZombieTag>();
            AddComponent<ZombieTimer>();
        }
    }
}
