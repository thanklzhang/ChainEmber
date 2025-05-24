using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class BaseSkillDirector
{
    public GameObject gameObject;
    public Transform transform;

    protected GameObject followEntity;
    protected Vector3 mousePositionOnGround;

    protected int resourceId;

    bool isEnable;
    bool isFinishLoad;

    //不考虑是否加载完成中的'是否显示'变量 , 这里应该是 state(show hide release  这样就可以根据是否加载完进行一些操作)
    private bool isShow = false;
    protected Config.Skill skillConfig;
    public void Init(int skillDirectorType, List<int> param,Config.Skill skillConfig)
    {
        this.skillConfig = skillConfig;
        this.OnInit(skillDirectorType, param);
    }

    public virtual void OnInit(int skillDirectorType, List<int> param)
    {

    }

    public void Show(GameObject followGameObject)
    {
        isShow = true;
        if (isFinishLoad)
        {
            this.OnShow();

            isEnable = true;

            gameObject.SetActive(true);
        }
        else
        {
            this.followEntity = followGameObject;
            StartLoad();
        }
    }

    public virtual void OnShow()
    {

    }

    public void StartLoad()
    {
        isFinishLoad = false;
        if (this.resourceId > 0)
        {
            ResourceManager.Instance.GetObject<GameObject>(this.resourceId, OnLoadFinish);
        }
    }

    public bool IsEnable()
    {
        return this.isEnable;
    }

    public void OnLoadFinish(GameObject go)
    {
     
        isFinishLoad = true;
        
        this.gameObject = go;

        this.transform = this.gameObject.transform;

        this.transform.position = followEntity.transform.position;
        
        if (isShow)
        {
            this.Show(this.followEntity);
        }
        else
        {
            //这里应该判断是否是已释放状态
        }

    }



    internal void UpdateMousePosition(Vector3 resultPos)
    {
        if (!this.isEnable)
        {
            return;
        }
        mousePositionOnGround = resultPos;
    }

    public void Update(float deltaTime)
    {
        if (!this.isEnable)
        {
            return;
        }
        this.OnUpdate(deltaTime);

    }

    public virtual void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        ////跟随鼠标
        //var mousePosition = mousePositionOnGround;
        //Vector3 toMouseDir = mousePosition - this.transform.position;
        //toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
        //this.transform.forward = toMouseDir;
    }

    public virtual void Hide()
    {
        isShow = false;
        isEnable = false;
        if (this.resourceId > 0)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Release()
    {
        if (this.resourceId > 0)
        {
            //这里其实也要判断 gameObject
            ResourceManager.Instance.ReturnObject(this.resourceId, this.gameObject);
        }
    }

}
