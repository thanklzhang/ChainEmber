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
    public class EntityStartMoveByPath_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as EntityStartMoveByPath_RecvMsg_Arg;
            var Guid = arg.Guid;
            var EndPos = arg.EndPos;
            var MoveSpeed = arg.MoveSpeed;
            
            var guid = Guid;
            var targetPos = EndPos;
            var moveSpeed = MoveSpeed;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            var isSkillForce = arg.isSkillForce;
            if (entity != null)
            {
                entity.StartMoveByPath(EndPos, moveSpeed,isSkillForce);
            }
        }
    }

    public class EntityStartMoveByPath_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int Guid;
        public List<UnityEngine.Vector3> EndPos;
        public float MoveSpeed;
        public bool isSkillForce;
    }
}