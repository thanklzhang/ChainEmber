using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotAnimationTrackNodePlayer : PlotTrackNodePlayer
    {
        AnimationTrackNode aniTrackNode;

        public override void OnInit()
        {
            aniTrackNode = (AnimationTrackNode)trackNode;
        }

        public override void OnStart()
        {
            int id = this.aniTrackNode.id;
            var plotEntity = this.GetPlotEntityById(id);
            var gameObject = plotEntity.obj as GameObject;

            var ani = gameObject.GetComponentInChildren<Animation>();
            var aniName = this.aniTrackNode.aniName;
            ani.Play(aniName);
        }

        public override void OnUpdate(float timeDelta)
        {

        }

        public override void OnEnd()
        {

        }
    }
}

