// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class TitleBarUIPre : BaseUI_pre
// {
//     public Transform optionRoot;
//     public Button closeBtn;
//     public Text nameText;
//     public GameObject bgGo;
//     public GameObject lineGo;
//
//     List<TitleOptionUIData> optionDataList = new List<TitleOptionUIData>();
//     List<TitleOptionShowObj> optionShowList = new List<TitleOptionShowObj>();
//
//     public Action clickCloseBtnAction;
//
//     protected override void OnInit()
//     {
//         this.optionRoot = this.transform.Find("root");
//         this.closeBtn = transform.Find("close").GetComponent<Button>();
//         nameText = transform.Find("funcName").GetComponent<Text>();
//         bgGo = transform.Find("bg").gameObject;
//         lineGo = transform.Find("line01").gameObject;
//
//         this.closeBtn.onClick.AddListener(() =>
//         {
//             clickCloseBtnAction?.Invoke();
//         });
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         TitleBarUIArgs titleBarListArgs = (TitleBarUIArgs)args;
//
//         this.optionDataList = titleBarListArgs.optionList;
//
//         this.RefreshOptionList();
//
//         nameText.text = titleBarListArgs.titleName;
//         
//         closeBtn.gameObject.SetActive(titleBarListArgs.isShowCloseBtn);
//         bgGo.SetActive(titleBarListArgs.isShowBg);
//         lineGo.SetActive(titleBarListArgs.isShowLine);
//         
//     }
//
//     void RefreshOptionList()
//     {
//         UIListArgs<TitleOptionShowObj, TitleBarUIPre> args = new
//             UIListArgs<TitleOptionShowObj, TitleBarUIPre>();
//         args.dataList = optionDataList;
//         args.showObjList = optionShowList;
//         args.root = optionRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//
//     }
//
//     protected override void OnUnload()
//     {
//         clickCloseBtnAction = null;
//         this.closeBtn.onClick.RemoveAllListeners();
//     }
// }
//
//
// public class TitleBarUIArgs : UIArgs
// {
//     public string titleName;
//     public List<TitleOptionUIData> optionList;
//     public bool isShowCloseBtn;
//     public bool isShowBg;
//     public bool isShowLine;
// }
//
// public class TitleOptionUIData
// {
//     public int configId;
//     public int count;
// }
//
// public class TitleOptionShowObj : BaseUIShowObj<TitleBarUIPre>
// {
//     Text nameText;
//     Text countText;
//     Image iconImg;
//     public TitleOptionUIData uiData;
//
//     int currIconResId;
//     Sprite currIconSprite;
//
//     public override void OnInit()
//     {
//         nameText = this.transform.Find("name").GetComponent<Text>();
//         countText = this.transform.Find("count").GetComponent<Text>();
//         iconImg = this.transform.Find("icon").GetComponent<Image>();
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//
//         this.uiData = (TitleOptionUIData)data;
//
//         var configId = this.uiData.configId;
//         var itemTb = ConfigManager.Instance.GetById<Config.Item>(configId);
//         nameText.text = itemTb.Name;
//         countText.text = "" + this.uiData.count;
//
//         currIconResId = itemTb.IconResId;
//         ResourceManager.Instance.GetObject<Sprite>(currIconResId, (sprite) =>
//         {
//             //TODO
//             //注意 这里界面关闭了还会再次执行
//             //这里应该判断是否界面界面关闭了等状态
//             currIconSprite = sprite;
//             iconImg.sprite = sprite;
//         });
//
//     }
//
//     public override void OnRelease()
//     {
//         if (currIconSprite != null)
//         {
//             ResourceManager.Instance.ReturnObject<Sprite>(currIconResId, currIconSprite);
//         }
//
//     }
//
// }
//
//
