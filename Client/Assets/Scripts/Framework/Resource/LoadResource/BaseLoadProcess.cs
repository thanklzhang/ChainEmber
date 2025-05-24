
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class BaseLoadProcess
{
    //loader 缓存池
    //public List<BaseLoader> loaderCache;

    //准备中的 loader 列表
    public List<BaseLoader> preparingList;
    //等待加载队列
    public List<BaseLoader> waitLoadQueue;
    //正在加载中队列
    public List<BaseLoader> loadingQueue;

    private Action<BaseLoader> loadFinishCallback;

    public int maxLoadingCount = 5;

    public void Init()
    {
        preparingList = new List<BaseLoader>();
        waitLoadQueue = new List<BaseLoader>();
        loadingQueue = new List<BaseLoader>();
    }

    internal void SetFinishCallback(Action<BaseLoader> loadFinishCallback)
    {
        this.loadFinishCallback = loadFinishCallback;
    }

    public void Update(float timeDelta)
    {
        this.UpdateLoad(timeDelta);
    }

    void UpdateLoad(float timeDelta)
    {
        //正在加载队列 并 移除已经加载完成的 loader
        for (int i = loadingQueue.Count - 1; i >= 0; i--)
        {
            var loadingLoader = loadingQueue[i];
            loadingLoader.Update(timeDelta);

            //loader 判断状态也可以
            if (loadingLoader.IsLoadFinish())
            {
                try
                {
                    loadingLoader.LoadFinish();
                    OnLoadFinish(loadingLoader);
                }
                catch (Exception e)
                {
                    Logx.LogError("UpdateLoad error : " + e.Message + " " + e.StackTrace);
                }
                finally
                {
                    loadingQueue.RemoveAt(i);
                }
            }
        }

        //从准备中列表 中 挑出准备完毕的 loader 进入 等待加载队列
        for (int i = preparingList.Count - 1; i >= 0; i--)
        {
            var prepareLoader = preparingList[i];

            if (prepareLoader.IsPrepareFinish())
            {
                prepareLoader.PrepareFinish();
                preparingList.RemoveAt(i);
                waitLoadQueue.Add(prepareLoader);
            }
        }

        //检查正在加载队列有没有空闲位置 有的话从 等待加载队列 中取 loader 填满正在加载队列
        var surplusCount = maxLoadingCount - loadingQueue.Count;
        if (surplusCount > 0)
        {
            for (int i = 0; i < surplusCount; i++)
            {
                if (waitLoadQueue.Count > 0)
                {
                    var waitLoader = waitLoadQueue[0];
                    waitLoadQueue.RemoveAt(0);
                    loadingQueue.Add(waitLoader);
                    
                    //加入正在加载队列 开始加载资源
                    var loader = waitLoader;
                    loader.StartLoad();
                }
                else
                {
                    break;
                }
            }
        }

    }

    //添加一个新的 loader 的时候
    public virtual void AddLoader(BaseLoader loader)
    {
        //Logx.Log("base : process AddLoader");

        preparingList.Add(loader);
        loader.Prepare();
        //loaderCache.Add(loader);
    }

    //当一个 loader 加载完成的时候
    public virtual void OnLoadFinish(BaseLoader loader)
    {
        this.loadFinishCallback?.Invoke(loader);
    }


}
