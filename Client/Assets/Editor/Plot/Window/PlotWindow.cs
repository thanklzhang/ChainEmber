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

namespace PlotDesigner.Editor
{
    public partial class PlotWindow : EditorWindow
    {
        public Vector2 scrollPos;
        public int selectPlotFileIndex = -1;
        public bool isRunning;
        public Vector2 mousePosition;
        public Rect plotPlayViewRect = new Rect(280, 80, 700, 435);
        private double preTime;
        public float currTime = 0.0f;
        float trackPlayOrgX = 130.0f;//轨道播放区域的起点偏移 x , 也是时间轴线的开始

        public List<string> fileList = new List<string>();
        List<TrackGraph> trackGraphList = new List<TrackGraph>();
        public Plot currPlot;

        public Transform plotRoot;
        public Transform plotMain;
        public Transform plotUIRoot;

        public static PlotWindow instance;

        public static PlotWindow window;
        [MenuItem("Tools/Plot Editor", false, 100)]
        public static void OpenPlotWindow()
        {
            window = EditorWindow.GetWindow<PlotWindow>(false, "Plot Editor");
            PlotWindow.instance = window;

            window.minSize = new Vector2(1000f, 100f);
            window.wantsMouseMove = true;
            window.Init();
        }

        public string plotConfigFolderPath;
        public List<Type> trackTypeList;
        public List<Type> trackNodeTypeList;

        public void Init()
        {
            plotConfigFolderPath = Application.dataPath + "/BuildRes/Plot";

            //读取所有剧情文件
            var path = plotConfigFolderPath;
            var filePaths = FileOperate.GetAllFilesFromFolder(path, "*.json");
            for (int i = 0; i < filePaths.Length; i++)
            {
                var filePath = filePaths[i];
                fileList.Add(Path.GetFileName(filePath));
            }

            //collect track and track node
            trackTypeList = new List<Type>();
            trackNodeTypeList = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    var currType = types[j];

                    //读取所有 Track 类型
                    if (!currType.IsAbstract)
                    {
                        if (typeof(Track).IsAssignableFrom(currType))
                        {
                            trackTypeList.Add(currType);
                        }

                        if (typeof(TrackNode).IsAssignableFrom(currType))
                        {
                            this.trackNodeTypeList.Add(currType);
                        }
                    }
                }

            }
        }

        public void OnGUI()
        {
            DrawAll();
        }

        public void OnEditorUpdate()
        {
            UpdatePlot();
            if (isRunning)
            {
                var deltaTime = EditorApplication.timeSinceStartup - preTime;
                preTime = EditorApplication.timeSinceStartup;

                currTime = currTime + (float)deltaTime;
            }
        }

        public void UpdatePlot()
        {
            UpdatePlotTrackList();
        }



        public void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
            preTime = EditorApplication.timeSinceStartup;

            Config.ConfigManager.Instance.Clear();
            
            Config.ConfigManager.Instance.LoadAllConfigDataByEditor();

            var gameInitPath = "Assets/Resources/GameInit.prefab";
            var gameInitPrefab = ResourceManager.Instance.GetObjectByEditor<GameObject>(gameInitPath);
            gameInit = GameObject.Instantiate(gameInitPrefab);


            plotRoot = gameInit.transform.Find("PlotRoot");
            plotMain = plotRoot.Find("Main");
            plotUIRoot = plotMain.Find("Canvas");

            plotMain.gameObject.SetActive(true);

            this.TestInit();

        }


        public void Update()
        {
            //   currTime += Time.deltaTime;

            //if (isRunning)
            //{
            //    this.Repaint();
            //}
            this.Repaint();

            //控制当前时间进程
            if (isCtrlTimeProgress)
            {
                var x = mousePosition.x - this.timeDegreeRect.x;
                currTime = x / PlotGraphDefine.lenPerTimeSpan * (1.0f / PlotGraphDefine.timeSpanCountPerSeconds);
            }

            //track 拖动节点
            if (currDragTrackNodeGraph != null)
            {
                var offset = mousePosition - beginClickMousePos;
                currDragTrackNodeGraph.Move(offset);
            }

            //track 拖动节点边界
            if (currDragBorderTrackNodeGraph != null)
            {
                currDragBorderTrackNodeGraph.SetTimeRangeByMousePosX(isLeftAtDraggingBorder, mousePosition.x);
            }


        }

        public void DrawAll()
        {
            HandleEvent();
            DrawPlotFileListView();
            DrawSinglePlotView();
            DrawNodeInfoView();
        }

        public Vector2 beginClickMousePos;

        public void HandleRightClick()
        {
            //单个剧情区域
            if (plotPlayViewRect.Contains(mousePosition))
            {
                TrackGraph track = null;
                for (int i = 0; i < trackGraphList.Count; i++)
                {
                    var t = trackGraphList[i];
                    //右击轨道标题部分
                    if (t.IsTitleAreaCantainsPoint(mousePosition))
                    {
                        track = t;
                        this.OnRightClickTrackTitleArea(t);
                        Event.current.Use();
                        break;
                    }
                    //右击轨道播放部分
                    if (t.IsPlayAreaCantainsPoint(mousePosition))
                    {
                        track = t;
                        this.OnRightClickTrackPlayArea(t, mousePosition);
                        Event.current.Use();
                        break;
                    }
                }

                //点击空白区域
                if (null == track)
                {
                    this.OnRightClickTrackBlankArea();
                }
            }

        }

        public void OnRightClickTrackBlankArea()
        {
            GenericMenu menu = new GenericMenu();

            foreach (var trackType in trackTypeList)
            {
                var typeName = trackType.Name;
                var title = "新建 " + typeName;
                menu.AddItem(new GUIContent(title), false, (temp) =>
                {
                    var opType = (Type)temp;

                    var insObj = Activator.CreateInstance(opType, true) as Track;
                    this.AddTrack(insObj);

                }, trackType);
            }

            menu.ShowAsContext();
        }

        public void HandleEvent()
        {
            if (null == Event.current)
            {
                return;
            }

            mousePosition = Event.current.mousePosition;
            if (EventType.MouseDown == Event.current.type)
            {
                if (1 == Event.current.button)
                {
                    HandleRightClick();
                }
                else if (0 == Event.current.button)
                {
                    beginClickMousePos = mousePosition;

                    if (IsMousePosInTimeCtrlProgress(mousePosition))
                    {
                        isCtrlTimeProgress = true;
                    }

                    if (IsNodeBorderCantainsPoint(mousePosition, out currDragBorderTrackNodeGraph, out isLeftAtDraggingBorder))
                    {
                        //isDraggingTrackNodeBorder = true;
                    }
                    else
                    {
                        if (IsMousePosInTrackNode(mousePosition, out currDragTrackNodeGraph))
                        {
                            currDragTrackNodeGraph.OnStartDrag();
                            currDragTrackNodeGraph.OnClick();
                        }
                    }
                }

            }
            else if (EventType.MouseUp == Event.current.type)
            {
                if (0 == Event.current.button)
                {
                    if (isCtrlTimeProgress)
                    {
                        isCtrlTimeProgress = false;
                    }

                    if (currDragTrackNodeGraph != null)
                    {
                        currDragTrackNodeGraph = null;
                    }

                    if (currDragBorderTrackNodeGraph != null)
                    {
                        currDragBorderTrackNodeGraph = null;
                    }

                }
            }
        }

        public TrackNodeGraph currDragTrackNodeGraph;

        public TrackNodeGraph currDragBorderTrackNodeGraph;
        public bool isDraggingTrackNodeBorder;
        public bool isLeftAtDraggingBorder;

        public bool isCtrlTimeProgress;

        public bool IsMousePosInTimeCtrlProgress(Vector2 mousePos)
        {
            if (timeDegreeRect.Contains(mousePos))// || timeCtrlBoxRect.Contains(mousePos)
            {
                return true;
            }
            return false;
        }

        public bool IsMousePosInTrackPlayArea(Vector2 mousePos, out TrackGraph track)
        {
            for (int i = 0; i < trackGraphList.Count; i++)
            {
                var t = trackGraphList[i];
                if (t.IsPlayAreaCantainsPoint(mousePos))
                {
                    track = t;
                    return true;
                }
            }

            track = null;
            return false;
        }

        public bool IsMousePosInTrackTitleArea(Vector2 mousePos, out TrackGraph track)
        {
            for (int i = 0; i < trackGraphList.Count; i++)
            {
                var t = trackGraphList[i];
                if (t.IsTitleAreaCantainsPoint(mousePos))
                {
                    track = t;
                    return true;
                }
            }

            track = null;
            return false;
        }

        public bool IsNodeBorderCantainsPoint(Vector2 mousePos, out TrackNodeGraph track, out bool isLeft)
        {
            for (int i = 0; i < trackGraphList.Count; i++)
            {
                var t = trackGraphList[i];
                if (t.IsNodeBorderCantainsPoint(mousePos, out track, out isLeft))
                {
                    return true;
                }
            }

            track = null;
            isLeft = false;
            return false;
        }

        public bool IsMousePosInTrackNode(Vector2 mousePos, out TrackNodeGraph track)
        {
            for (int i = 0; i < trackGraphList.Count; i++)
            {
                var t = trackGraphList[i];
                if (t.IsNodeCantainsPoint(mousePos, out track))
                {
                    return true;
                }
            }

            track = null;
            return false;
        }

        public void AddTrack(Track track)
        {
            currPlot.AddTrack(track);
            SetAllTrackGraph();
        }

        public void DeleteTrack(TrackGraph trackGraph)
        {
            currPlot.DeleteTrack(trackGraph.trackData);
            //trackGraphList.Remove(trackGraph);
            SetAllTrackGraph();
        }

        public void OnRightClickTrackTitleArea(TrackGraph trackGraph)
        {
            if (trackGraph != null)
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("删除"), false, (temp) =>
                {
                    DeleteTrack((TrackGraph)temp);
                }, trackGraph);

                menu.ShowAsContext();
            }
        }

        public void OnRightClickTrackPlayArea(TrackGraph trackGraph, Vector2 mousePosition)
        {
            if (trackGraph != null)
            {
                TrackNodeGraph nodeGraph = null;

                var isHaveNodeOnThisPos = IsMousePosInTrackNode(mousePosition, out nodeGraph);
                if (isHaveNodeOnThisPos)
                {
                    //有 trackNode , 执行节点操作 并选中
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("删除"), false, (temp) =>
                    {
                        this.DeleteTrackNode(trackGraph, nodeGraph);
                    }, 0);
                    menu.ShowAsContext();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();

                    foreach (var trackType in trackNodeTypeList)
                    {
                        var typeName = trackType.Name;
                        var title = "新建 " + typeName;
                        menu.AddItem(new GUIContent(title), false, (temp) =>
                        {
                            var opType = (Type)temp;

                            var insObj = Activator.CreateInstance(opType, true) as TrackNode;

                            float currTime = PlotGraphTool.GetTimeByGraphX(mousePosition.x - plotPlayViewRect.x - trackPlayOrgX);
                            float startTime = currTime - 1.0f;
                            float endTime = currTime + 1.0f;
                            insObj.startTime = startTime;
                            insObj.endTime = endTime;
                            this.AddTrackNode(trackGraph, insObj);

                        }, trackType);
                    }

                    menu.ShowAsContext();
                }
            }
        }

        private void AddTrackNode(TrackGraph trackGraph, TrackNode insObj)
        {
            this.currPlot.AddTrackNode(trackGraph.trackData, insObj);
            SetAllTrackGraph();
        }

        private void DeleteTrackNode(TrackGraph trackGraph, TrackNodeGraph nodeGraph)
        {
            this.currPlot.DeleteTrackNode(trackGraph.trackData, nodeGraph.nodeData);
            SetAllTrackGraph();
        }

        private void OnClickSaveBtn()
        {
            var file = fileList[selectPlotFileIndex];
            var path = Path.Combine(plotConfigFolderPath, file);
            JsonTool.SaveObject(path, this.currPlot);

            EditorUtility.DisplayDialog("提示", "保存成功", "确定");
        }

        //文件列表部分
        public void DrawPlotFileListView()
        {
            GUILayout.BeginArea(new Rect(0, 0, 250, 500), EditorStyles.helpBox);
            GUILayout.Label("剧情文件");
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, new GUILayoutOption[] { });


            for (int i = 0; i < fileList.Count; i++)
            {
                var file = fileList[i];
                var isSelect = selectPlotFileIndex == i;
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

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }




        public void OnClickFile(int index)
        {
            if (selectPlotFileIndex == index)
            {
                return;
            }
            ClearTarckRes();
            //Logx.Log("OnClickFile : " + index);
            selectPlotFileIndex = index;

            //加载所需资源
            var file = fileList[selectPlotFileIndex];
            //加载剧情文件资源
            //load file
            var path = Path.Combine(this.plotConfigFolderPath, file);

            //string json = JsonTool.LoadObjectFromFile(path);

            currPlot = JsonTool.LoadObjectFromFile<Plot>(path);
            if (null == currPlot)
            {
                currPlot = new Plot();
            }
            currPlot.Init();
            //currPlot.Load(json);
            LoadAllRes();

            SetAllTrackGraph();

        }

        GameObject gameInit;
        public void LoadAllRes()
        {
            if (currPlot.trackDataList != null)
            {
                foreach (var trackData in currPlot.trackDataList)
                {
                    if (trackData.trackNodeList != null)
                    {
                        foreach (var trackNodeData in trackData.trackNodeList)
                        {
                            var id = trackNodeData.id;
                            if (!resDic.ContainsKey(id))
                            {
                                if (id > 0)
                                {
                                    var resId = trackNodeData.resId;
                                    if (resId > 0)
                                    {
                                        //根据资源 id 可以得到是什么类型 之后添加
                                        var prefab = ResourceManager.Instance.GetObjectByConfiIdEditor<GameObject>(resId);
                                        GameObject res = GameObject.Instantiate(prefab);
                                        resDic.Add(trackNodeData.id, res);
                                    }
                                }

                            }
                        }
                    }

                }
            }

        }

        public void ClearTarckRes()
        {
            foreach (var item in this.resDic)
            {
                var obj = item.Value;
                if (obj is GameObject)
                {
                    DestroyImmediate(obj);
                }
            }
            this.resDic.Clear();
        }

        public void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;

            Config.ConfigManager.Instance.Clear();

            ClearTarckRes();

            DestroyImmediate(gameInit);
        }

        Dictionary<int, UnityEngine.Object> resDic = new Dictionary<int, UnityEngine.Object>();
        public UnityEngine.Object GetPlotUnitById(int id)
        {
            if (resDic.ContainsKey(id))
            {
                return resDic[id];
            }
            //Logx.LogWarning("GetPlotResById : the res is not found , id : " + id);
            return null;
        }

        Rect timeDegreeRect;

        //单个剧情部分
        public void DrawSinglePlotView()
        {
            //剧情工具栏---

            GUILayout.BeginArea(new Rect(280, 10, 700, 30), EditorStyles.helpBox);
            GUILayout.BeginHorizontal(new GUILayoutOption[] { });
            GUILayout.FlexibleSpace();
            //if (GUILayout.Button("重新播放", new GUILayoutOption[] { /*GUILayout.Width(50), GUILayout.Height(50)*/ }))
            //{
            //    preTime = EditorApplication.timeSinceStartup;
            //    currTime = 0.0f;
            //    isRunning = true;
            //}
            GUILayout.Space(10);
            if (isRunning)
            {
                if (GUILayout.Button("暂停", new GUILayoutOption[] { /*GUILayout.Width(50), GUILayout.Height(50)*/ }))
                {
                    isRunning = false;
                }
            }
            else
            {
                if (GUILayout.Button("播放", new GUILayoutOption[] { /*GUILayout.Width(50), GUILayout.Height(50)*/ }))
                {
                    preTime = EditorApplication.timeSinceStartup;
                    isRunning = true;
                }
            }

            GUILayout.Space(10);
            if (GUILayout.Button("结束", new GUILayoutOption[] { /*GUILayout.Width(50), GUILayout.Height(50)*/ }))
            {
                currTime = 0.0f;
                isRunning = false;
            }
            GUILayout.Space(100);
            if (GUILayout.Button("保存", new GUILayoutOption[] { /*GUILayout.Width(50), GUILayout.Height(50)*/ }))
            {
                this.OnClickSaveBtn();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            //剧情播放部分---

            GUI.Box(plotPlayViewRect, "");

            //当前时间
            var currTimeStyle = new GUIStyle();
            currTimeStyle.normal.textColor = Color.red;
            var showTimeStr = "" + Math.Round(currTime, 2);
            GUI.Label(plotPlayViewRect, showTimeStr, currTimeStyle);

            //时间刻度
            float timeMaxLen = 700.0f;
            float timeLineOrgX = plotPlayViewRect.x + (trackPlayOrgX + 1);

            float upSpaceY = 10.0f;

            Handles.DrawLine(new Vector2(timeLineOrgX, plotPlayViewRect.y + upSpaceY), new Vector2(timeLineOrgX + timeMaxLen - trackPlayOrgX, plotPlayViewRect.y + upSpaceY));

            float shortLine = 5.0f;
            float longLine = 10.0f;
            timeDegreeRect = new Rect(timeLineOrgX, plotPlayViewRect.y + upSpaceY - 45, timeMaxLen - trackPlayOrgX, longLine + 45);

            for (int i = 0; i < PlotGraphDefine.timeSpanCountPerSeconds * PlotGraphDefine.showSecondsCount + 1; i++)
            {
                float currTimeX = timeLineOrgX + i * PlotGraphDefine.lenPerTimeSpan;
                var currTimeLineLen = (0 == i % PlotGraphDefine.timeSpanCountPerSeconds) ? longLine : shortLine;
                Handles.DrawLine(new Vector2(currTimeX, plotPlayViewRect.y + upSpaceY), new Vector2(currTimeX, plotPlayViewRect.y + upSpaceY + currTimeLineLen));
            }


            //轨道
            for (int i = 0; i < trackGraphList.Count; i++)
            {
                var trackGraph = trackGraphList[i];
                trackGraph.Draw(plotPlayViewRect.min);
            }

            //当前播放时间轴线
            var timeProgressOrgX = timeLineOrgX + PlotGraphDefine.timeSpanCountPerSeconds * currTime * PlotGraphDefine.lenPerTimeSpan;

            var timeProgressLineLen = 500.0f;
            var preColor = Handles.color;
            Handles.color = Color.red;
            Handles.DrawLine(new Vector2(timeProgressOrgX, plotPlayViewRect.y - 10), new Vector2(timeProgressOrgX, plotPlayViewRect.y + timeProgressLineLen));
            Handles.color = preColor;

            var timeProgressBoxOrgY = plotPlayViewRect.y - 10;
            GUI.backgroundColor = Color.white;
            var style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;
            timeCtrlBoxRect = new Rect(timeProgressOrgX - 7, timeProgressBoxOrgY - 22, 12, 22);
            GUI.Box(timeCtrlBoxRect, "", style);



        }

        Rect timeCtrlBoxRect;

        public void SetAllTrackGraph()
        {
            trackGraphList.Clear();

            //轨道
            float trackHeight = PlotGraphDefine.trackHeight;
            float trackPlayWidth = PlotGraphDefine.trackPlayWidth;
            float trackSpaceY = PlotGraphDefine.trackSpaceY;

            var rect = plotPlayViewRect;
            rect.y += 10.0f + 10.0f;
            if (currPlot.trackDataList != null)
            {
                for (int i = 0; i < currPlot.trackDataList.Count; i++)
                {
                    var trackData = currPlot.trackDataList[i];

                    TrackGraph tGraph = new TrackGraph();
                    tGraph.SetData(trackData);
                    tGraph.titleRect = new Rect(rect.x, rect.y + i * (trackHeight + trackSpaceY), trackPlayOrgX - 5.0f, trackHeight);
                    tGraph.playNodeRect = new Rect(rect.x + trackPlayOrgX, rect.y + i * (trackHeight + trackSpaceY), trackPlayWidth, trackHeight);
                    trackGraphList.Add(tGraph);

                }
            }

        }
      
        public void SetCurrSelectSO(ScriptableObject currSelectSO,TrackNodeGraph nodeGraph)
        {
            Selection.activeObject = currSelectSO;
        }

        public void RefreshInspector()
        {
            //EditorUtility.SetDirty(currSelectSO);
        }
        //public int t1;

        //SerializedObject test;
        //SerializedProperty testPro;

        public void TestInit()
        {
            //currSelectNode = new TrackNode()//TransformTrackNode
            //{
            //    id = 1,
            //    resId = 2,
            //    startTime = 1.1f,
            //    endTime = 3.5f,
            //    //startPos = new Vector3(1, 2, 3),
            //    //endPos = new Vector3(2, 2, 10),
            //    //startForward = new Vector3(1, 1, 1),
            //    //endForward = new Vector3(2, 1, 1),
            //};

            //test = new SerializedObject(this);
            //testPro = test.FindProperty("t1");
        }

        //单个节点信息
        string nodeName;
        public void DrawNodeInfoView()
        {


            float x = plotPlayViewRect.x + plotPlayViewRect.width + 10.0f;
            float y = 5.0f;
            float width = 300.0f;
            float height = 500.0f;
            var rect = new Rect(x, y, width, height);

            ////GUI.Box(rect, "");
            //GUILayout.BeginArea(rect, "");
            //GUILayout.BeginVertical();

            //GUILayout.Label("节点信息");
            //GUILayout.Space(5.0f);

            //var property = testPro;

            //EditorGUILayout.PropertyField(property,true);

            ////var nodeType = currSelectNode.GetType();
            //////nodeName = EditorGUILayout.TextField(nodeName, new GUILayoutOption[0]);
            ////BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            ////FieldInfo[] fields = nodeType.GetFields(bindingAttr);
            ////for (int i = 0; i < fields.Length; i++)
            ////{
            ////    var field = fields[i];
            ////    GUILayout.BeginHorizontal();
            ////    GUILayout.Label(field.Name);
            ////    //field.GetValue(currSelectNode)
            ////    //GUILayout.TextField();

            ////    GUILayout.EndHorizontal();
            ////}

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
        }


    }
}