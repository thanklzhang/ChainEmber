using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotWordTrackNodePlayer : PlotTrackNodePlayer
    {
        WordTrackNode wordTrackNode;
        Text wordText;
        public override void OnInit()
        {
            wordTrackNode = (WordTrackNode)trackNode;
        }

        public override void OnStart()
        {
            var uiRoot = this.GetPlotUIRoot();
            wordText = uiRoot.Find("Text").GetComponent<Text>();
            wordText.gameObject.SetActive(true);
        }

        public override void OnUpdate(float timeDelta)
        {
            var currTime = this.GetCurrTime();
            var startTime = this.trackNode.startTime;
            var endTime = this.trackNode.endTime;
            var timeSpan = endTime - startTime;
            var type = this.wordTrackNode.showType;
            if (type == WordShowType.Null)
            {

            }
            else if (type == WordShowType.TypeWriter)
            {
                var wordStr = this.wordTrackNode.word;
                var len = wordStr.Length;

                var time01Value = (currTime - startTime) / timeSpan;
                int resultLen = (int)(len * time01Value);
                if (resultLen <= wordStr.Length)
                {
                    wordText.text = wordStr.Substring(0, resultLen);
                }
                else
                {
                    wordText.text = wordStr;
                }
            }
        }

        public override void OnEnd()
        {
            wordText.gameObject.SetActive(false);
        }
    }
}

