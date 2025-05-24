using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//本地玩家实体的装备栏显示
public class LocalPlayerItemUIShowObj : ItemCellUIShowObj
{
    private DragScript dragScript;
    private DropScript dropScript;

    public override void Init(GameObject gameObject, BattleUI battleUI,int entityGuid)
    {
        base.Init(gameObject, battleUI,entityGuid);

        locationType = ItemLocationType.EntityItemBar;
        
        dragScript = itemTran.gameObject.GetComponent<DragScript>();
        if (null == dragScript)
        {
            dragScript = itemTran.gameObject.AddComponent<DragScript>();
        }

        dragScript.target = itemTran;

        dragScript.onBeginDragBeforeAction += OnBeginDrag_Before;
        dragScript.onBeginDragAction += OnBeginDrag;
        dragScript.onDragAction += OnDrag;
        dragScript.onEndDragAction += OnEndDrag;


        dropScript = this.gameObject.GetComponent<DropScript>();
      
        dropScript.OnDropAction += OnDropAction;
    }


    //作为拖动开始点------------------
    public void OnBeginDrag_Before(PointerEventData eventData)
    {
        dragScript.transferData = this;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       

    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //this.parentUI.OnItemEndDrag(this,eventData);

       
    }


    //作为拖动结束点-------------------------
    public void OnDropAction(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        var dragScript = dropped.GetComponent<DragScript>();

        if (null == dragScript)
        {
            return;
        }

        if (dragScript.transferData is ItemCellUIShowObj)
        {
            var itemCellUIShowObj = dragScript.transferData as ItemCellUIShowObj;
            if (itemCellUIShowObj != null)
            {
                BattleManager.Instance.MsgSender.Send_MoveItemTo(
                    itemCellUIShowObj.GetItemMoveLocationArg(), this.GetItemMoveLocationArg());
                
                // //TODO 可以通用
                // if (itemCellUIShowObj.locationType == ItemLocationType.EntityItemBar)
                // {
                //     //从实体道具栏拖过来的
                //     var showObj = dragScript.transferData as ItemCellUIShowObj;
                //
                //     ItemMoveLocationArg_Client srcMoveArg = new ItemMoveLocationArg_Client();
                //     srcMoveArg.locationType = ItemLocationType.EntityItemBar;
                //     srcMoveArg.itemIndex = showObj.index;
                //     //这里如果还是用的 ItemUIShowObj 那么需要传入 entityGuid
                //     srcMoveArg.entityGuid = showObj.entityGuid;
                //
                //     ItemMoveLocationArg_Client desMoveArg = new ItemMoveLocationArg_Client();
                //     desMoveArg.locationType = ItemLocationType.EntityItemBar;
                //     desMoveArg.itemIndex = this.index;
                //     desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
                //
                //
                //     BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
                // }
                // else if (itemCellUIShowObj.locationType == ItemLocationType.Warehouse)
                // {
                //     //从玩家仓库拖过来的
                //     var showObj = dragScript.transferData as WarehouseItemUIShowObj;
                //
                //     ItemMoveLocationArg_Client srcMoveArg = new ItemMoveLocationArg_Client();
                //     srcMoveArg.locationType = ItemLocationType.Warehouse;
                //     srcMoveArg.itemIndex = showObj.index;
                //
                //     ItemMoveLocationArg_Client desMoveArg = new ItemMoveLocationArg_Client();
                //     desMoveArg.locationType = ItemLocationType.EntityItemBar;
                //     desMoveArg.itemIndex = this.index;
                //     desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
                //
                //     BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
                // }
                //
                // // GameObject.Destroy(dropped);
            }
        }

      
    }
    
    public override ItemMoveLocationArg_Client GetItemMoveLocationArg()
    {
        ItemMoveLocationArg_Client moveLocArg = new ItemMoveLocationArg_Client();
        moveLocArg.locationType = ItemLocationType.EntityItemBar;
        moveLocArg.itemIndex = this.index;
        moveLocArg.entityGuid = this.entityGuid;
        return moveLocArg;
    }

    public override void Release()
    {
        base.Release();

        dragScript.onBeginDragBeforeAction -= OnBeginDrag_Before;
        dragScript.onBeginDragAction -= OnBeginDrag;
        dragScript.onDragAction -= OnDrag;
        dragScript.onEndDragAction -= OnEndDrag;

        dropScript.OnDropAction -= OnDropAction;
    }
}