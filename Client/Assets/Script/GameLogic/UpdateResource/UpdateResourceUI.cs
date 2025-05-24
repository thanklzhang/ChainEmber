using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//资源更新模块
public class UpdateResourceUI
{
    GameObject gameObject;
    Transform transform;

    Text stateText;
    RectTransform progressRootRect;
    RectTransform progressRect;
    Text speedText;
    Text progressText;
    Text errorText;

    UpdateResourceModule updateResModule;

    float prgressWidth;

    Dictionary<UpdateResStateType, string> stateStrDic = new Dictionary<UpdateResStateType, string>()
    {
        {UpdateResStateType.CopyRes,"复制游戏资源中(此过程不消耗流量)" },
        {UpdateResStateType.CheckVersion,"检查资源版本中" },
        {UpdateResStateType.GetNewestResFileList,"获取最新游戏资源中" },
        {UpdateResStateType.DownloadRes,"下载最新游戏资源中" },
        {UpdateResStateType.Finish,"更新完成" },
        {UpdateResStateType.Error,"发生错误" },
    };

    public void Init(GameObject gameObject, UpdateResourceModule updateResModule)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.updateResModule = updateResModule;

        stateText = transform.Find("stateText").GetComponent<Text>();
        progressRect = transform.Find("progressBar/front").GetComponent<RectTransform>();
        progressRootRect = transform.Find("progressBar/back").GetComponent<RectTransform>();
        speedText = transform.Find("speedText").GetComponent<Text>();
        progressText = transform.Find("progressText").GetComponent<Text>();
        errorText = transform.Find("errorText").GetComponent<Text>();

        prgressWidth = progressRootRect.rect.width;

        this.progressRect.sizeDelta = new Vector2(0, this.progressRect.sizeDelta.y);

        this.AddListener();
    }

    public void AddListener()
    {
        updateResModule.event_updateResState += RefreshState;
        updateResModule.event_updateResInfo += RefreshCurrResInfo;
        updateResModule.event_updateDownloadBytes += RefreshDownloadBytes;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //刷新当前的状态
    public void RefreshState(UpdateResStateType state, string errInfo)
    {
        var str = stateStrDic[state];

        speedText.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);

        if (state == UpdateResStateType.CopyRes)
        {
            speedText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(true);

        }
        else if (state == UpdateResStateType.CheckVersion)
        {
            speedText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
        }
        else if (state == UpdateResStateType.GetNewestResFileList)
        {
            speedText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
        }
        else if (state == UpdateResStateType.DownloadRes)
        {
            speedText.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
        }
        else if (state == UpdateResStateType.Finish)
        {
            speedText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
        }
        else if (state == UpdateResStateType.Error)
        {
            errorText.gameObject.SetActive(true);
            speedText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            str += " 原因 : " + errInfo;
            errorText.text = str;
            return;
        }

        stateText.text = str;
    }

    //刷新当前的资源信息
    public void RefreshCurrResInfo(int currCount, int totalCount)
    {
        progressText.text = "资源数量 : " + currCount + "/" + totalCount;

        var progress = currCount / (float)totalCount;

        this.progressRect.sizeDelta = new Vector2(prgressWidth * progress, this.progressRect.sizeDelta.y);
    }

    //刷新当前下载 bytes(每帧)
    public void RefreshDownloadBytes(ulong bytes)
    {
        var mb = bytes / 1024.0f / 1024.0f / Time.deltaTime;
        var mbStr = mb.ToString("F2");
        speedText.text = string.Format("下载速度:{0}MB/s", mbStr);
    }

    public void RemoveListener()
    {
        updateResModule.event_updateResState -= RefreshState;
        updateResModule.event_updateResInfo -= RefreshCurrResInfo;
        updateResModule.event_updateDownloadBytes -= RefreshDownloadBytes;
    }

    public void Release()
    {
        this.RemoveListener();
    }
}
