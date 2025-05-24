using System;
using System.Collections.Generic;
using Battle_Client;

namespace Battle_Client
{
    public partial class BattleManager
    {
        public List<ClientRecvMsg> recvMsgList;

        public void InitBattleRecvMsg()
        {
            recvMsgList = new List<ClientRecvMsg>();
        }

        public void RecvBattleMsg<T>(BaseClientRecvMsgArg arg)
            where T : ClientRecvMsg,  new()
        {
            T t = new T();
            t.msgArg = arg;

            recvMsgList.Add(t);
        }

        public void UpdateRecvMsgList()
        {
            while (recvMsgList.Count > 0)
            {
                var msg = recvMsgList[0];

                try
                {
                    Logx.Log(LogxType.Battle, "(client) recv battle msg : " + msg.GetType());
                    msg.Handle();
                }
                catch (Exception e)
                {
                    Logx.LogError(LogxType.Battle, e);
                }
                finally
                {
                    recvMsgList.RemoveAt(0);
                }
            }
        }

        public void ClearRecvMsg()
        {
            recvMsgList?.Clear();
        }
    }
}