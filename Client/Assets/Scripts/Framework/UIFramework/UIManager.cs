using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrlArgs
{
}

public enum CtrlState
{
    Null = 0,

    Inited,

    Loading,

    Active,

    EnterAni,

    ExitAni,

    Inactive,

    Exit,
}

public enum CtrlShowMode
{
    Fixed = 0,
    Float = 1,
    Free = 2
}


public class UIManager : Singleton<UIManager>
{
    Transform uiRoot;

    public List<BaseUI> ctrlCacheList = new List<BaseUI>();

    //全局贮存 UI 一直存在
    public Dictionary<Type, BaseUI> globalCtrlDic = new Dictionary<Type, BaseUI>();


    public RectTransform uiRootRectTran;
    
    //public BaseCtrl currCtrl;

    public BaseUI CurrFixedCtrl;

    public Dictionary<UIShowLayer, Transform> layerRootDic;

    public static Dictionary<UIShowLayer, string> uiShowLayerDic = new Dictionary<UIShowLayer, string>()
    {
        { UIShowLayer.Floor_0, "FloorUIRoot/layer" },
        { UIShowLayer.Middle_0, "MiddleUIRoot/layer" },
        { UIShowLayer.Top_0, "TopUIRoot/layer" },
    };


    public IEnumerator LoadGlobalCtrlReq()
    {
        yield return GlobalUIMgr.Instance.LoadReq();
    }


    internal void Init(Transform uiRoot)
    {
        this.uiRoot = uiRoot;
        layerRootDic = new Dictionary<UIShowLayer, Transform>();
        foreach (var kv in uiShowLayerDic)
        {
            var type = kv.Key;
            var tranPath = kv.Value;
            var layerRoot = this.uiRoot.Find(tranPath);
            layerRootDic.Add(type, layerRoot);
        }

        uiRootRectTran = this.uiRoot.GetComponent<RectTransform>();
    }

    public void SetParent(BaseUI ctrl,GameObject uiGameObject)
    {

        var layerRoot = layerRootDic[ctrl.uiShowLayer];
        uiGameObject.transform.SetParent(layerRoot, false);
        uiGameObject.transform.SetAsLastSibling();
    }

    public void Open<T>(UICtrlArgs args = null, Action loadFinishCallback = null) where T : BaseUI, new()
    {
        //判断是否是全局 UI
        var globalCtrl = GlobalUIMgr.Instance.Get<T>();
        if (globalCtrl != null)
        {
            GlobalUIMgr.Instance.Open<T>(args);
            return;
        }

        var findCtrl = FindCtrl<T>();
        if (null == findCtrl)
        {
            //没找到 开始一个新的 ctrl

            // CurrMainCtrlPre = newCtrl;
            BaseUI newCtrl = new T();
            newCtrl.Init();

            newCtrl.StartLoad((uiGameObject) =>
            {
                loadFinishCallback?.Invoke();

                SetParent(newCtrl,uiGameObject);
                
                if (newCtrl.showMode == CtrlShowMode.Fixed)
                {
                    InactivePreCtrl();
                }

                ctrlCacheList.Add(newCtrl);

                newCtrl.Open(args);
                newCtrl.Active();
                //开始播放动效
                newCtrl.StartEnterAni(null);
            });
        }
        else
        {
            //已经存在 目前的方案是 ：直接把 ctrl 提到最前面

            loadFinishCallback?.Invoke();

            ctrlCacheList.Remove(findCtrl);

            var newCtrl = findCtrl;

            ctrlCacheList.Add(newCtrl);

            if (newCtrl.showMode == CtrlShowMode.Fixed)
            {
                InactivePreCtrl();
            }

            newCtrl.Active();
            //开始播放动效
            newCtrl.StartEnterAni(null);
        }
    }

    public IEnumerator EnterRequest<T>(UICtrlArgs args = null) where T : BaseUI, new()
    {
        yield return null;
        var isFinish = false;
        Open<T>(args, () => { isFinish = true; });

        while (true)
        {
            yield return null;

            if (isFinish)
            {
                yield break;
            }
        }
    }

    //之前的 ctrl 失效
    public void InactivePreCtrl()
    {
        for (int i = ctrlCacheList.Count - 1; i >= 0; i--)
        {
            var tempCtrl = ctrlCacheList[i];
            tempCtrl.Inactive();

            if (tempCtrl.showMode == CtrlShowMode.Fixed)
            {
                break;
            }
        }
    }

    public void CloseTopFixedUI()
    {
        var lastUI = ctrlCacheList[^1];
        if (lastUI.showMode == CtrlShowMode.Fixed)
        {
            CloseFixedUICtrl(lastUI);
        }
    }

    public void Close<T>() where T : BaseUI
    {
        //判断是否是全局 UI
        var globalCtrl = GlobalUIMgr.Instance.Get<T>();
        if (globalCtrl != null)
        {
            GlobalUIMgr.Instance.Close<T>();
            return;
        }

        var findCtrl = FindCtrl<T>();

        if (null == findCtrl)
        {
            return;
        }

        //TODO 检查是否是最上层的 uiCtrl ，并且是 fixed 类型
        if (findCtrl.showMode == CtrlShowMode.Float)
        {
            findCtrl.StartExitAni(() =>
            {
                findCtrl.Inactive();
                findCtrl.Close();
                ctrlCacheList.Remove(findCtrl);
            });
        }
        else if (findCtrl.showMode == CtrlShowMode.Fixed)
        {
            // if (CurrFixedCtrl == findCtrl)
            // {
            //     findCtrl.StartExitAni(() =>
            //     {
            //         findCtrl.Inactive();
            //         findCtrl.Close();
            //         ctrlCacheList.Remove(findCtrl);
            //
            //         ActivePreCtrls();
            //     });
            // }
            // else
            // {
            //     Logx.LogError(LogxType.Game, "findCtrl != ctrl : " + typeof(T) + " != " + CurrFixedCtrl?.GetType());
            // }

            CloseFixedUICtrl(findCtrl);
        }
    }

    public void CloseFixedUICtrl(BaseUI findCtrl)
    {
        findCtrl.StartExitAni(() =>
        {
            findCtrl.Inactive();
            findCtrl.Close();
            ctrlCacheList.Remove(findCtrl);

            ActivePreCtrls();
        });
    }


    //激活之前的 ctrl
    public void ActivePreCtrls()
    {
        for (int i = this.ctrlCacheList.Count - 1; i >= 0; --i)
        {
            var ctrl = this.ctrlCacheList[i];
            if (ctrl.showMode == CtrlShowMode.Float)
            {
                ctrl.Active();
            }
            else if (ctrl.showMode == CtrlShowMode.Fixed)
            {
                ctrl.Active();
                ctrl.StartEnterAni(null);
                break;
            }
        }
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            // if (ctrl.state == CtrlState.Loading)
            // {
            //     if (ctrl.CheckLoadFinish())
            //     {
            //         ctrl.LoadFinish();
            //     }
            // }

            if (ctrl.state == CtrlState.Active)
            {
                ctrl.Update(deltaTime);
            }
        }
        
        GlobalUIMgr.Instance.Update(deltaTime);
    }

    public void LateUpdate(float deltaTime)
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            if (ctrl.state == CtrlState.Active)
            {
                ctrl.LateUpdate(deltaTime);
            }
        }
    }


    public BaseUI FindCtrl<T>() where T : BaseUI
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            if (ctrl.GetType() == typeof(T))
            {
                return ctrl;
            }
        }

        return null;
    }
}