using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIButton : Button
{
    [SerializeField]
    public int clickAudioResId;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.onClick.AddListener(OnClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        if (clickAudioResId > 0)
        {
            AudioManager.Instance.PlaySound(clickAudioResId);
        }
    }
}