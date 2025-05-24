using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Config;
using ServerSimulation.Services;

//using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LoginSceneCtrl : BaseSceneCtrl
{
    public override void Init()
    {
        sceneName = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>((int)ResIds.LoginScene).Name;
    }

    public override void StartLoad(Action action = null)
    {
        //open loading UI
        CoroutineManager.Instance.StartCoroutine(_StartLoad());
    }

    public IEnumerator _StartLoad()
    {
        GameMain.Instance.CloseInitLoadingUI();
        
        UIManager.Instance.Open<LoadingUICtrl>();
        
        EventSender.SendLoadingProgress(0 / 1.0f,"加载场景中...");

        //加载场景
        yield return SceneLoadManager.Instance.LoadRequest(sceneName);
        
        EventSender.SendLoadingProgress(0.4f / 1.0f,"加载界面中...");
        
        
        //加载 UI 并打开
        yield return UIManager.Instance.EnterRequest<LoginUI>();
        
        EventSender.SendLoadingProgress(1.0f / 1.0f,"加载完成");
        
        this.LoadFinish();
        
    }

    public void LoadFinish()
    {
        //close loading UI
        UIManager.Instance.Close<LoadingUICtrl>();
    }

    /// <summary>
    /// 处理登录逻辑
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="callback">登录结果回调</param>
    public static void HandleLogin(string username, string password, Action<bool, string> callback)
    {
        // 使用LoginService进行登录
        LoginService.Instance.Login(username, password, (success, userId, errorMessage) =>
        {
            if (success)
            {
                // 登录成功，回调
                callback?.Invoke(true, null);
            }
            else
            {
                // 登录失败，回调错误信息
                callback?.Invoke(false, errorMessage);
            }
        });
    }

    /// <summary>
    /// 处理注册逻辑
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="callback">注册结果回调</param>
    public static void HandleRegister(string username, string password, Action<bool, string> callback)
    {
        // 使用LoginService进行注册
        LoginService.Instance.Register(username, password, (success, userId, errorMessage) =>
        {
            if (success)
            {
                // 注册成功，回调
                callback?.Invoke(true, null);
            }
            else
            {
                // 注册失败，回调错误信息
                callback?.Invoke(false, errorMessage);
            }
        });
    }

    // public override void Exit(Action action)
    // {
    //     SceneLoadManager.Instance.Unload(sceneName,() =>
    //     {
    //         base.Exit(action);
    //     });
    // }
}