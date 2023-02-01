using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Elpy.FunTime
{
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            Cursor.visible = false;
        }

        protected override void OnUpdate()
        {
            var cameraSingleton = CameraSingleton.Instance;
            if (cameraSingleton == null) return;

            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");

            cameraSingleton.transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }
    }
}