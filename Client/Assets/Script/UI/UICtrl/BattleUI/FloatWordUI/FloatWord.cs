using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class FloatWord
{
    public GameObject gameObject;
    Transform transform;
    public Text valueText;
    public GameObject followGo;
    RectTransform parentRoot;
    bool isUsing;

    public bool IsUsing
    {
        get => isUsing;
    }

    float timer;

    Animator ani;
    private float aniLength;

    public void Init(GameObject go, Transform parentRoot)
    {
        this.parentRoot = parentRoot.GetComponent<RectTransform>();
        this.gameObject = go;
        this.transform = this.gameObject.transform;

        this.valueText = transform.Find("root/text/value_text").GetComponent<Text>();

        ani = transform.GetComponentInChildren<Animator>();
    }

    // internal void Start(string word, GameObject followGo, AbnormalStateBean stateBean, Color color)
    internal void Start(FloatWordBean bean)
    {
        this.isUsing = true;
        this.followGo = bean.followGo;
        var word = bean.wordStr;
        SetShowValue(word);

        this.gameObject.SetActive(true);

        AnimationClip clip = null;
        string stateName = "";
        var stateType = bean.stateType;
        var trigger = bean.triggerType;
        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
        if (stateType == EntityAbnormalStateType.CurrHp_Add ||
            stateType == EntityAbnormalStateType.CurrHp_Sub)
        {
            if (trigger == AbnormalStateTriggerType.Start)
            {
                if (bean.showStyle == FloatWordShowStyle.Left)
                {
                    //向左
                    stateName = "float_word_left";
                }
                else if (bean.showStyle == FloatWordShowStyle.Right)
                {
                    stateName = "float_word_right";
                }
            }

          
        }
        else if (stateType == EntityAbnormalStateType.AvoidNormalAttack)
        {
            if (trigger == AbnormalStateTriggerType.Trigger)
            {
                stateName = "float_word_middle";
            }
        }
        else if (stateType == EntityAbnormalStateType.AvoidProjectile)
        {
            if (trigger == AbnormalStateTriggerType.Trigger)
            {
                stateName = "float_word_middle";
            }
        }


        clip = FindClip(stateName);

        this.aniLength = clip.length;

        ani.Play(stateName);

        this.transform.SetAsLastSibling();

        this.valueText.color = bean.color;
    }

    public AnimationClip FindClip(string stateName)
    {
        var clips = ani.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            if (clip.name.Contains(stateName))
            {
                return clip;
            }
        }

        return null;
    }

    public void SetShowValue(string word)
    {
        valueText.text = word;
    }

    public void Update(float deltaTime)
    {
        if (null == this.followGo)
        {
            //这里应该和 entity 同步
            return;
        }

        var entityObj = this.followGo;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRoot, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 80;


        timer += deltaTime;
        if (timer >= this.aniLength)
        {
            this.Finish();
        }
    }

    public void Finish()
    {
        timer = 0;
        this.gameObject.SetActive(false);
        this.isUsing = false;
    }

    public void Release()
    {
        Finish();
    }
}