using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Reflection;
using PlotDesigner.Runtime;

namespace BattleTrigger.Editor
{
    [Serializable]
    public class TriggerNodeGraph
    {
        //public Rect parentRect;
        public int floor;
        public TriggerGroupGraph groupGraph;
        protected GUIStyle bgBtnStyle;
        protected float headOffsetX = 0.0f;
        public virtual void Draw()//Rect childRect
        {
            var isSelect = TriggerWindow.instance.currSelectTriggerNodeGraph == this;

            bgBtnStyle = new GUIStyle();
            if (isSelect)
            {
                bgBtnStyle.normal.background = Texture2D.whiteTexture;
            }
            else
            {
                bgBtnStyle.normal.background = Texture2D.grayTexture;
            }

            bgBtnStyle.alignment = TextAnchor.MiddleLeft;

            GUILayout.BeginHorizontal();

            var headX = this.floor * 35 + headOffsetX;
            GUILayout.Space(headX);

            DrawLeft();

            var str = "    " + GetDrawContentStr();
            var width = 50 + str.Length * 8;
            if (GUILayout.Button(str, bgBtnStyle, new GUILayoutOption[] { GUILayout.Height(20), GUILayout.Width(width) }))
            {
                //Logx.Log("TriggerWindow.instance.isRightClick : " + TriggerWindow.instance.isRightClick);
                if (TriggerWindow.instance.isRightClick)
                {
                    //右键点击
                    this.OnRightClick();
                }
                else
                {
                    // 左键点击（也包括中键）
                    this.OnLeftClick();
                }


                GUIUtility.keyboardControl = 0;
            }

            GUILayout.EndHorizontal();
        }

        protected string showStr;
        public void SetShowStr(string str)
        {
            this.showStr = str;
        }

        public virtual void DrawLeft()
        {
            
        }

        public virtual string GetDrawContentStr()//Rect childRect
        {
            return "";
        }

        public void Create(int floor)
        {
            this.floor = floor;
            this.OnCreate();
        }
        public virtual void OnCreate()
        {

        }

        public void Parse(JsonData nodeJsonData, int floor)
        {
            this.floor = floor;
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(JsonData nodeJsonData)
        {

        }

        public TriggerNodeGraph Clone()
        {
            TriggerNodeGraph node = OnClone();

            node.floor = this.floor;
            node.groupGraph = this.groupGraph;
            node.bgBtnStyle = this.bgBtnStyle;
            node.headOffsetX = this.headOffsetX;
            node.showStr = this.showStr;

            return node;
        }

        public virtual TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new TriggerNodeGraph();
            return node;
        }

        public virtual void OnLeftClick()
        {
            this.OnSelect();
        }

        public virtual void OnRightClick()
        {
            TriggerWindow.instance.OnRightClickNode(this);
        }

        public virtual void OnSelect()
        {
            TriggerWindow.instance.SetSelectTriggerNodeGraph(this);
        }

        //当选择该节点的时候 绘画本节点的变量信息
        public virtual void DrawSelectInfo()
        {

        }

        public virtual void SetFloorIncludeChildren(int floor)
        {
            this.floor = floor;
        }

        public JsonData ToJson()
        {
            JsonData jd = new JsonData();
            jd["__ASSEMBLY__"] = "BattleServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];
            var list = typeName.Split(new string[] { "Graph" }, StringSplitOptions.RemoveEmptyEntries);
            var resultTypeName = list[0];
            jd["__TYPE__"] = "Battle.BattleTrigger.Config." + resultTypeName;

            return this.OnToJson(jd);
        }

        public virtual JsonData OnToJson(JsonData jd)
        {
            return jd;
        }
    }
}