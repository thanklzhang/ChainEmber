using System;
using System.Collections;
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
    public class BattleClient_MsgSender_Local : IBattleClientMsgSender
    {
        Battle.Battle battle;

        public BattleClient_MsgSender_Local(Battle.Battle battle)
        {
            this.battle = battle;
        }

        public void Send_PlayerLoadProgress(int progress)
        {
            GameMain.Instance.StartCoroutine(DelayLoadProgress(progress));
            //battle.PlayerMsgReceiver.On_PlayerLoadProgress(0, progress);
        }

        //由于同帧请求同帧返回回有逻辑问题 所以先延迟调用
        //之后会变成统一收敛战斗队列消息进行处理
        IEnumerator DelayLoadProgress(int progress)
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            yield return new WaitForSeconds(0.5f);
            // var myUid = GameDataManager.Instance.UserData.Uid;
            //  
            // // PlayerLoadProgress_BattleMsgArg msgArg = new PlayerLoadProgress_BattleMsgArg();
            // // msg.progress = progress;
            // //
            var arg = new PlayerLoadProgress_MsgArg()
            {
                progress = progress
            };

            battle.OnRecvBattleMsg<PlayerLoadProgress_BattleMsg>(
                hero.playerIndex, arg);
            //battle.PlayerMsgReceiver.On_PlayerLoadProgress((long)myUid, progress);
        }

        public void Send_BattleReadyFinish()
        {
            GameMain.Instance.StartCoroutine(DelayBattleReadyFinish());
            //battle.PlayerMsgReceiver.On_BattleReadyFinish(0);
        }

        IEnumerator DelayBattleReadyFinish()
        {
            yield return new WaitForSeconds(0.1f);
            var myUid = GameDataManager.Instance.UserData.Uid;

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var arg = new BattleReadyFinish_MsgArg()
            {
            };
            battle.OnRecvBattleMsg<ReadyFinish_BattleMsg>(
                hero.playerIndex, arg);


            //battle.PlayerMsgReceiver.On_BattleReadyFinish((long)myUid);
        }


        public void Send_ClientPlotEnd()
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            // var msg = new ClientPlotEnd_BattleMsg()
            // {
            // };
            // var myUid = GameDataManager.Instance.UserData.Uid;
            // battle.PlayerMsgReceiver.On_ClientPlotEnd((long)myUid);

            battle.OnRecvBattleMsg<ClientPlotEnd_BattleMsg>(
                hero.playerIndex, null);
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            if (null == hero)
            {
                return;
            }

            var arg = new MoveEntity_MsgArg()
            {
                moveEntityGuid = guid,
                targetPos = pos,
            };
            battle.OnRecvBattleMsg<MoveEntity_BattleMsg>(
                hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_MoveEntity(guid, pos);
        }


        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos,
            UnityEngine.Vector3 mousePos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
            var _mousePos = new Battle.Vector3(mousePos.x, mousePos.y, mousePos.z);
            //battle.PlayerMsgReceiver.On_UseSkill(releaserGuid, skillId, targetGuid, pos);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var msg = new UseSkill_BattleMsgArg()
            {
                releaserGuid = releaserGuid,
                skillId = skillId,
                targetGuid = targetGuid,
                targetPos = pos,
                mousePos =  _mousePos
            };
            battle.OnRecvBattleMsg<UseSkill_BattleMsg>(
                hero.playerIndex, msg);
        }

        public void Send_UseItem(ItemUseArg_Client itemUseArg)
        {
            var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var arg = new Battle_ItemUseArg()
            {
                itemIndex = itemUseArg.itemIndex,
                releaserGuid = itemUseArg.releaserGuid,
                targetGuid = itemUseArg.targetGuid,
                targetPos = pos,
            };

            // var useItemMsg = new Battle_ItemUseArg()
            // {
            //     itemUseArg = arg
            // };
            battle.OnRecvBattleMsg<UseItem_BattleMsg>(
                hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_UseItem(arg);
        }

        public void Send_UseSkillItem(ItemUseArg_Client itemUseArg)
        {
            var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
            var arg = new Battle_ItemUseArg()
            {
                itemIndex = itemUseArg.itemIndex,
                releaserGuid = itemUseArg.releaserGuid,
                targetGuid = itemUseArg.targetGuid,
                targetPos = pos,
            };
            // battle.PlayerMsgReceiver.On_UseSkillItem(arg);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            // var useSkillItemMsg = new UseSkillItem_BattleMsg()
            // {
            //     itemUseArg = arg
            // };
            battle.OnRecvBattleMsg<UseSkillItem_BattleMsg>(
                hero.playerIndex, arg);
        }

        public void Send_OpenBox(RewardQuality quality)
        {
            Battle_OpenBoxArg arg = new Battle_OpenBoxArg();
            //arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            arg.quality = quality;

            //var hero = BattleManager.Instance.GetLocalCtrlHero();
            var player = BattleManager.Instance.GetLocalPlayer();


            // var arg = new Battle_OpenBoxArg()
            // {
            //     msgArg = arg
            // };

            battle.OnRecvBattleMsg<OpenBox_BattleMsg>(
                player.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_OpenBox(arg);
        }

        public void Send_SelectBoxReward(RewardQuality quality, int index)
        {
            // var hero = BattleManager.Instance.GetLocalCtrlHero();
            var player = BattleManager.Instance.GetLocalPlayer();

            Battle_SelectBoxRewardArg arg = new Battle_SelectBoxRewardArg();
            //arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            arg.quality = quality;
            arg.index = index;

            // SelectBoxReward_BattleMsg msg = new SelectBoxReward_BattleMsg()
            // {
            //     arg = arg
            // };

            battle.OnRecvBattleMsg<SelectBoxReward_BattleMsg>(player.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_SelectBoxReward(arg);
        }

        public void Send_BuyBoxFromShop(RewardQuality quality, int buyCount)
        {
            var arg = new BuyBoxFromShop_BattleMsgArg();
            arg.quality = quality;
            arg.buyCount = buyCount;

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            battle.OnRecvBattleMsg<BuyBoxFromShop_BattleMsg>(hero.playerIndex, arg);
        }

        public void Send_OperateHeroByArraying(int opHeroGuid, Vector3 targetPos, int toUnderstudyIndex)
        {
            var arg = new HeroOperationByArraying_BattleMsgArg();

            arg.opHeroGuid = opHeroGuid;
            arg.targetPos = BattleConvert.ConvertToVector3(targetPos);
            arg.toUnderstudyIndex = toUnderstudyIndex;

            var hero = BattleManager.Instance.GetLocalCtrlHero();
            battle.OnRecvBattleMsg<HeroOperationByArraying_BattleMsg>(hero.playerIndex, arg);
        }

        public void Send_SelectReplaceSkill(int selectSkillId)
        {
            var arg = new SelectReplaceSkill_BattleMsgArg();
            arg.selectSkillId = selectSkillId;

            var hero = BattleManager.Instance.GetLocalCtrlHero();
            battle.OnRecvBattleMsg<SelectReplaceSkill_BattleMsg>(hero.playerIndex, arg);
        }

        public void Send_SelectReplaceHero(int selectConfigId)
        {
            var arg = new SelectReplaceHero_BattleMsgArg();
            arg.entityConfigId = selectConfigId;
            var hero = BattleManager.Instance.GetLocalCtrlHero();
            battle.OnRecvBattleMsg<SelectReplaceHero_BattleMsg>(hero.playerIndex, arg);
        }

        public void Send_MoveItemTo(ItemMoveLocationArg_Client srcArg, ItemMoveLocationArg_Client desArg)
        {
            var arg = new MoveItem_BattleMsg_Arg();
            arg.srcOpLoc = BattleConvert.ToItemOpLocation(srcArg);
            arg.desOpLoc = BattleConvert.ToItemOpLocation(desArg);

            var hero = BattleManager.Instance.GetLocalCtrlHero();
            battle.OnRecvBattleMsg<MoveItem_BattleMsg>(hero.playerIndex, arg);
        }

        public void Send_AskEnterBattleProcess()
        {
            var arg = new AskEnterBattleProcess_BattleMsg_Arg();
            var player = BattleManager.Instance.GetLocalPlayer();
            battle.OnRecvBattleMsg<AskEnterBattleProcess_BattleMsg>(player.playerIndex, arg);
        }

        public void Send_SelectToRevive(int entityGuid,bool isRevive)
        {
            var arg = new SelectToRevive_BattleMsg_Arg
            {
                entityGuid = entityGuid,
                isRevive = isRevive
            };
            var player = BattleManager.Instance.GetLocalPlayer();
            battle.OnRecvBattleMsg<SelectToRevive_BattleMsg>(player.playerIndex, arg);
            
        }

        public void Send_BuyPopulation()
        {
            var arg = new BuyPopulation_BattleMsgArg()
            {
                buyCount = 1
            };
            var player = BattleManager.Instance.GetLocalPlayer();
            battle.OnRecvBattleMsg<BuyPopulation_BattleMsg>(player.playerIndex, arg);
        }
    }
}