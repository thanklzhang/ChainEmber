using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using GameData;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    //战斗中的加载创建相关
    public partial class BattleManager
    {
        public MapSaveData mapSaveData;
        
        public IEnumerator StartLoad()
        {
            if (this.battleType == BattleType.Remote)
            {
                yield return StartLoad_SimulateServer();
            }
            OnLoadFinish();
        }

        /// <summary>
        /// 远端战斗加载(模拟后端)
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_SimulateServer()
        {
            //创建战斗数据
            CreateBattleData(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }

      
        //加载地图
        public IEnumerator LoadMapData(int battleConfigId, Action<MapSaveData> finishCallback)
        {
            var battleConfigTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleConfigTb.MapId);

            var isFinish = false;
            // var mapList = new List<List<int>>();
            var mapSaveData = new MapSaveData();
            var path = GlobalConfig.buildPath + "/" + mapConfig.MapDataPath;
            ResourceManager.Instance.GetObject<TextAsset>(path, (textAsset) =>
            {
                //Logx.Log("local execute : load text finish: " + textAsset.text);
                var json = textAsset.text;
                // mapList = LitJson.JsonMapper.ToObject<List<List<int>>>(json);
                mapSaveData = LitJson.JsonMapper.ToObject<MapSaveData>(json);
                isFinish = true;
            });

            while (!isFinish)
            {
                yield return null;
            }

            // finishCallback?.Invoke(mapList);
            finishCallback?.Invoke(mapSaveData);
        }

        public IEnumerator StartLoad_Common()
        {
            Logx.Log(LogxType.Game, "StartLoad_Common : load start");


            //读取战斗相关配置数据
            var battleTableId = BattleManager.Instance.battleConfigId;
            var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleTableId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleTb.MapId);
            // var battleTriggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(battleTb.TriggerId);
            var sceneResTb = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(mapConfig.ResId);


            //加载场景
            EventSender.SendLoadingProgress(0.3f, "加载 场景 中");
            yield return SceneLoadManager.Instance.LoadRequest(sceneResTb.Name);
            SetSceneInfo();
            SetCameraInfo();

            Logx.Log(LogxType.Game, "StartLoad_Common : scene load finish");

            //加载 UI 并打开
            EventSender.SendLoadingProgress(0.4f, "加载 战斗界面 中");
            yield return UIManager.Instance.EnterRequest<BattleUI>();
            Logx.Log(LogxType.Game, "StartLoad_Common : BattleUICtrl load finish");
            //battle ui
            // objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });

            //战斗实体资源
            EventSender.SendLoadingProgress(0.5f, "加载 战斗实体 中");
            yield return BattleEntityManager.Instance.LoadInitEntities();
            Logx.Log(LogxType.Game, "StartLoad_Common : entities load finish");

            // ShowMapPosView();
        }

        void SetSceneInfo()
        {
            sceneRoot = GameObject.Find("_scene_root").transform;
        }

        //显示辅助地图坐标
        void ShowMapPosView()
        {
            var temp = Resources.Load("GridPos") as GameObject;
            var list = mapSaveData.mapList;
            for (int i = 0; i < list.Count; i++)
            {
                var row = i;
                var l = list[i];
                for (int j = 0; j < l.Count; j++)
                {
                    var line = j;
                    var ins = GameObject.Instantiate(temp, null, false);
                    ins.transform.position = new UnityEngine.Vector3(row, 0, line);
                    ins.GetComponent<TextMesh>().text = "(" + row + "," + line + ")";
                }
            }
        }

        public void OnLoadFinish()
        {
            this.playerInput.InitPlayerInput();
            // var understudyRoot = sceneRoot.Find("UnderstudyArea");
            // UnderstudyManager_Client.Instance.Init(understudyRoot);
        }

        #region 开始战斗

        //开始远端战斗(现在是本地服务模拟战斗)
        public void StartSimulateRemoteBattle(BattleClient_CreateBattleArgs battleClientArgs)
        {
            battleType = BattleType.Remote;

            this.battleClientArgs = battleClientArgs;

            Logx.Log(LogxType.Game, "start a remote battle");

            //进入战斗场景
            SceneCtrlManager.Instance.Enter<BattleSceneCtrl>();
        }

        #endregion
    }
}