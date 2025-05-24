using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine.UI;
using PlotDesigner.Runtime;

namespace PlotDesigner.Editor
{
    public partial class PlotWindow
    {

        public void UpdatePlotTrackList()
        {
            if (currPlot != null)
            {
                var trackDataList = currPlot.trackDataList;
                if (trackDataList != null)
                {
                    for (int i = 0; i < trackDataList.Count; i++)
                    {
                        var trackData = trackDataList[i];

                        if (trackData.trackNodeList != null)
                        {
                            for (int j = 0; j < trackData.trackNodeList.Count; j++)
                            {
                                var trackNodeData = trackData.trackNodeList[j];

                                //动画轨道节点
                                if (trackNodeData is AnimationTrackNode)
                                {
                                    UpdateAnimationTrackNode(trackNodeData);
                                }
                                else if (trackNodeData is TransformTrackNode)
                                {
                                    UpdateTransformTrackNode(trackNodeData);
                                }
                                else if (trackNodeData is WordTrackNode)
                                {
                                    UpdateWordTrackNode(trackNodeData);
                                }

                            }
                        }
                        
                    }
                }
              
            }
        }


        public void UpdateAnimationTrackNode(TrackNode trackNodeData)
        {
            var startTime = trackNodeData.startTime;
            var endTime = trackNodeData.endTime;
            var aniTrackNode = (AnimationTrackNode)trackNodeData;
            var id = trackNodeData.id;

            if (id > 0)
            {
                var plotUnit = this.GetPlotUnitById(id) as GameObject;
                if (null == plotUnit)
                {
                    return;
                }
                var isLoop = aniTrackNode.isLoop;
                var ani = plotUnit.GetComponentInChildren<Animation>();
                var aniName = aniTrackNode.aniName;
                if (currTime >= startTime && currTime <= endTime)
                {
                    //plotUnit.SetActive(true);
                    var aniState = ani[aniName];
                    var lastTime = aniState.clip.length;
                    aniState.weight = 1;
                    aniState.enabled = true;
                    var timeSpan = (currTime - startTime);
                    var time01Value = timeSpan / lastTime;
                    if (isLoop)
                    {
                        var intPart = (int)time01Value;
                        time01Value = time01Value - intPart;
                    }
                    aniState.normalizedTime = time01Value;
                    ani.Sample();
                    aniState.enabled = false;
                }
                else
                {
                    //plotUnit.SetActive(false);
                }
            }

         
        }

        public void UpdateTransformTrackNode(TrackNode trackNodeData)
        {
            var startTime = trackNodeData.startTime;
            var endTime = trackNodeData.endTime;
            var trackNode = (TransformTrackNode)trackNodeData;
            var id = trackNodeData.id;

            GameObject plotUnit = null;
            if (trackNodeData is CameraTrackNode)
            {
                plotUnit = GameObject.Find("Camera_3D").gameObject;
            }
            else
            {
                plotUnit = this.GetPlotUnitById(id) as GameObject;
            }

            if (currTime >= startTime && currTime <= endTime)
            {
                var timeSpan = (currTime - startTime);
                var time01Value = timeSpan / (endTime - startTime);

                if (plotUnit != null)
                {
                    //先用直线
                    //pos
                    var currPos = MathTool.GetVector3Lerp(trackNode.startPos, trackNode.endPos, time01Value);
                    plotUnit.transform.position = currPos;
                    //toward
                    var currForward = MathTool.GetVector3Lerp(trackNode.startForward, trackNode.endForward, time01Value);
                    plotUnit.transform.forward = currForward;
                }

               
            }
        }

        public void UpdateWordTrackNode(TrackNode trackNodeData)
        {
            var startTime = trackNodeData.startTime;
            var endTime = trackNodeData.endTime;
            var trackNode = (WordTrackNode)trackNodeData;
            var id = trackNodeData.id;
            var timeSpan = endTime - startTime;
            var wordText = plotUIRoot.Find("Text").GetComponent<Text>();

            if (currTime >= startTime && currTime <= endTime)
            {
                string wordStr = trackNode.word;

                if (trackNode.showType == WordShowType.TypeWriter)
                {
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
                else
                {
                    wordText.text = wordStr;
                }


                wordText.gameObject.SetActive(true);

            }
            else
            {
                wordText.gameObject.SetActive(false);
            }

        }
    }
}