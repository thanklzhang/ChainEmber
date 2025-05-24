using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotCameraTrackNodePlayer : PlotTransformTrackNodePlayer
    {
        CameraTrackNode cameraTrackNode;

        public override void OnInit()
        {
            base.OnInit();
            cameraTrackNode = (CameraTrackNode)trackNode;
        }

        public override void OnStart()
        {
            var cameraObj = CameraManager.Instance.GetCamera3D();
            tran = cameraObj.camera.transform;
        }

        public override void OnUpdate(float timeDelta)
        {
            base.OnUpdate(timeDelta);
        }

        public override void OnEnd()
        {

        }
    }
}

