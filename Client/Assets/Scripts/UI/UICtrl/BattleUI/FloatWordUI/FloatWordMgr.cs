using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public enum FloatWordShowStyle
{
    Left = 0,
    Middle = 1,
    Right = 2
}

public class FloatWordBean
{
    public string wordStr;
    public FloatWordShowStyle showStyle;
    public GameObject followGo;
    public EntityAbnormalStateType stateType;
    public AbnormalStateTriggerType triggerType;
    public Color color;
}

public class FloatWordMgr
{
    public Transform root;
    public GameObject floatWordPrefab;
    int initInstanceCount = 10;
    public Dictionary<int, FloatWord> floatWordDic;
    public void Init(Transform root)
    {
        this.root = root;
        this.floatWordPrefab = root.Find("word").gameObject;

        floatWordPrefab.SetActive(false);

        floatWordDic = new Dictionary<int, FloatWord>();

        for (int i = 0; i < initInstanceCount; i++)
        {
            AddNew();
        }
    }

    FloatWord AddNew()
    {
        GameObject newGo = GameObject.Instantiate(floatWordPrefab, root, false);
        
        FloatWord fw = new FloatWord();
        fw.Init(newGo, this.root);

        floatWordDic.Add(newGo.GetInstanceID(), fw);

        return fw;
    }

    // public void ShowFloatWord(string word, GameObject followGo, AbnormalStateBean stateBean, Color color)
    public void ShowFloatWord(FloatWordBean bean)
    {
        var fw = FindCanUse();
        if (null == fw)
        {
            fw = AddNew();
        }

        fw.Start(bean);
    }

    public FloatWord FindCanUse()
    {
        foreach (var item in floatWordDic)
        {
            if (!item.Value.IsUsing)
            {
                return item.Value;
            }
        }

        return null;
    }

    public void Update(float deltaTime)
    {
        foreach (var item in floatWordDic)
        {
            if (item.Value.IsUsing)
            {
                item.Value.Update(deltaTime);
            }
        }
    }

    public void Release()
    {
        foreach (var item in floatWordDic)
        {
            if (item.Value.IsUsing)
            {
                item.Value.Release();
            }
        }
    }
}