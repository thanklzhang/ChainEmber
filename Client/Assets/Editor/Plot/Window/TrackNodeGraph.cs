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


    public class TrackNodeGraph
    {
        public TrackNode nodeData;
        public TrackGraph trackGraph;

        Rect mainRect;
        Rect leftRect;
        Rect rightRect;

        Vector2 parentPos;//父节点的绝对坐标

        public void SetData(TrackNode node)
        {
            this.nodeData = node;
        }

        ////根据当前对应时间 获取 本地相对坐标 
        //public float GetGraphXByTime(float time)
        //{
        //    float timeSpanCount = time / (1.0f / PlotGraphDefine.timeSpanCountPerSeconds);
        //    var x = timeSpanCount * PlotGraphDefine.lenPerTimeSpan;
        //    return x;
        //}

        ////根据 本地相对坐标 获取 当前对应时间
        //public float GetTimeByGraphX(float x)
        //{
        //    //多少个刻度单位
        //    float timeSpanCount = x / PlotGraphDefine.lenPerTimeSpan;
        //    //时间
        //    float time = timeSpanCount * (1.0f / PlotGraphDefine.timeSpanCountPerSeconds);
        //    return time;
        //}

        public TrackNodeSO so = ScriptableObject.CreateInstance<TrackNodeSO>();
        public bool isSelect = false;
        public void Draw(Vector2 parentPos)
        {
            this.parentPos = parentPos;
            var startTime = nodeData.startTime;
            var endTime = nodeData.endTime;

            var startX = PlotGraphTool.GetGraphXByTime(startTime);
            var endX = PlotGraphTool.GetGraphXByTime(endTime);
            var len = endX - startX;


            var trackHeight = PlotGraphDefine.trackHeight - 10;
            mainRect = new Rect(parentPos.x + startX, parentPos.y + 5, len, trackHeight);

            var style = new GUIStyle();

            //选中标识
            //var isSelect = this == PlotWindow.instance.currSelectTrackNodeGraph;
            //if (!isSelect)
            //{
            //    style.normal.background = Texture2D.grayTexture;
            //}
            //else
            //{
            //    style.normal.background = Texture2D.whiteTexture;
            //}

            var isSelect = so != null && so == Selection.activeObject;//PlotWindow.instance.currSelectSO;
            if (!isSelect)
            {
                style.normal.background = Texture2D.grayTexture;
            }
            else
            {
                style.normal.background = Texture2D.whiteTexture;
            }


            //绘制整体显示部分
            GUI.Box(mainRect, "", style);


            //节点类型名称显示
            GUIStyle typeNameStyle = new GUIStyle();
            typeNameStyle.normal.textColor = Color.black;
            var typeName = this.nodeData.GetType().Name.Replace("TrackNode","");
            var typeNameRect = mainRect;
            typeNameRect.x += 5.0f;
            GUI.Label(typeNameRect, typeName, typeNameStyle);



            //左右边界
            leftRect = new Rect(parentPos.x + startX, parentPos.y + 5, 5, trackHeight);
            rightRect = new Rect(parentPos.x + startX + len - 5, parentPos.y + 5, 5, trackHeight);
            var pre = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            var borderStyle = new GUIStyle();
            EditorGUIUtility.AddCursorRect(leftRect, MouseCursor.ResizeHorizontal);
            EditorGUIUtility.AddCursorRect(rightRect, MouseCursor.ResizeHorizontal);
            GUI.Box(leftRect, "");
            GUI.Box(rightRect, "");

            GUI.backgroundColor = pre;
        }

        public bool IsNodeCantainsPoint(Vector2 point)
        {
            return mainRect.Contains(point);
        }

        public bool IsNodeBorderCantainsPoint(Vector2 point, out bool isLeft)
        {
            if (leftRect.Contains(point))
            {
                isLeft = true;
                return true;
            }
            if (rightRect.Contains(point))
            {
                isLeft = false;
                return true;
            }
            isLeft = false;
            return false;
        }

        public void SetStartTime(float startTime)
        {
            this.nodeData.startTime = startTime;

            if (this.nodeData.startTime < 0)
            {
                this.nodeData.startTime = 0;
            }

            if (this.nodeData.startTime > this.nodeData.endTime)
            {
                this.nodeData.startTime = this.nodeData.endTime;
            }

            PlotWindow.instance.RefreshInspector();
        }

        public void SetEndTime(float endTime)
        {
            this.nodeData.endTime = endTime;

            if (this.nodeData.endTime < 0)
            {
                this.nodeData.endTime = 0;
            }

            if (this.nodeData.endTime < this.nodeData.startTime)
            {
                this.nodeData.endTime = this.nodeData.startTime;
            }
            PlotWindow.instance.RefreshInspector();
        }

        float preNodeStartTime;
        float preNodeEndTime;
        public void OnStartDrag()
        {
            preNodeStartTime = this.nodeData.startTime;
            preNodeEndTime = this.nodeData.endTime;
        }

        internal void Move(Vector2 offset)
        {
            var timeOffset = PlotGraphTool.GetTimeByGraphX(offset.x);
            //Logx.Log("timeOffset 1 : " + offset.x + "  " + timeOffset);
            //Logx.Log("timeOffset 2 : " + (this.nodeData.startTime + timeOffset));
            //Logx.Log("timeOffset 3 : " + (this.nodeData.endTime + timeOffset));
            SetStartTime(preNodeStartTime + timeOffset);
            SetEndTime(preNodeEndTime + timeOffset);

        }

        public void OnClick()
        {
            //if (null == so)
            //{
            //    so = ScriptableObject.CreateInstance<TrackNodeSO>();
            //}

            so.trackNode = this.nodeData;
            
            PlotWindow.instance.SetCurrSelectSO(so, this);


        }

        internal void SetTimeRangeByMousePosX(bool isLeft, float mousePosX)
        {
            var offsetX = mousePosX - this.parentPos.x;
            if (offsetX < 0)
            {
                offsetX = 0;
            }
            var time = PlotGraphTool.GetTimeByGraphX(offsetX);
            if (isLeft)
            {
                this.SetStartTime(time);
            }
            else
            {
                this.SetEndTime(time);
            }

        }

        public void Select()
        {
            //so.isSelect = true;
        }

        internal void CancelSelect()
        {
            //so.isSelect = false;
        }
    }
}