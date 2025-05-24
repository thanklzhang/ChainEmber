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
    public class TriggerGroupGraph
    {
        List<TriggerNodeGraph> triggerNodeGraphList;
        public int floor;
        public void Init(List<TriggerNodeGraph> triggerNodeGraphList, int floor)
        {
            this.triggerNodeGraphList = triggerNodeGraphList;
            this.floor = floor;
            foreach (var item in this.triggerNodeGraphList)
            {
                item.groupGraph = this;
            }
        }

        public void AddNewTriggerNodeGraph(TriggerNodeGraph newNodeGraph, int insertIndex)
        {
            this.triggerNodeGraphList.Insert(insertIndex, newNodeGraph);
            newNodeGraph.groupGraph = this;
        }

        public void AddNewTriggerNodeGraphToEnd(TriggerNodeGraph newNodeGraph)
        {
            int index = this.triggerNodeGraphList.Count;
            AddNewTriggerNodeGraph(newNodeGraph, index);
        }

        public void Draw(Rect rect)
        {
            GUILayout.BeginVertical();

            for (int i = 0; i < triggerNodeGraphList.Count; i++)
            {
                var nodeGraph = triggerNodeGraphList[i];

                //var x = rect.x + 10;
                //var y = rect.y + 10;
                //var yHeight = 40;
                //var childRect = new Rect(x, y + yHeight * i, 250,35);
                //Vector2 _offset = new Vector2(0, yHeight * i);
                //nodeGraph.Draw(childRect);
                GUILayout.Space(3);
                nodeGraph.Draw();

            }

            GUILayout.EndVertical();

            //GUILayout.EndArea();

        }

        public JsonData ToJson()
        {
            JsonData arr = new JsonData();
            arr.SetJsonType(JsonType.Array);
            foreach (var nodeGraph in triggerNodeGraphList)
            {
                var jsonD = nodeGraph.ToJson();
                arr.Add(jsonD);
            }

            return arr;
        }

        internal int GetNodeIndex(TriggerNodeGraph targetNodeGraph)
        {
            return this.triggerNodeGraphList.IndexOf(targetNodeGraph);
        }

        internal void DeleteTriggerNodeGraph(TriggerNodeGraph targetNodeGraph)
        {
            this.triggerNodeGraphList.Remove(targetNodeGraph);
        }

        internal TriggerGroupGraph Clone()
        {
            TriggerGroupGraph group = new TriggerGroupGraph();
            group.triggerNodeGraphList = new List<TriggerNodeGraph>();
            foreach (var item in this.triggerNodeGraphList)
            {
                var newObj = item.Clone();
                //group.triggerNodeGraphList.Add(newObj);
                group.AddNewTriggerNodeGraphToEnd(newObj);
            }
            group.floor = this.floor;
            return group;
        }

        internal void SetFloor(int floor)
        {
            this.floor = floor;
            foreach (var item in triggerNodeGraphList)
            {
                item.SetFloorIncludeChildren(floor);
            }
        }

        internal void Select(int index)
        {
            if (index < triggerNodeGraphList.Count)
            {
                var first = triggerNodeGraphList[index];

                first.OnSelect();
            }
            GUIUtility.keyboardControl = 0;

        }
    }
}