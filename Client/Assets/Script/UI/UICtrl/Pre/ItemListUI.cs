//using GameModelData;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ItemListUI : BaseUI
//{
//    Button nextBtn;
//    Button closeBtn;
//    protected override void OnInit()
//    {
//        this.name = UIName.ItemListUI;
//        this.resPath = UIName.ItemListUI;
//        this.type = UIType.Normal;
//    }

//    protected override void OnLoadFinish()
//    {
//        nextBtn = this.transform.Find("nextBtn").GetComponent<Button>();
//        nextBtn.onClick.AddListener(() =>
//        {
//            UIManager.Instance.OpenUI(UIName.EquipmentListUI);
//        });

//        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//        closeBtn.onClick.AddListener(() =>
//        {
//            this.ActiveClose();
//        });
//    }

//    protected override void OnOpen()
//    {

//    }
//}
