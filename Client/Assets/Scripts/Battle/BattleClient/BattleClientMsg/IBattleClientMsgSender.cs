using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public class ItemUseArg_Client
    {
        public int itemIndex;
        public int releaserGuid;
        public int targetGuid;
        public UnityEngine.Vector3 targetPos;
    }

   
    public interface IBattleClientMsgSender
    {
        void Send_PlayerLoadProgress(int progress); //千分比
        void Send_BattleReadyFinish();
        void Send_ClientPlotEnd();
        void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos);
        void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos,
            Vector3 mousePos);
        void Send_UseItem(ItemUseArg_Client itemUseArg);
        void Send_UseSkillItem(ItemUseArg_Client itemUseArg);
        void Send_OpenBox(RewardQuality quality);
        void Send_SelectBoxReward(RewardQuality quality, int index);
        void Send_BuyBoxFromShop(RewardQuality quality, int buyCount);
        void Send_OperateHeroByArraying(int opHeroGuid,Vector3 targetPos,int toUnderstudyIndex);
        void Send_SelectReplaceSkill(int selectSkillId);
        void Send_SelectReplaceHero(int selectConfigId);
        void Send_MoveItemTo(ItemMoveLocationArg_Client srcItemMoveArg,ItemMoveLocationArg_Client desItemMoveArg);
        void Send_AskEnterBattleProcess();
        void Send_SelectToRevive(int entityGuid, bool isRevive);
        void Send_BuyPopulation();
    }

    public class ItemMoveLocationArg_Client
    {
        public ItemLocationType locationType;
        public int itemIndex;
        public int entityGuid;
    }

    //道具归属类型
    // public enum ItemBelongType
    // {
    //     //仓库
    //     Warehouse = 0,
    //     //实体道具栏
    //     EntityItemBar = 1
    // }
}