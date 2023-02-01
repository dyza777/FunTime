using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Elpy.FunTime
{
    public class BulletCoolingSliderMono : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        public float InitialValue;
        public float Value;

        private void Start()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
            {
                return;
            }
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = em.CreateEntity(typeof(BulletCoolingSliderData), typeof(Slider));
            em.AddComponentObject(entity, _slider);

            _slider.value = InitialValue;
        }
    }
}
