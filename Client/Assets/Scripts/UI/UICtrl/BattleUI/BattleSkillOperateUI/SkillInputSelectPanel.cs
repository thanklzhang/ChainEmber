using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;

using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client.BattleSkillOperate
{
    public class SkillInputSelectCell
    {
        public int skillConfigId;
        public GameObject gameObject;
        public Transform transform;

        //是否选中
        public bool isSelect;

        public void Init()
        {
            //find
        }

        public void Refresh()
        {
            //show
        }

    }

    //技能选择部分
    public class SkillInputSelectPanel
    {
        public GameObject gameObject;
        public Transform transform;

        private List<SkillInputSelectCell> selectItemList;

        private SkillInputSelectCell currSelect;
        public void Init(GameObject gameObject, BattleUI battleUIPre)
        {
            this.gameObject = gameObject;
            this.transform = this.gameObject.transform;


            selectItemList = new List<SkillInputSelectCell>();
        }

        //设置当前选择技能 ， index 0:w技能 1:e技能
        public void SetSelect(int index)
        {
            //selectItemList . Find
            currSelect = selectItemList[index];
            RefreshAllUI();
        }

        public SkillInputSelectCell GetCurrSelect()
        {
            return currSelect;
        }

        //更新技能设置
        public void UpdateSkill(int index,int skillConfigId)
        {
            //selectItemList . Find
            RefreshAllUI();
        }
        

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void RefreshAllUI()
        {
            
        }

        public void Update(float deltaTime)
        {
       


        }

        
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Release()
        {
        
        
        }

    }

}
