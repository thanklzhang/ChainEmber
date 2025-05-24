using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class HeroStateName
{
    GameObject gameObject;
    Transform transform;

    public TextMeshProUGUI valueText;

    HeroStateUIMgr stateUIMgr;

    RectTransform parentTranRect;

    //实体信息
    private GameObject entityObj;
    private int fromEntityGuid;

    public void Init(GameObject gameObject, BattleEntity_Client entity, HeroStateUIMgr stateUIMgr)
    {
        this.gameObject = gameObject;
        this.stateUIMgr = stateUIMgr;
        parentTranRect = stateUIMgr.transform.GetComponent<RectTransform>();
        this.transform = this.gameObject.transform;

        valueText = this.transform.Find("valueText").GetComponent<TextMeshProUGUI>();

        var entityConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(entity.configId);
        valueText.text = entityConfig != null ? entityConfig.Name : entity.configId.ToString();

        this.valueText.gameObject.SetActive(true);
    }

    public void Refresh(BattleEntity_Client entity, int fromEntityGuid)
    {
        // var entityConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(entity.configId);
        // valueText.name = entityConfig != null ? entityConfig.Name : entity.configId.ToString();

        // RefreshData(entity,fromEntityGuid);
        //
        // RefreshShow();

        var player = BattleManager.Instance.GetLocalPlayer();
        var relationType = player.GetRelationFromEntity(entity);


        if (relationType == EntityRelationType.Enemy)
        {
            if (this.valueText.gameObject.activeSelf)
            {
                this.valueText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!this.valueText.gameObject.activeSelf)
            {
                this.valueText.gameObject.SetActive(true);
            }
        }
    }

    //刷新数据
    public void RefreshData(BattleEntity_Client entity, int fromEntityGuid)
    {
    }

    //刷新显示
    public void RefreshShow()
    {
    }

    public void Update(float timeDelta)
    {
    }
}