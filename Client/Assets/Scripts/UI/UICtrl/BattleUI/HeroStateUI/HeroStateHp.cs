

using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class HeroStateHp
{
    GameObject gameObject;
    Transform transform;

    public Transform bgRoot;
    public Transform hp;
    public Text valueText;

    HeroStateUIMgr stateUIMgr;
    RectTransform parentTranRect;
    EntityHpColorSelector colorSelector;
    
    public Image hpBg;
    
    Color normalDamageColor = new Color(1, 0.744f, 0.108f);
    Color addHpColor = new Color(0, 1, 0, 1);

    //实体信息
    private float preCurrHp;
    private float nowCurrHp;
    private GameObject entityObj;
    private EntityRelationType relationType;
    private int fromEntityGuid;
    private float maxHp;

    public void Init(GameObject gameObject,BattleEntity_Client entity, HeroStateUIMgr stateUIMgr)
    {
        this.gameObject = gameObject;
        this.stateUIMgr = stateUIMgr;
        parentTranRect = stateUIMgr.transform.GetComponent<RectTransform>();
        this.transform = this.gameObject.transform;

        bgRoot = this.transform.Find("bg");
        hp = this.transform.Find("bg/hpFill/hp");
        hpBg = hp.GetComponent<Image>();
        valueText = this.transform.Find("valueText").GetComponent<Text>();
        colorSelector = hp.GetComponent<EntityHpColorSelector>();

        preCurrHp = entity.CurrHealth;

    }

    public void Refresh(BattleEntity_Client entity,int fromEntityGuid)
    {
        RefreshData(entity,fromEntityGuid);

        RefreshShow();
    }

    //刷新数据
    public void RefreshData(BattleEntity_Client entity,int fromEntityGuid)
    {
        //preCurrHp = entity.CurrHealth;
        nowCurrHp = entity.CurrHealth;
        entityObj = entity.gameObject;
        maxHp = entity.MaxHealth;
        this.fromEntityGuid = fromEntityGuid;

        var player = BattleManager.Instance.GetLocalPlayer();
        var relationType = player.GetRelationFromEntity(entity);
        
        this.relationType = relationType;
    }

    //刷新显示
    public void RefreshShow()
    {
        var currHp = this.nowCurrHp;
        var maxHp = this.maxHp;

        if (0 == maxHp)
        {
            maxHp = 1;
        }

        var ratio = currHp / maxHp;

        if (preCurrHp >= 0)
        {
            //飘字
            var changeHp = currHp - preCurrHp;

            var word = "" + changeHp;
            if (changeHp > 0)
            {
                word = "+" + changeHp;
            }
            var go = this.entityObj;
            FloatWordShowStyle floatStyle = FloatWordShowStyle.Left;
            var fromEntityGuid = this.fromEntityGuid;
            if (fromEntityGuid > 0)
            {
                var currEntityPos = go.transform.position;
                var fromEntity = BattleEntityManager.Instance.FindEntity(this.fromEntityGuid);
                if (fromEntity != null)
                {
                    var fromEntityPos = fromEntity.gameObject.transform.position;
                    var dir = (fromEntityPos - currEntityPos).normalized;
                    if (dir.x > 0)
                    {
                        floatStyle = FloatWordShowStyle.Left;
                    }
                    else
                    {
                        floatStyle = FloatWordShowStyle.Right;
                    }
                }

                Color color = normalDamageColor;
                if (changeHp > 0)
                {
                    color = addHpColor;
                }

                FloatWordBean bean = new FloatWordBean();
                bean.wordStr = word;
                bean.followGo = go;
                bean.color = color;
                bean.showStyle = floatStyle;
                bean.stateType = changeHp > 0 ? EntityAbnormalStateType.CurrHp_Add
                        : EntityAbnormalStateType.CurrHp_Sub;
                bean.triggerType = AbnormalStateTriggerType.Start;
                
                stateUIMgr.ShowFloatWord(bean);
            }
        }

        preCurrHp = nowCurrHp;

        var width = bgRoot.GetComponent<RectTransform>().rect.width;
        var currLen = width * ratio;

        hpBg.fillAmount = ratio;
        valueText.text = "" + currHp + "/" + maxHp;

        //背景颜色和字体颜色
        var relationType = this.relationType;

        if (relationType == EntityRelationType.Self)
        {
            hpBg.sprite = this.colorSelector.selfSprite;
        
        }
        else if (relationType == EntityRelationType.Friend)
        {
            hpBg.sprite = this.colorSelector.friendSprite;
        }
        else if (relationType == EntityRelationType.Enemy)
        {
            hpBg.sprite = this.colorSelector.enemySprite;
        }
    }

    public void Update(float timeDelta)
    {
     
    }

}

