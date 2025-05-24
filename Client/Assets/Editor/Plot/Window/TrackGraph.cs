using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using PlotDesigner.Runtime;

namespace PlotDesigner.Editor
{


    public class TrackGraph
    {
        //绝对 rect
        public Rect titleRect;
        public Rect playNodeRect;

        public Track trackData;

        public List<TrackNodeGraph> trackNodeGraphList;

        public void Draw(Vector2 parentPos)
        {
            var titleStr = this.trackData.GetType().Name;
            GUI.Box(titleRect, titleStr);

            var style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;
            GUI.Box(playNodeRect, "", style);


            //draw node
            foreach (var gra in trackNodeGraphList)
            {
                gra.Draw(new Vector2(playNodeRect.x, playNodeRect.y));
            }
        }

        //标题区域是否包含点
        public bool IsTitleAreaCantainsPoint(Vector2 point)
        {
            return this.titleRect.Contains(point);
        }

        //播放区域是否包含点
        public bool IsPlayAreaCantainsPoint(Vector2 point)
        {
            return this.playNodeRect.Contains(point);
        }


        //点是否在轨道节点中
        public bool IsNodeCantainsPoint(Vector2 point, out TrackNodeGraph nodeGraph)
        {
            foreach (var trackGraph in trackNodeGraphList)
            {
                if (trackGraph.IsNodeCantainsPoint(point))
                {
                    nodeGraph = trackGraph;
                    return true;
                }
            }
            nodeGraph = null;
            return false;
        }

        //点是否在轨道节点的两边上
        public bool IsNodeBorderCantainsPoint(Vector2 point, out TrackNodeGraph nodeGraph, out bool isLeft)
        {
            foreach (var trackGraph in trackNodeGraphList)
            {
                if (trackGraph.IsNodeBorderCantainsPoint(point, out isLeft))
                {
                    nodeGraph = trackGraph;
                    return true;
                }
            }
            isLeft = false;
            nodeGraph = null;
            return false;
        }

        internal void SetData(Track trackData)
        {
            this.trackData = trackData;

            trackNodeGraphList = new List<TrackNodeGraph>();
            if (this.trackData.trackNodeList != null)
            {
                foreach (var node in this.trackData.trackNodeList)
                {
                    TrackNodeGraph nodeGraph = new TrackNodeGraph();
                    nodeGraph.SetData(node);
                    nodeGraph.trackGraph = this;
                    trackNodeGraphList.Add(nodeGraph);
                }
            }
          

        }

        //public void DeleteTrack()
        //{

        //    trackGraphList.Remove();
        //    PlotWindow.instance.DeleteTrack(this.trackData);
        //}

    }

}