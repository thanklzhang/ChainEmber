using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    //IEntity entity;

    //public Text currHpText;
    //public Text maxHpText;
    //public Image currHpImg;

    //public void Init(IEntity entity)
    //{
    //    this.entity = entity;
    //    maxHpText.text = "" + entity.info.MaxHp;
    //    //maxHpText.text = "" + entity.transform.position.x + " " + entity.transform.position.z;
    //    currHpText.text = "" + entity.info.CurrHp;
    //    currHpImg.fillAmount = entity.info.CurrHp / entity.info.MaxHp;

    //    //var localHero = CombatManager.Instance.GetLocalHero();

    //    //设置血的颜色
    //    //currHpImg.color = entity.team == localHero.team ? Color.green : Color.red;


    //    //各种事件

    //    entity.info.UpdateMaxHp += (maxHp) =>
    //    {
    //        maxHpText.text = "" + entity.info.MaxHp;
    //        //maxHpText.text = "" + entity.transform.position.x + " " + entity.transform.position.z;
    //        currHpImg.fillAmount = entity.info.CurrHp / entity.info.MaxHp;
    //    };

    //    entity.info.UpdateCurrHp += (hp) =>
    //    {
    //        currHpText.text = "" + entity.info.CurrHp;
    //        currHpImg.fillAmount = entity.info.CurrHp / entity.info.MaxHp;
    //    };

    //    UpdateInfo();
    //}

    //void UpdateInfo()
    //{
    //    var sPos = Camera.main.WorldToScreenPoint(entity.transform.position);
    //    this.GetComponent<RectTransform>().position = sPos + Vector3.up * 25.0f;
    //}


    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (entity != null)
    //    {
    //        UpdateInfo();
    //    }
    //    else
    //    {
    //        //之后用事件  并且删除用管理器删除
    //        Destroy(this.gameObject);
    //    }


    //    //will delete
    //    //maxHpText.text = "" + entity.transform.position.x + " " + entity.transform.position.z;
    //}
}
