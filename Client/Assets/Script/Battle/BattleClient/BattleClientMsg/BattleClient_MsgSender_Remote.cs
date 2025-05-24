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
    //战斗客户端消息发送器
    public class BattleClient_MsgSender_Remote : IBattleClientMsgSender
    {
        public void Send_PlayerLoadProgress(int progress)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
           // netHandle.SendPlayerLoadProgress(progress);
        }

        public void Send_BattleReadyFinish()
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            //netHandle.SendBattleReadyFinish(null);
        }

        public void Send_ClientPlotEnd()
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            //netHandle.SendClientPlotEnd();
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            //netHandle.SendMoveEntity(guid, targetPos);
        }

        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos
        ,UnityEngine.Vector3 mousePos)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            //netHandle.SendUseSkill(skillId, targetGuid, targetPos);
        }

        public void Send_UseItem(ItemUseArg_Client itemUseArg)
        {
            throw new NotImplementedException();
        }

        public void Send_UseSkillItem(ItemUseArg_Client itemUseArg)
        {
            throw new NotImplementedException();
        }

        public void Send_OpenBox(RewardQuality quality)
        {
            throw new NotImplementedException();
        }

        public void Send_SelectBoxReward(RewardQuality quality,int index)
        {
            throw new NotImplementedException();
        }

        public void Send_BuyBoxFromShop(RewardQuality quality, int buyCount)
        {
            throw new NotImplementedException();
        }

        public void Send_OperateHeroByArraying(int opHeroGuid, Vector3 targetPos, int toUnderstudyIndex)
        {
            throw new NotImplementedException();
        }

        public void Send_SelectReplaceSkill(int selectSkillId)
        {
            throw new NotImplementedException();
        }

        public void Send_SelectReplaceHero(int selectConfigId)
        {
            throw new NotImplementedException();
        }

        public void Send_MoveItemTo(ItemMoveLocationArg_Client srcArg,ItemMoveLocationArg_Client desArg)
        {
            
        }

        public void Send_AskEnterBattleProcess()
        {
            throw new NotImplementedException();
        }

        public void Send_SelectToRevive(int entityGuid, bool isRevive)
        {
            throw new NotImplementedException();
        }

        public void Send_BuyPopulation()
        {
            throw new NotImplementedException();
        }
    }
}