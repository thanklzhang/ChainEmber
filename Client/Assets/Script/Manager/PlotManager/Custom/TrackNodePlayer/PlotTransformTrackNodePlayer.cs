using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotTransformTrackNodePlayer : PlotTrackNodePlayer
    {
        TransformTrackNode tranTrackNode;
        protected Transform tran;
        public override void OnInit()
        {
            tranTrackNode = (TransformTrackNode)trackNode;
        }

        public override void OnStart()
        {
            int id = this.tranTrackNode.id;
            var plotEntity = this.GetPlotEntityById(id);
            tran = (plotEntity.obj as GameObject).transform;
        }

        public override void OnUpdate(float timeDelta)
        {
            //先用直线
            UpdatePos(timeDelta);
            UpdateForward(timeDelta);

        }

        public void UpdatePos(float timeDelta)
        {
            var startTime = this.tranTrackNode.startTime;
            var endTime = this.tranTrackNode.endTime;
            var totalTime = endTime - startTime;

            var startPos = this.tranTrackNode.startPos;
            var endPos = this.tranTrackNode.endPos;

            var delta = (GetCurrTime() - startTime) / totalTime;

            var len = (endPos - startPos).magnitude;
            var dir = (endPos - startPos).normalized;
            var currPos = startPos + dir * len * delta;

            this.tran.position = currPos;
        }

        public void UpdateForward(float timeDelta)
        {
            var startTime = this.tranTrackNode.startTime;
            var endTime = this.tranTrackNode.endTime;
            var totalTime = endTime - startTime;

            var startForward = this.tranTrackNode.startForward;
            var endForward = this.tranTrackNode.endForward;

            var delta = (GetCurrTime() - startTime) / totalTime;

            var len = (endForward - startForward).magnitude;
            var dir = (endForward - startForward).normalized;
            var currForward = startForward + dir * len * delta;
            this.tran.forward = currForward;
        }

        public override void OnEnd()
        {

        }
    }
}

