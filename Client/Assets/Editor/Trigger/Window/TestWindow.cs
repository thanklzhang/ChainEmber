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
    public partial class TestWindow : EditorWindow
    {
        public static TestWindow instance;


        public static TestWindow window;
        [MenuItem("Tools/zzzz TestWindow Editor", false, 100)]
        public static void OpenTriggerWindow()
        {
            window = EditorWindow.GetWindow<TestWindow>(false, "TestWindow Editor");
            TestWindow.instance = window;

            window.minSize = new Vector2(1300f, 100f);
            window.wantsMouseMove = true;
            window.Init();
        }

        public void Init()
        {

        }

        public void OnGUI()
        {
            DrawAll();
        }

        Vector2 fileCatalogueScrollPos;
        Rect pos = new Rect(0, 0, 300, 600);
        Rect rect = new Rect(0, 0, 500, 800);
        public void DrawAll()
        {

            GUILayout.BeginArea(new Rect(100, 100, 2500, 5000), EditorStyles.helpBox);
            GUILayout.Label("战斗触发器文件");
            //fileCatalogueScrollPos = GUI.BeginScrollView(fileCatalogueScrollPos, false, true, new GUILayoutOption[] { });

            rect = new Rect(0, 0, 500, 22 * 20);
            fileCatalogueScrollPos = GUI.BeginScrollView(pos, fileCatalogueScrollPos, rect);


            ////TODO 树形结构
            //for (int i = 0; i < fileList.Count; i++)
            //{
            //    var file = fileList[i];
            //    var isSelect = selectTriggerFileIndex == i;
            //    var preColor = GUI.backgroundColor;
            //    var nColor = Color.white;
            //    GUI.backgroundColor = isSelect ? nColor : new Color(0.6f, 0.6f, 0.6f, 1.0f);

            //    GUILayout.Space(5.0f);
            //    if (GUILayout.Button(file, new GUILayoutOption[] { GUILayout.Height(30.0f) }))
            //    {
            //        OnClickFile(i);
            //    }
            //    GUI.backgroundColor = preColor;
            //}
            ////EditorGUILayout.Foldout(true,new GUIContent() {  text = "123"});

            for (int i = 0; i < 22; i++)
            {
                GUI.Button(new Rect(10, 10 + i * 20, 100, 20), "aa" + i);
            }

            GUI.EndScrollView();
            GUILayout.EndArea();

            


            //GUI.Button(new Rect(10, 10, 100, 20), "aa");
            //GUI.Button(new Rect(10, 30, 100, 30), "bb");
            //GUI.Button(new Rect(10, 100, 100, 30), "cc");

            //Rect r = new Rect(100,230,500,1000);
            //GUILayout.BeginArea(r);
            //GUILayout.BeginVertical();


            //GUI.Button(new Rect(10,10,100,20),"aa");
            //GUI.Button(new Rect(10, 30, 100, 30), "bb");
            //GUI.Button(new Rect(10, 100, 100, 30), "cc");

            //GUILayout.Button("aa");
            //GUILayout.Button("bb");
            //GUILayout.Button("cc");
            //GUILayout.Button("dd");


            //GUILayout.EndVertical();
            //GUILayout.EndArea();

        }

    }
}