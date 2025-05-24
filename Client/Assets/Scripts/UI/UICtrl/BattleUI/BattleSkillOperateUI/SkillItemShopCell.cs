using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;

using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client.BattleSkillOperate
{
    //技能书道具购买格子
    public class SkillItemShopCell
    {
        public GameObject gameObject;
        public Transform transform;
        
        private Button buyBtn;
        
        //data
        
        public void Init(GameObject gameObject, BattleUI battleUIPre)
        {
            this.gameObject = gameObject;
            this.transform = this.gameObject.transform;

            buyBtn.onClick.AddListener(OnClickBuyBtn);
        }
   
        public void OnClickBuyBtn()
        {
            //parent . OnClickOne
        }

        public void RefreshItem(int info)
        {
            //refresh data
            
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
