using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;

using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client.BattleSkillOperate
{
    //技能书道具购买部分
    public class SkillItemShopPanel
    {
        public GameObject gameObject;
        public Transform transform;

      
        private List<SkillItemShopCell> shopCellList;

        public void Init(GameObject gameObject, BattleUI battleUIPre)
        {
            this.gameObject = gameObject;
            this.transform = this.gameObject.transform;
            
          
        }

        public void OnClickOne(SkillItemShopCell shopItemCell)
        {
            //shopItemCell . data
            
            //send net
        }

        public void UpdateShopItem(int shopItemGuid)
        {
            //find by guid

            SkillItemShopCell cell = null;
            cell.RefreshItem(0);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void RefreshAllUI()
        {
            //fill shopCellList and show
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