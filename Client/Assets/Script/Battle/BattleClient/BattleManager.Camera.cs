using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;

using Battle_Client;
using GameData;
using NetProto;

using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
  
    //战斗中的摄像机相关
    public partial class BattleManager
    {
        public Vector3 cameraPosOffset = new Vector3(0, 10, -3.2f);
        
        public Quaternion cameraRotationOffset;

        public void SetCameraInfo()
        {
            //先这样去 之后增加 scene 读取的接口
            var sceneRoot = GameObject.Find("_scene_root").transform;
            // var sceneObj = sceneRoot.GetChild(0).gameObject;
            // sceneObj.transform.position = new Vector3(0, 0, 0);

            var tempCameraTran = sceneRoot.Find("Camera");

            var camera3D = CameraManager.Instance.GetCamera3D();
            camera3D.SetPosition(tempCameraTran.position);
            camera3D.SetRotation(tempCameraTran.rotation);
            cameraRotationOffset = tempCameraTran.rotation;

            // //地图 cell 视图工具查看器(目前只限本地战斗)
            // if (BattleManager.Instance.IsLocalBattle())
            // {
            //     mapCellView = sceneObj.GetComponent<MapCellView>();
            //     var map = BattleManager.Instance.GetLocalBattleMap();
            //     mapCellView?.SetMap(map);
            //     //mapCellView.SetRenderPath(new List<Pos>());
            // }

        }

        public void UpdateCamera()
        {
            if (PlotManager.Instance.IsRunning())
            {
                return;
            }

            var heroObj = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            if (null == heroObj)
            {
                return;
            }

            var camera3D = CameraManager.Instance.GetCamera3D();
            var heroPos = heroObj.transform.position + cameraPosOffset;

            camera3D.SetPosition(heroPos);
            //camera3D.SetForward(heroPos);
            camera3D.SetRotation(this.cameraRotationOffset);
        }
        
    }
}