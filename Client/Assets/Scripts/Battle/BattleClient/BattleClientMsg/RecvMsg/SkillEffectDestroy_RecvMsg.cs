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
using UnityEngine.UI;

namespace Battle_Client
{
    public class SkillEffectDestroy_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillEffectDestroy_RecvMsg_Arg;
            var effectGuid = arg.effectGuid;
          
            //移除
            BattleSkillEffectManager_Client.Instance.DestroySkillEffect(effectGuid);
            
            // //如果是 buff 先处理 UI 显示
            // var effect = BattleSkillEffectManager_Client.Instance.FindSkillEffect(effectGuid);
            // if (effect != null)
            // {
            //     var targetEntityGuid = effect.GetFollowEntityGuid();
            //
            //     if (targetEntityGuid > 0)
            //     {
            //         //var entity = BattleEntityManager.Instance.FindEntity(targetEntityGuid);
            //         //if (entity != null)
            //         //{
            //
            //         //}
            //         BuffEffectInfo_Client buffUIData = new BuffEffectInfo_Client()
            //         {
            //             targetEntityGuid = targetEntityGuid,
            //             guid = effectGuid,
            //             isRemove = true
            //         };
            //         EventDispatcher.Broadcast(EventIDs.OnBuffInfoUpdate, buffUIData);
            //     }
            // }
            
        }
    }

    public class SkillEffectDestroy_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int effectGuid;
    }
}