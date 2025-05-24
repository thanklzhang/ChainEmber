using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全局 ctrl 游戏进程中一直存在 不受正常的 ctrl 管理
public class GlobalUIMgr : Singleton<GlobalUIMgr>
{

    public Dictionary<Type,BaseUI> ctrlDic = new  Dictionary<Type,BaseUI>();
    
    public void Init()
    {
        
    }

    //所有全局 UI
    public IEnumerator LoadReq()
    {
        yield return LoadUIReq<TitleBarUI>();
        yield return LoadUIReq<LoadingUICtrl>();
        yield return LoadUIReq<CommonTipsUI>();
    }

    public IEnumerator LoadUIReq<T>() where T : BaseUI , new ()
    {
        yield return null;

        //标题栏
        T ctrl = new T();
        ctrlDic.Add(ctrl.GetType(),ctrl);
        ctrl.Init();

        GameObject loadGo = null;
        var isFinishLoad = false;

        ctrl.StartLoad((go) =>
        {
            loadGo = go;
            
            UIManager.Instance.SetParent(ctrl,go); 
            isFinishLoad = true;
        });

        while (!isFinishLoad)
        {
            yield return null;
        }

        if (null == loadGo)
        {
            yield break;
        }

        loadGo.gameObject.SetActive(false);
        ctrl.LoadFinish(loadGo);
    }

    public BaseUI Get<T>() where T : BaseUI
    {
        BaseUI ctrl = null;
        var type = typeof(T);
        if (ctrlDic.TryGetValue(type, out ctrl))
        {
            return ctrl;
        }
        return null;
    }

    public void Open<T>(UICtrlArgs args) where T : BaseUI
    {
        var ctrl = Get<T>();
        ctrl?.Open(args);
        ctrl?.Active();
    }

    public void Update(float deltaTime)
    {
        foreach (var ctrl in ctrlDic.Values)
        {
            if (ctrl.state == CtrlState.Active)
            {
                ctrl.Update(deltaTime);
            }
        }
    }

    public void Close<T>()where T : BaseUI
    {
        var ctrl = Get<T>();
        ctrl.Inactive();
        ctrl?.Close();
        
    }

    public void Release()
    {
        
    }
}