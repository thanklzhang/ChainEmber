//using GameModelData;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class EquipmentListUI : BaseUI
//{
//    Button nextBtn;
//    Button closeBtn;
//    protected override void OnInit()
//    {
//        this.name = UIName.EquipmentListUI;
//        this.resPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
//        this.type = UIType.Normal;
//    }

//    protected override void OnLoadFinish()
//    {
//        nextBtn = this.transform.Find("nextBtn").GetComponent<Button>();
//        nextBtn.onClick.AddListener(() =>
//        {
//            UIManager.Instance.OpenUI(UIName.ItemListUI);
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
