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

    using UnityEditor;
    using UnityEngine;

    public partial class TriggerWindow : EditorWindow
    {
        public static TriggerWindow instance;

        public string triggerConfigFolderPath;

        public List<string> fileList = new List<string>();

        List<Type> triggerEventTypeList;
        public List<Type> triggerNodeTypeList;

        //
        public Vector2 mousePosition;

        //List<TriggerNodeGraph> triggerNodeGraphList = new List<TriggerNodeGraph>();

        TriggerGroupGraph rootGroupGraph;


        public static TriggerWindow window;
        [MenuItem("Tools/Trigger Editor", false, 100)]
        public static void OpenTriggerWindow()
        {
            window = EditorWindow.GetWindow<TriggerWindow>(false, "Trigger Editor");
            TriggerWindow.instance = window;

            window.minSize = new Vector2(1300f, 500f);
            window.wantsMouseMove = true;
            window.Init();
        }

        public void Init()
        {
            triggerConfigFolderPath = Application.dataPath + "/BuildRes/BattleTrigger";

            ReadFiles();

             //collect track and track node
             triggerNodeTypeList = new List<Type>();
            triggerEventTypeList = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    var currType = types[j];

                    //读取所有 TriggerEvent,TriggerNode 类型
                    if (!currType.IsAbstract)
                    {
                        if (typeof(TriggerEventGraph).IsAssignableFrom(currType))
                        {
                            triggerEventTypeList.Add(currType);
                        }
                        else if (typeof(TriggerNodeGraph).IsAssignableFrom(currType))
                        {
                            triggerNodeTypeList.Add(currType);
                        }
                    }
                }

            }
        }

        public void ReadFiles()
        {
            //读取所有触发器文件
            var path = triggerConfigFolderPath;
            var filePaths = FileOperate.GetAllFilesFromFolder(path, "*.json", SearchOption.AllDirectories);
            fileList = new List<string>();
            for (int i = 0; i < filePaths.Length; i++)
            {
                var filePath = filePaths[i];
                var pathName = filePath.Substring(triggerConfigFolderPath.Length + 1);
                fileList.Add(pathName);//Path.GetFileName(filePath);
            }
        }

        public void OnGUI()
        {
            DrawAll();
        }

        public void DrawAll()
        {
            HandleEvent();
            DrawTriggerFileListView();
            DrawSingleTriggerView();

            isRightClick = false;

        }

        public bool isRightClick;
        //处理输入事件
        public void HandleEvent()
        {
            if (null == Event.current)
            {
                return;
            }

            mousePosition = Event.current.mousePosition;

            if (EventType.MouseDown == Event.current.type)
            {
                if (0 == Event.current.button)
                {
                    //左键点击
                    HandleLeftClick();
                }
                else if (1 == Event.current.button)
                {
                    //右键点击
                    HandleRightClick();
                }

            }
            else if (EventType.MouseUp == Event.current.type)
            {
                if (0 == Event.current.button)
                {
                    //左键抬起  
                }
                else if (1 == Event.current.button)
                {
                    //右键抬起  
                    isRightClick = true;
                }
            }
            else if (EventType.ValidateCommand == Event.current.type)
            {
                if (Event.current.commandName.Equals("Copy"))
                {
                    Event.current.Use();
                }
                else if (Event.current.commandName.Equals("Paste"))
                {
                    Event.current.Use();
                }
                else if (Event.current.commandName.Equals("SoftDelete"))
                {
                    Event.current.Use();
                }
            }
            else if (EventType.ExecuteCommand == Event.current.type)
            {
                if (Event.current.commandName.Equals("Copy"))
                {
                    this.OnCommand_Ctrl_C();
                    Event.current.Use();
                }
                else if (Event.current.commandName.Equals("Paste"))
                {
                    this.OnCommand_Ctrl_V();
                    Event.current.Use();
                }
                else if (Event.current.commandName.Equals("SoftDelete"))
                {
                    this.OnCommand_Del();
                    Event.current.Use();
                }
            }
        }


        public void HandleLeftClick()
        {

        }

        public void HandleRightClick()
        {

        }

        public Vector2 fileCatalogueScrollPos;
        public int selectTriggerFileIndex = -1;
        //文件列表部分
        public void DrawTriggerFileListView()
        {
            GUILayout.BeginArea(new Rect(0, 0, 250, 500), EditorStyles.helpBox);
            GUILayout.Label("战斗触发器文件");
            if (GUILayout.Button("新建触发器"))
            {
                var scrPath = GlobalConfig.buildPath + "/" + "FileTemplate/BattleTriggerTemp/battle_trigger_temp.json";
                //var desName = Const.buildPath + "/" + "BattleTriggerConfig/new_trigger";
                //var desName = Application.dataPath.Replace("/001_GameFramework_Client/Assets","");
                var desName = GlobalConfig.buildPath + @"\BattleTrigger\new_trigger";
                Debug.Log("desName : " + desName);
                //var desPath = Const.buildPath + "/" + "BattleTriggerConfig/new_trigger.json";
                var currDesName = desName;
                for (int i = 0; i < 10000; i++)
                {
                    if (File.Exists(currDesName + ".json"))
                    {
                        currDesName = desName + i;
                    }
                    else
                    {
                        break;
                    }
                }
                var resultDesPath = currDesName + ".json";
                FileTool.CopyFile(scrPath, resultDesPath);
                ReadFiles();
                //AssetDatabase.Refresh();
            }
            fileCatalogueScrollPos = GUILayout.BeginScrollView(fileCatalogueScrollPos, false, true, new GUILayoutOption[] { });

            //TODO 树形结构
            for (int i = 0; i < fileList.Count; i++)
            {
                var file = fileList[i];
                var isSelect = selectTriggerFileIndex == i;
                var preColor = GUI.backgroundColor;
                var nColor = Color.white;
                GUI.backgroundColor = isSelect ? nColor : new Color(0.6f, 0.6f, 0.6f, 1.0f);

                GUILayout.Space(5.0f);
                if (GUILayout.Button(file, new GUILayoutOption[] { GUILayout.Height(30.0f) }))
                {
                    OnClickFile(i);
                }
                GUI.backgroundColor = preColor;
            }
            //EditorGUILayout.Foldout(true,new GUIContent() {  text = "123"});
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public TriggerNodeGraph rootEventGraph;

        public void OnClickFile(int index)
        {
            if (selectTriggerFileIndex == index)
            {
                return;
            }

            selectTriggerFileIndex = index;

            //加载所需资源
            var file = fileList[selectTriggerFileIndex];
            //加载触发文件资源
            //load file
            var path = Path.Combine(this.triggerConfigFolderPath, file);
            var json = FileOperate.GetTextFromFile(path);

            var triggerRootNode = LitJson.JsonMapper.ToObject(json);
            var eventJsonData = triggerRootNode["triggerEvent"];
            var actionJsonData = triggerRootNode["executeGroup"];

            //event
            rootEventGraph = ParseTriggerActionNode(eventJsonData, 0);
            //action
            rootGroupGraph = new TriggerGroupGraph();
            var nodeGraphList = ParseTriggerNodeList(actionJsonData, 0);
            rootGroupGraph.Init(nodeGraphList, 0);

            rootGroupGraph.Select(0);
            //string json = JsonTool.LoadObjectFromFile(path);

            //currPlot = JsonTool.LoadObjectFromFile<Plot>(path);

            //currPlot.Load(json);
            //LoadAllRes();

            //SetupAllTriggerGraph();

        }

        public List<TriggerNodeGraph> ParseTriggerNodeList(JsonData nodeJsonData, int floor)
        {
            List<TriggerNodeGraph> nodeList = new List<TriggerNodeGraph>();
            var isArray = nodeJsonData.IsArray;
            if (!isArray)
            {
                return nodeList;
            }

            for (int i = 0; i < nodeJsonData.Count; i++)
            {
                var nodeJson = nodeJsonData[i];
                var nodeGraph = ParseTriggerActionNode(nodeJson, floor);
                if (nodeGraph != null)
                {
                    nodeList.Add(nodeGraph);
                }
            }
            return nodeList;
        }

        public List<TriggerNodeGraph> nodeGraphList;
        public static string NameSpaceName = "BattleTrigger.Editor";
        public TriggerNodeGraph ParseTriggerActionNode(JsonData nodeJsonData, int floor)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            TriggerNodeGraph nodeGraph = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseTriggerActionNode fullName : " + fullName);
            var resultClassName = fullName + "Graph";
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(TriggerNodeGraph)))
                {
                    nodeGraph = Activator.CreateInstance(type) as TriggerNodeGraph;
                    nodeGraph.Parse(nodeJsonData, floor);
                }
            }
            else
            {
                Logx.LogError("the type of node is not found : " + resultClassName);
            }

            return nodeGraph;


        }

        public Rect triggerViewRect = new Rect(280, 40, 700, 435);
        public Vector2 triggerProcessScrollPos;
        public void DrawSingleTriggerView()
        {
            if (null == rootGroupGraph)
            {
                return;
            }

            //工具栏部分
            var toolBarAreaRect = new Rect(280, 10, 700, 30);
            GUILayout.BeginArea(toolBarAreaRect, EditorStyles.helpBox);
            GUILayout.BeginHorizontal(new GUILayoutOption[] { });
            GUILayout.FlexibleSpace();
            GUILayout.Space(10);
            if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(25) }))
            {
                this.OnClickSaveBtn();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();



            //触发器流程部分
            GUI.Box(triggerViewRect, "");
            var rect = triggerViewRect;
            rect.x += 20;
            rect.y += 20;
            rect.width = 500;
            rect.height = 435;//dynamic

            GUILayout.BeginArea(rect);
            triggerProcessScrollPos = GUILayout.BeginScrollView(triggerProcessScrollPos, false, true, new GUILayoutOption[] { });
            GUILayout.Label("触发事件:");
            rootEventGraph.Draw();
            GUILayout.Label("执行动作:");
            rootGroupGraph.Draw(rect);

            GUILayout.EndScrollView();


            GUILayout.EndArea();
            //触发器节点属性部分
            this.DrawSelectTriggerNodeInfo();

        }

        public void OnClickSaveBtn()
        {
            if (selectTriggerFileIndex < 0)
            {
                return;
            }

            JsonData root = new JsonData();
            root["triggerEvent"] = this.rootEventGraph.ToJson();
            root["executeGroup"] = rootGroupGraph.ToJson();
            var resultJson = root.ToJson();


            var file = fileList[selectTriggerFileIndex];
            var path = Path.Combine(this.triggerConfigFolderPath, file);
            JsonTool.SaveJson(path, resultJson);

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("提示", "保存成功", "确定");
        }

        public TriggerNodeGraph currSelectTriggerNodeGraph;

        Vector2 triggerSelectNodeProcessScrollPos;
        public void DrawSelectTriggerNodeInfo()
        {
            if (null == currSelectTriggerNodeGraph)
            {
                return;
            }
            var rect = triggerViewRect;
            rect.x += 560;
            rect.y += 20;
            rect.width = 460;
            rect.height = 440;//dynamic

            GUILayout.BeginArea(rect);

            triggerSelectNodeProcessScrollPos = GUILayout.BeginScrollView(triggerSelectNodeProcessScrollPos, false, true, new GUILayoutOption[] { });


            GUILayout.BeginVertical();

            currSelectTriggerNodeGraph.DrawSelectInfo();

            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        public void LoadAllRes()
        {

        }

        public void SetSelectTriggerNodeGraph(TriggerNodeGraph nodeGraph)
        {
            this.currSelectTriggerNodeGraph = nodeGraph;
        }


        private void OnCommand_Ctrl_C()
        {
            if (currSelectTriggerNodeGraph != null)
            {
                currCopyNodeGraph = currSelectTriggerNodeGraph;
            }
        }

        public void OnCommand_Ctrl_V()
        {
            if (currCopyNodeGraph != null && currSelectTriggerNodeGraph != null)
            {
                PasteNode(currCopyNodeGraph, currSelectTriggerNodeGraph);
            }
        }

        public void OnCommand_Del()
        {
            if (currSelectTriggerNodeGraph != null)
            {
                var groupGraph = currSelectTriggerNodeGraph.groupGraph;
                groupGraph.DeleteTriggerNodeGraph(currSelectTriggerNodeGraph);
            }
        }

        public TriggerNodeGraph currCopyNodeGraph;

        //当节点右键点击的时候
        public void OnRightClickNode(TriggerNodeGraph targetNodeGraph)
        {
            if (targetNodeGraph is TriggerEventGraph)
            {
                OpenTriggerEventNodeMenu(targetNodeGraph);
            }
            else
            {
                OpenTriggerActionNodeMenu(targetNodeGraph);
            }

        }

        //打开触发事件菜单
        public void OpenTriggerEventNodeMenu(TriggerNodeGraph targetNodeGraph)
        {
            GenericMenu menu = new GenericMenu();

            var typeList = triggerEventTypeList;
            foreach (var nodeType in typeList)
            {
                var typeName = nodeType.Name;
                var title = "更换触发事件为 " + typeName + " 节点";
                menu.AddItem(new GUIContent(title), false, (temp) =>
                {
                    //点击
                    var opType = (Type)temp;

                    var insObj = Activator.CreateInstance(opType, true) as TriggerEventGraph;
                    insObj.Create(targetNodeGraph.floor);

                    rootEventGraph = null;

                    rootEventGraph = insObj;
                    insObj.OnSelect();

                }, nodeType);
            }
            menu.ShowAsContext();
        }

        //打开触发动作菜单
        public void OpenTriggerActionNodeMenu(TriggerNodeGraph targetNodeGraph)
        {
            GenericMenu menu = new GenericMenu();

            var typeList = triggerNodeTypeList;
            foreach (var nodeType in typeList)
            {
                var typeName = nodeType.Name;
                var title = "新建动作节点/新建 " + typeName + " 节点";
                menu.AddItem(new GUIContent(title), false, (temp) =>
                {
                    //点击
                    var opType = (Type)temp;

                    var insObj = Activator.CreateInstance(opType, true) as TriggerNodeGraph;
                    insObj.Create(targetNodeGraph.floor);
                    var groupGraph = targetNodeGraph.groupGraph;
                    //当前选中几点的相对索引
                    if (targetNodeGraph is AssistantShowNodeGraph)
                    {
                        //辅助节点 插到最后一个
                        var assNodeGraph = targetNodeGraph as AssistantShowNodeGraph;
                        groupGraph.AddNewTriggerNodeGraphToEnd(insObj);
                        insObj.OnSelect();
                    }
                    else
                    {
                        //有效果的节点 插到选中节点之后
                        var nodeIndex = groupGraph.GetNodeIndex(targetNodeGraph);
                        if (nodeIndex >= 0)
                        {
                            groupGraph.AddNewTriggerNodeGraph(insObj, nodeIndex + 1);
                            insObj.OnSelect();
                        }
                    }
                }, nodeType);
            }

            menu.AddItem(new GUIContent("复制节点"), false, (t) =>
            {
                currCopyNodeGraph = targetNodeGraph;
            }, "");

            menu.AddItem(new GUIContent("粘贴节点"), false, (t) =>
            {
                PasteNode(currCopyNodeGraph, targetNodeGraph);
            }, "");

            menu.AddItem(new GUIContent("删除节点"), false, (t) =>
            {
                var groupGraph = targetNodeGraph.groupGraph;
                groupGraph.DeleteTriggerNodeGraph(targetNodeGraph);
            }, "");

            menu.ShowAsContext();
        }

        public void PasteNode(TriggerNodeGraph currCopyNodeGraph, TriggerNodeGraph targetNodeGraph)
        {
            if (currCopyNodeGraph != null)
            {
                var groupGraph = targetNodeGraph.groupGraph;
                var newObj = currCopyNodeGraph.Clone();
                var nodeIndex = groupGraph.GetNodeIndex(targetNodeGraph);
                groupGraph.AddNewTriggerNodeGraph(newObj, nodeIndex + 1);

                newObj.SetFloorIncludeChildren(targetNodeGraph.floor);

                newObj.OnSelect();
            }
        }

    }
}