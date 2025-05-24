using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

public enum UIShowLayer
{
    Floor_0 = 0,
    Floor_1 = 1,
    Floor_2 = 2,

    Middle_0 = 10,
    Middle_1 = 11,
    Middle_2 = 12,

    Top_0 = 20,
    Top_1 = 21,
    Top_2 = 22,
}

public abstract class BaseUI
{
    public GameObject gameObject;
    public Transform transform;

    public Action enterAniFinish;
    public Action exitAniFinish;

    public CtrlState state;
    public CtrlShowMode showMode;

    public UICtrlArgs args;

    public int uiResId;
    public UIShowLayer uiShowLayer;
 

    //设置初始化信息
    public void Init()
    {
        this.state = CtrlState.Inited;
        Logx.Log(LogxType.Ctrl, " init : ctrl : " + this.GetType());

        uiResId = (int)ResIds.BattleUI;
        // ui = CreateUIInstance();
        // ui.Init(this);
        this.OnInit();
    }

    // //设置 ctrl 和 ui 的对应关系 : xxxCtrl -> xxxUI
    // public BaseUI CreateUIInstance()
    // {
    //     var ctrlName = this.GetType().Name;
    //     var index = ctrlName.IndexOf("Ctrl", StringComparison.Ordinal);
    //      var uiName = ctrlName.Substring(0, index) + "UI";
    //      var uiType = Type.GetType(uiName);
    //      if (null == uiType)
    //      {
    //          Logx.LogError(LogxType.Ctrl,"the ui type is not found ： " + uiName);
    //          return null;
    //      }
    //
    //      Logx.Log(LogxType.Ctrl, " CreateUIInstance : uiName : " + uiName);
    //      ui = Activator.CreateInstance(uiType) as BaseUI;
    //      return ui;
    // }

    //开始加载
    public void StartLoad(Action<GameObject> action)
    {
        this.laodFinishAction = action;
        this.state = CtrlState.Loading;

        Logx.Log(LogxType.UI, " StartLoad : ctrl : " + this.GetType());

        //TODO:不光是 UI , 有可能有自定义的资源相关
        ResourceManager.Instance.GetObject<GameObject>(this.uiResId, LoadFinish);

        this.OnStartLoad();
    }

    public Action<GameObject> laodFinishAction;


    //加载资源完毕
    public void LoadFinish(GameObject uiGameObject)
    {
        Logx.Log(LogxType.Ctrl, " LoadFinish : ctrl : " + this.GetType());

        this.gameObject = uiGameObject;
        this.transform = this.gameObject.transform;

        // ui.LoadFinish(uiGameObject);
        
        this.OnLoadFinish();
        
        laodFinishAction?.Invoke(this.gameObject);

       
    }

    //打开
    public void Open(UICtrlArgs args)
    {
        Logx.Log(LogxType.Ctrl, " Enter : ctrl : " + this.GetType());

        this.args = args;

        // ui.Enter();
        this.OnOpen(args);
    }

    //激活
    public void Active()
    {
        Logx.Log(LogxType.Ctrl, " Active : ctrl : " + this.GetType());

        if (this.state == CtrlState.Active)
        {
            this.Refresh();
            return;
        }

        this.state = CtrlState.Active;

        this.gameObject.SetActive(true);
        // ui.Active();
        this.OnActive();
    }

    //进入界面的动效 开始播放
    public void StartEnterAni(Action action)
    {
        Logx.Log(LogxType.Ctrl, " StartEnterAni : ctrl : " + this.GetType());

        //TODO: 动效的时候有个不能触碰的遮罩
        this.state = CtrlState.EnterAni;
        enterAniFinish = action;

        //TODO: 接入动效后改成动效结束后调用
        this.EnterAniFinish();
    }

    //进入界面的动效 播放完毕
    public void EnterAniFinish()
    {
        Logx.Log(LogxType.Ctrl, " EnterAniFinish : ctrl : " + this.GetType());

        this.state = CtrlState.Active;
        enterAniFinish?.Invoke();
    }

    //刷新整体界面
    public void Refresh()
    {
        Logx.Log(LogxType.Ctrl, " Refresh : ctrl : " + this.GetType());

        this.OnRefresh();
    }

    public void Update(float deltaTime)
    {
        this.OnUpdate(deltaTime);
    }

    public void LateUpdate(float deltaTime)
    {
        this.OnLateUpdate(deltaTime);
    }

    //退出界面的动效 开始播放
    public void StartExitAni(Action action)
    {
        Logx.Log(LogxType.Ctrl, " StartExitAni : ctrl : " + this.GetType());

        //TODO: 动效的时候有个不能触碰的遮罩
        this.state = CtrlState.ExitAni;
        this.exitAniFinish = action;

        //TODO: 接入动效后改成动效结束后调用
        this.ExitAniFinish();
    }

    //退出界面的动效 播放完毕
    public void ExitAniFinish()
    {
        Logx.Log(LogxType.Ctrl, " ExitAniFinish : ctrl : " + this.GetType());

        this.state = CtrlState.Inactive;
        this.exitAniFinish?.Invoke();
    }

    //失效
    public void Inactive()
    {
        Logx.Log(LogxType.Ctrl, " Inactive : ctrl : " + this.GetType());

        this.state = CtrlState.Inactive;
        this.gameObject.SetActive(false);
        // ui.Inactive();
        this.OnInactive();
    }


    //关闭
    public void Close()
    {
        Logx.Log(LogxType.Ctrl, " Exit : ctrl : " + this.GetType());

        this.state = CtrlState.Exit;

        // ui.Exit();

        ResourceManager.Instance.ReturnObject(this.uiResId, this.gameObject);
        this.gameObject = null;

        this.OnClose();
    }

    //拓展-------------------------
 
    //--------------------------
    protected virtual void OnInit()
    {
    }

    protected virtual void OnStartLoad()
    {
    }

    protected virtual void OnLoadFinish()
    {
    }

    protected virtual void OnOpen(UICtrlArgs args)
    {
    }

    protected virtual void OnActive()
    {
    }

    protected virtual void OnRefresh()
    {
    }

    protected virtual void OnUpdate(float deltaTime)
    {
    }

    protected virtual void OnLateUpdate(float deltaTime)
    {
    }

    protected virtual void OnInactive()
    {
    }

    protected virtual void OnClose()
    {
    }
}