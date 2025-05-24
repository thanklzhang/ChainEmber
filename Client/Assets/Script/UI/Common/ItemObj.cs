//using GameModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.UI;

//public class ItemObj
//{
//    public Transform root;
//    public Item item;
//    Image icon;
//    Button btn;
//    Image qualityBg;//过后变成 设置 sprite

//    public static ItemObj Create(GameObject rootObj, Item item)
//    {
//        ItemObj obj = new ItemObj();
//        obj.root = rootObj.transform;
//        obj.item = item;
//        obj.Init();

//        return obj;
//    }

//    public void Init()
//    {
//        //icon = root.Find("icon").GetComponent<Image>();
//        //qualityBg = root.Find("qualityBg").GetComponent<Image>();
//        //icon.sprite = GameResource.Instance.GetItemSprite(item.config.icon);
//        //qualityBg.color = Common.GetItemQualityColor((itemQuality)item.quality);
//        //btn = root.GetComponent<Button>();

//    }

//    public void Show()
//    {
//        root.gameObject.SetActive(true);
//    }

//    public void Hide()
//    {
//        root.gameObject.SetActive(false);
//    }

//    public void AddClickListener(Action action)
//    {
//        btn.onClick.RemoveAllListeners();
//        btn.onClick.AddListener(() =>
//        {
//            action?.Invoke();
//        });
//    }
//}

