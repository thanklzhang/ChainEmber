//using Assets.Script.Combat;
//using FixedPointy;
//using GameModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.UI;

//public class HeroInfoObj
//{
//    public Transform root;
//    public CombatViewEntity viewEntity;
//    Transform hpBg;
//    Image hpImg;


//    public static HeroInfoObj Create(GameObject rootObj, CombatViewEntity data)
//    {
//        HeroInfoObj obj = new HeroInfoObj();
//        obj.root = rootObj.transform;
//        obj.viewEntity = data;
//        obj.Init();

//        return obj;
//    }

//    public void Init() 
//    {
//        hpBg = root.Find("hpBg");
//        hpImg = hpBg.Find("hp").GetComponent<Image>();

//        //listen

//        //viewEntity.entity.onChangeHp += ChangeHp;

//        viewEntity.entity.onChangeAttribute += ChangeAttribute;

//        hpImg.fillAmount = (float)(viewEntity.entity.GetAttribute(EffectAttributeType.CurrHp) / viewEntity.entity.GetAttribute(EffectAttributeType.MaxHp));
//    }

//    private void ChangeAttribute(EffectAttributeType type, Fix value)
//    {
//        if (type == EffectAttributeType.CurrHp)
//        {
//            hpImg.fillAmount = (float)(value / viewEntity.entity.GetAttribute(EffectAttributeType.MaxHp));
//        }
//    }

//    //void ChangeHp(Fix hp)
//    //{
//    //    hpImg.fillAmount = (float)(hp / viewEntity.entity.maxHp);
//    //}

//    public void Show()
//    {
//        root.gameObject.SetActive(true);
//    }

//    public void Hide()
//    {
//        root.gameObject.SetActive(false);
//    }

//    public void Update(float timeDelta)
//    {
//        //Debug.Log(timeDelta);
//        var screenPos = Camera.main.WorldToScreenPoint(viewEntity.entityObj.transform.position);
//        //Debug.Log("update : " + screenPos);
//        //Debug.Log(viewEntity.entityObj.transform.position + " " + screenPos + " ");
//        //this.root.localPosition = screenPos;// +Vector3.up * 60.0f
//        this.root.position = screenPos + Vector3.up * 100.0f;
//    }

//}

