using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Object = UnityEngine.Object;

public class AudioManager : Singleton<AudioManager>
{
    public Transform root;
    public GameObject gameObject;

    public AudioSource bgmAduioSource;
    public AudioSource uiAduioSource;

    public void Init(Transform root)
    {
        this.root = root;
        this.gameObject = this.root.gameObject;
        var audioList = this.gameObject.GetComponents<AudioSource>();
        bgmAduioSource = audioList[0];
        uiAduioSource = audioList[1];
    }

    public void PlayBGM(int resId)
    {
        ResourceManager.Instance.GetObject<AudioClip>(resId, (obj) => { OnLoadBGMFinish(resId, obj); });
    }

    public void OnLoadBGMFinish(int resId, Object obj)
    {
        if (null == obj)
        {
            Logx.LogWarning("the audio is null : resId : " + resId);
            return;
        }

        var clip = obj as AudioClip;

        bgmAduioSource.clip = clip;
        bgmAduioSource.loop = true;
        bgmAduioSource.Play();
    }

    public void StopBGM()
    {
        bgmAduioSource.Stop();
    }

    public void PlaySound(int resId)
    {
        ResourceManager.Instance.GetObject<AudioClip>(resId, (obj) => { OnLoadSoundFinish(resId, obj); });
    }
    
    public void OnLoadSoundFinish(int resId, Object obj)
    {
        if (null == obj)
        {
            Logx.LogWarning("the audio is null : resId : " + resId);
            return;
        }

        var clip = obj as AudioClip;

        uiAduioSource.clip = clip;
        uiAduioSource.Play();
    }

}