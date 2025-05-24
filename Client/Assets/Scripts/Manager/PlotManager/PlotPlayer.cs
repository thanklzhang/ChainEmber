using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotEntity
    {
        public int id;
        public int res;
        public UnityEngine.Object obj;
    }

    //单个剧情播放者
    public class PlotPlayer
    {
        Plot plotData;
        List<PlotTrackPlayer> trackPlayerList;
        public Dictionary<int, PlotEntity> plotEntityDic;

        float currTime;
        bool isRunning;
        bool isEnd;
        string assembleName;



        bool isLoading;

        Dictionary<int, int> gameObjectResIdToCountDic;

        float maxEndTime;

        public Action<string> endAction;
        public void Init()
        {
            this.Reset();
        }

        public void Reset()
        {

            trackPlayerList = new List<PlotTrackPlayer>();
            plotEntityDic = new Dictionary<int, PlotEntity>();
            assembleName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            plotData = null;
           
            currTime = 0.0f;
            isRunning = false;
            isEnd = false;
            isLoading = false;
            gameObjectResIdToCountDic = null;
            maxEndTime = 0.0f;
            endAction = null;
        }


        internal void StartPlot(Plot plot, Action<string> endAction = null)
        {
            // Logx.Log("plot : StartPlot");
            this.endAction = endAction;

            plotMain.gameObject.SetActive(true);

            this.plotData = plot;

            //填充数据
            for (int i = 0; i < plotData.trackDataList.Count; i++)
            {
                var trackData = plotData.trackDataList[i];

                PlotTrackPlayer trackPlayer = CreateTrackPlayerByTrackDataType(trackData.GetType());
                if (trackPlayer != null)
                {
                    trackPlayer.Init(trackData, this);
                    trackPlayerList.Add(trackPlayer);
                }
                else
                {
                    var trackPlayerName = "Plot" + trackData.GetType().Name + "Player";
                    Logx.LogError("PlotPlayer : SetPlot : create object fail , the type of src is : " + trackData.GetType() + ", perhaps the trackPlayer is not exist : " + trackPlayerName);
                }
            }

            this.StartCollectInfo();

        }

        private PlotTrackPlayer CreateTrackPlayerByTrackDataType(Type type)
        {
            var trackPlayerName = "Plot" + type.Name + "Player";
            var nameSpaceName = "PlotDesigner.Runtime";
            var fullName = nameSpaceName + "." + trackPlayerName;
            PlotTrackPlayer trackPlayer = null;
            //Logx.Log("CreateInstance : " + assembleName + " : " + fullName);
            trackPlayer = Assembly.Load(assembleName).CreateInstance(fullName) as PlotTrackPlayer;
            return trackPlayer;
        }

        Transform plotRoot;
        Transform plotMain;
        Transform plotUIRoot;

        internal void SetPlotRoot(Transform plotRoot)
        {
            this.plotRoot = plotRoot;
            plotMain = plotRoot.Find("Main");
            plotUIRoot = plotMain.Find("Canvas");
        }

        internal Transform GetPlotUIRoot()
        {
            return plotUIRoot;
        }

        int needLoadCount = 0;
        int finishLoadCount = 0;
        private LoadResGroupRequest loadRequest;

        public void StartCollectInfo()
        {
            // Logx.Log("plot : StartCollectInfo");

            gameObjectResIdToCountDic = new Dictionary<int, int>();
            plotEntityDic = new Dictionary<int, PlotEntity>();
            //收集资源
            //这里可以改成收敛到一处 直接找 id 和 res 的对应即可
            for (int i = 0; i < plotData.trackDataList.Count; i++)
            {
                var trackData = plotData.trackDataList[i];

                foreach (var trackNode in trackData.trackNodeList)
                {
                    var id = trackNode.id;
                    var resId = trackNode.resId;
                    //注意这里 如果有两个 resId 都填了 但是是一个 1 个 id  那么就不对了 可能影响加载
                    //所以之后这里要统一要改成： 剧情中直接给出 id 列表 这样直接找 id 和 res 的对应 并计算 res 数量
                    if (resId > 0)
                    {
                        //这里先只收集 gameObject
                        var isGo = Config.ResourceConfig_Tool.IsGameObject(resId);
                        if (isGo)
                        {
                            if (gameObjectResIdToCountDic.ContainsKey(resId))
                            {
                                gameObjectResIdToCountDic[resId] += 1;
                            }
                            else
                            {
                                gameObjectResIdToCountDic.Add(resId, 1);
                            }

                            needLoadCount += 1;
                        }

                        //填充剧情实体
                        if (!plotEntityDic.ContainsKey(id))
                        {
                            PlotEntity pe = new PlotEntity();
                            pe.id = id;
                            pe.res = resId;
                            plotEntityDic.Add(id, pe);
                        }
                    }


                }
            }

            //收集显示对象

            this.StartLoadRes();
        }

        public void StartLoadRes()
        {
            //Logx.Log("plot : StartLoadRes , gameObjectResIdToCountDic count : " + gameObjectResIdToCountDic.Count);

            var objsRequestList = new List<LoadObjectRequest>();

            foreach (var item in gameObjectResIdToCountDic)
            {
                var resId = item.Key;
                var count = item.Value;

                var resTb = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(resId);
                var path = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

                objsRequestList.Add(new LoadGameObjectRequest(path, count) { selfFinishCallback = (hash) => { OnOneRequestListFinish(resId, hash); } });

            }

            this.loadRequest = ResourceManager.Instance.LoadObjects(objsRequestList);

            isLoading = true;
        }

        public void OnOneRequestListFinish(int resId, HashSet<GameObject> gameObjects)
        {
            // Logx.Log("plot : OnOneRequestListFinish : " + gameObjects.Count);

            foreach (var go in gameObjects)
            {
                foreach (var item in this.plotEntityDic)
                {
                    var plotEntity = item.Value;
                    if (plotEntity.res == resId && null == plotEntity.obj)
                    {
                        plotEntity.obj = go;
                    }
                }
            }

            //finishLoadCount = finishLoadCount + gameObjects.Count;

            //if (finishLoadCount >= needLoadCount)
            //{
            //    this.OnLoadFinish();
            //}
        }

        public void OnLoadFinish()
        {
            // Logx.Log("plot : OnLoadFinish , load all finish");

            isLoading = false;

            this.StartExecute();
        }

        internal void StartExecute()
        {
            // Logx.Log("plot : StartExecute");

            maxEndTime = GetTotalTime();

            isRunning = true;
            for (int i = 0; i < trackPlayerList.Count; i++)
            {
                var trackPlayer = trackPlayerList[i];
                trackPlayer.StartExecute();
            }
        }

        internal PlotEntity GetPlotEntityById(int id)
        {
            if (plotEntityDic.ContainsKey(id))
            {
                return plotEntityDic[id];
            }
            return null;
        }

        public void Update(float timeDelta)
        {
            if (isLoading)
            {
                var isFinish = this.loadRequest.CheckFinish();
                if (isFinish)
                {
                    this.OnLoadFinish();
                }
            }

            if (!isRunning || isEnd)
            {
                return;
            }

            for (int i = 0; i < trackPlayerList.Count; i++)
            {
                var trackPlayer = trackPlayerList[i];
                trackPlayer.Update(timeDelta);
            }

            currTime = currTime + timeDelta;

            if (currTime >= maxEndTime)
            {
                //isRunning = false;
                OnEnd();
            }
        }

        public float GetTotalTime()
        {
            //获取所有轨道的 endTime 最大的时间
            float maxEndTime = 0;
            foreach (var trackPlayer in this.trackPlayerList)
            {
                float endTime = trackPlayer.GetMaxEndTime();
                if (endTime >= maxEndTime)
                {
                    maxEndTime = endTime;
                }
            }

            return maxEndTime;
        }

        public float GetCurrTime()
        {
            return currTime;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        //结束
        public void OnEnd()
        {
            // Logx.Log("plot : OnEnd , currTime : " + currTime);
            isEnd = true;
            this.endAction.Invoke("");

        }

      
        //关闭
        public void Close()
        {
            // Logx.Log("plot : Close ");

            plotMain.gameObject.SetActive(false);

            //释放资源
            foreach (var item in plotEntityDic)
            {
                var plotEntity = item.Value;
                //目前只看 gameObject
                if (plotEntity.obj is GameObject)
                {
                    var go = plotEntity.obj as GameObject;

                    var resId = plotEntity.res;
                    var resTb = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(resId);
                    var path = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

                    ResourceManager.Instance.ReturnObject(path, go);
                }
            }

            Reset();
        }

    }
}

