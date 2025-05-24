using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Battle
{
    public class BattleRecvMsgContext
    {
        //发送者玩家 index
        public int playerIndex;
        public Battle battle;
    }
    public class BaseBattleRecvMsg
    {
        public BaseBattleRecvMsgArg msgArg;
        public BattleRecvMsgContext context;
        public virtual void Handle()
        {
            
        }

        public BattlePlayer GetRecvPlayer()
        {
            var playerIndex = context.playerIndex;
            var player = context.battle.FindPlayerByPlayerIndex(playerIndex);
            return player;
        }

        public BattleEntity GetRecvPlayerEntity()
        {
            var player = GetRecvPlayer();
            var entity = context.battle.FindEntity(player.ctrlHeroGuid);
            return entity;
        }
    }

    public class BaseBattleRecvMsgArg
    {
        
    }

    public class BattleRecvMsgManager
    {
        List<BaseBattleRecvMsg> msgList;
        private Battle battle;
        public void Init(Battle battle)
        {
            this.battle = battle;
            msgList = new List<BaseBattleRecvMsg>();
        }

        public void OnRecvMsg<T>(int playerIndex,BaseBattleRecvMsgArg arg) where T : BaseBattleRecvMsg,new()
        {
            T t = new T();
            t.context = new BattleRecvMsgContext()
            {
                battle = this.battle,
                playerIndex = playerIndex
            };
            t.msgArg = arg;
            msgList.Add(t);
        }

        public void Update()
        {
            while (msgList.Count > 0)
            {
                var msg = msgList[0];

                try
                {
                    //Logx.Log(LogxType.Zxy,"recv battle msg : " + msg.GetType());
                    msg.Handle();
                }
                catch (Exception e)
                {   
                    BattleLog.LogError(e.ToString());
                }
                finally
                {
                    msgList.RemoveAt(0);
                }
            }
         
        }

        public void Clear()
        {
            msgList?.Clear();;
        }
    }
   
}

