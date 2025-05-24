using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleHeroInfoShowObj : BaseUIShowObj<BattleHeroInfoUICtrl>
{
    public BattleHeroInfoUIData uiData;

    private Text nameText;
    private Image avatarImg;
    private Text hpShowText;
    private Image progressImg;
    private EntityHpColorSelector colorSelector;

    public override void OnInit()
    {
        nameText = this.transform.Find("name").GetComponent<Text>();
        avatarImg = this.transform.Find("avatarBg/avatar").GetComponent<Image>();
        hpShowText = this.transform.Find("hpMain/valueText").GetComponent<Text>();
        progressImg = this.transform.Find("hpMain/bg/hpFill/hp").GetComponent<Image>();
        colorSelector = this.transform.Find("hpMain/bg/hpFill/hp").GetComponent<EntityHpColorSelector>();
    }

    public override void OnRefresh(object data, int index)
    {

        if (data != null)
        {
            this.uiData = (BattleHeroInfoUIData)data;

        }

        RefreshHeroInfo();
    }

    internal void UpdateInfo(BattleHeroInfoUIData uiData)
    {
        this.uiData.currHealth = uiData.currHealth;
        this.uiData.maxHealth = uiData.maxHealth;

        this.RefreshHeroInfo();
    }

    void RefreshHeroInfo()
    {
        if (this.uiData != null)
        {
            var heroConfig = ConfigManager.Instance.GetById<EntityInfo>(this.uiData.heroConfigId);
            this.nameText.text = heroConfig.Name;
            var progress = 0.0f;
            if (this.uiData.maxHealth != 0)
            {
                progress = this.uiData.currHealth / this.uiData.maxHealth;
            }

            this.progressImg.fillAmount = progress;
            var currHpStr = (int)this.uiData.currHealth;
            var maxHpStr = (int)this.uiData.maxHealth;
            this.hpShowText.text = string.Format("{0}/{1}", currHpStr,maxHpStr);

            var avatarResId = heroConfig.AvatarResId;
            //TODO：注意 这里不正确卸载可能会造成引用计数不对而无法正确卸载资源
            ResourceManager.Instance.GetObject<Sprite>(avatarResId, (sprite) => { this.avatarImg.sprite = sprite; });
          
            
            //血条
            var selfPlayerIndex = BattleManager.Instance.GetLocalPlayer().playerIndex;
            var currEntityPlayerIndex = uiData.playerIndex;
            bool isSelf = selfPlayerIndex == currEntityPlayerIndex;
            bool isEnemy = currEntityPlayerIndex < 0;
            EntityRelationType relationType = EntityRelationType.Friend;
            if (isSelf)
            {
                relationType = EntityRelationType.Self;
            }
            else if (isEnemy)
            {
                relationType = EntityRelationType.Enemy;
            }
            else
            {
                relationType = EntityRelationType.Friend;
            }
          
            if (relationType == EntityRelationType.Self)
            {
                // hpBg.color = this.colorSelector.selfBgColor;
                // valueText.color = this.colorSelector.selfTextColor;
        
                progressImg.sprite = this.colorSelector.selfSprite;
        
            }
            else if (relationType == EntityRelationType.Friend)
            {
                // hpBg.color = this.colorSelector.enemyBgColor;
                // valueText.color = this.colorSelector.enemyTextColor;
            
                progressImg.sprite = this.colorSelector.friendSprite;
            }
            else if (relationType == EntityRelationType.Enemy)
            {
                // hpBg.color = this.colorSelector.friendBgColor;
                // valueText.color = this.colorSelector.friendTextColor;
                progressImg.sprite = this.colorSelector.enemySprite;
            }
            
            
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }


    }

    public void Update(float deltaTime)
    {
    }

    internal int GetHeroGuid()
    {
        return this.uiData.heroGuid;
    }

    public override void OnRelease()
    {
    }
}



public class BattleHeroInfoUIData
{
    public int playerIndex;
    public int heroGuid;
    public int heroConfigId;
    public int level;
    public float currHealth;
    public float maxHealth;
}