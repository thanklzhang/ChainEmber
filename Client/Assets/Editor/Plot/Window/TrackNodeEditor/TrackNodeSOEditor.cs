using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using PlotDesigner.Runtime;

namespace PlotDesigner.Editor
{
    [CustomEditor(typeof(TrackNodeSO), true)]
    public class TrackNodeSOEditor : UnityEditor.Editor
    {
        SerializedObject obj;

        public Dictionary<Type, Action> typeActionDic = new Dictionary<Type, Action>();

        public void OnEnable()
        {
            //sub
            typeActionDic.Add(typeof(AnimationTrackNode), AnimationTrackNodeEditor);
            typeActionDic.Add(typeof(TransformTrackNode), TransformTrackNodeEditor);
            typeActionDic.Add(typeof(WordTrackNode), WorldTrackNodeEditor);

            //base
            typeActionDic.Add(typeof(CameraTrackNode), CameraTrackNodeEditor);
        }

        public override void OnInspectorGUI()
        {
            //obj = new SerializedObject(target);
            obj = serializedObject;
            //trackNode = obj.FindProperty("trackNode");
            var so = target as TrackNodeSO;

            //base
            DrawPropertiesExcluding(obj);

            EditorGUILayout.LabelField("-----------------");

            //sub
            var currType = so.trackNode.GetType();
            foreach (var item in typeActionDic)
            {
                var key = item.Key;
                var action = item.Value;

                //Logx.Log("type : " + currType.Name + " , " + key.Name);
                if (key.IsAssignableFrom(currType) || currType == key)
                {
                    action?.Invoke();
                }

            }


            if (GUI.changed)
            {
                Undo.RecordObject(so,"sdfsdf");
            }

            //注意顺序
            obj.ApplyModifiedProperties();
            obj.Update();
        }

        public void AnimationTrackNodeEditor()
        {
            var so = target as TrackNodeSO;
            var trackNode = (AnimationTrackNode)so.trackNode;
            EditorGUILayout.BeginVertical();

            AddNodeTypeLable("动画");

            //EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("aniName");
            
            trackNode.aniName = EditorGUILayout.TextField(trackNode.aniName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("isLoop");
            
            trackNode.isLoop = EditorGUILayout.Toggle(trackNode.isLoop);
            EditorGUILayout.EndHorizontal();

            //if (EditorGUI.EndChangeCheck())
            //{
                
            //}

            EditorGUILayout.EndVertical();
        }

        public void TransformTrackNodeEditor()
        {
            var so = target as TrackNodeSO;
            var trackNode = (TransformTrackNode)so.trackNode;
            EditorGUILayout.BeginVertical();

            AddNodeTypeLable("变换");

            EditorGUILayout.BeginHorizontal();
            trackNode.startPos = EditorGUILayout.Vector3Field("startPos", trackNode.startPos);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            trackNode.endPos = EditorGUILayout.Vector3Field("endPos", trackNode.endPos);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            trackNode.startForward = EditorGUILayout.Vector3Field("startForward", trackNode.startForward);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            trackNode.endForward = EditorGUILayout.Vector3Field("endForward", trackNode.endForward);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        public void WorldTrackNodeEditor()
        {
            var so = target as TrackNodeSO;
            var trackNode = (WordTrackNode)so.trackNode;

            EditorGUILayout.BeginVertical();

            AddNodeTypeLable("字幕");

            EditorGUILayout.BeginHorizontal();
            trackNode.word = EditorGUILayout.TextField("word", trackNode.word);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            trackNode.showType = (WordShowType)EditorGUILayout.EnumPopup("showType", trackNode.showType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

        }

        public void CameraTrackNodeEditor()
        {
            var so = target as TrackNodeSO;
            var trackNode = (CameraTrackNode)so.trackNode;

            EditorGUILayout.BeginVertical();

            AddNodeTypeLable("相机");

            EditorGUILayout.BeginHorizontal();
            trackNode.testId = EditorGUILayout.IntField("testId", trackNode.testId);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

        }

        public void AddNodeTypeLable(string typeName)
        {
            EditorGUILayout.BeginHorizontal();

            //var preColor = GUI.color;
            //GUI.color = Color.red;
            var style = new GUIStyle() { fontSize = 15 };
            style.normal.textColor = Color.black;
            EditorGUILayout.LabelField("节点类型", style);
            EditorGUILayout.LabelField(typeName, style);

            //GUI.color = preColor;
            EditorGUILayout.EndHorizontal();
        }
    }
}