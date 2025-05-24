using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
//using Unity.Services.Core;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class SceneCtrlManager : Singleton<SceneCtrlManager>
{
    private List<BaseSceneCtrl> sceneCtrlsList;

    public void Init()
    {
        sceneCtrlsList = new List<BaseSceneCtrl>();
    }

    private BaseSceneCtrl _lastCtrl;
    private BaseSceneCtrl _currCtrl;
    
    public void Enter<T>() where T : BaseSceneCtrl, new()
    {
        var findCtrl = Find<T>();

        if (null == findCtrl)
        {
            //新场景 
            T newSceneCtrl = new T();
            sceneCtrlsList.Add(newSceneCtrl);
            newSceneCtrl.Init();

            _lastCtrl = null;
            if (sceneCtrlsList.Count - 2 >= 0)
            {
                _lastCtrl = sceneCtrlsList[^2];
            }

            _currCtrl = newSceneCtrl;
            newSceneCtrl.StartLoad(() =>
            {
                if (_lastCtrl != null)
                { 
                    _lastCtrl.Exit(() =>
                    {
                        newSceneCtrl.Enter();
                    });
                }
                else
                {
                    newSceneCtrl.Enter();
                }
            });
            
            // if (_lastCtrl != null)
            // { 
            //     newSceneCtrl.StartLoad(() =>
            //     {
            //         _lastCtrl.Exit(() =>
            //         {
            //             newSceneCtrl.Enter();
            //         });
            //     });
            //     
            //
            // }
            // else
            // {
            //     newSceneCtrl.StartLoad(() =>
            //     {
            //         newSceneCtrl.Enter();
            //     });
            //  
            // }
            //
           
        
        }
        else
        {
            var lastCtrl = sceneCtrlsList[^1];
            
            //已经存在的场景 把这个场景提到最前
            sceneCtrlsList.Remove(findCtrl);
            sceneCtrlsList.Add(findCtrl);
            
            // findCtrl.StartLoad(() =>
            // {
            //     lastCtrl.Exit(() =>
            //     {
            //         findCtrl.Enter();
            //     });
            // });
            
            findCtrl.StartLoad(() =>
            {
                lastCtrl.Exit(() =>
                {
                    findCtrl.Enter();
                });
            });
           
        }
    }

    public void Exit<T>() where T : BaseSceneCtrl, new()
    {
        if (0 == this.sceneCtrlsList.Count)
        {
            Logx.LogWarning(LogxType.SceneCtrl, "0 == this.sceneCtrlsList.Count");
            return;
        }

        var lastCtrl = this.sceneCtrlsList[^1];
        var lastType = lastCtrl.GetType();
        var currType = typeof(T);
        if (lastType != currType)
        {
            Logx.LogWarning(LogxType.SceneCtrl, "lastCtrl.GetType() != typeof(T) : "
                                                + lastType + " != " + currType);
            return;
        }

        //当前失效并退出
        lastCtrl.Exit(() =>
        {
            //上一个激活
            if (this.sceneCtrlsList.Count > 0)
            {
                lastCtrl = this.sceneCtrlsList[^1];
                lastCtrl.StartLoad();
                lastCtrl.Enter();
            }
        });
    }

    private BaseSceneCtrl Find<T>() where T : BaseSceneCtrl
    {
        foreach (var scene in sceneCtrlsList)
        {
            if (typeof(T) == scene.GetType())
            {
                return scene;
            }
        }

        return null;
    }
}