using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleConvert
    {
        public static Vector3Proto ConvertToVector3Proto(Vector3 position)
        {
            Vector3Proto v3Proto = new Vector3Proto();
            v3Proto.X = ToValue(position.x);
            v3Proto.Y = ToValue(position.y);
            v3Proto.Z = ToValue(position.z);
            return v3Proto;
        }

        public static Vector3 ConvertToVector3(Vector3Proto vector3Proto)
        {
            var x = GetValue(vector3Proto.X);
            var y = GetValue(vector3Proto.Y);
            var z = GetValue(vector3Proto.Z);
            Vector3 vector3 = new Vector3(x, y, z);
            return vector3;
        }

        public static Vector3 ConvertToVector3(Battle.Vector3 position)
        {
            UnityEngine.Vector3 v = new Vector3()
            {
                x = position.x,
                y = position.y,
                z = position.z
            };

            return v;
        }

        public static Battle.Vector3 ConvertToVector3(UnityEngine.Vector3 uPos)
        {
            Battle.Vector3 pos = new Battle.Vector3();
            pos.x = uPos.x;
            pos.y = uPos.y;
            pos.z = uPos.z;
            return pos;
        }

        public static int ToValue(float value)
        {
            int v = (int)(value * 1000);
            return v;
        }

        public static float GetValue(int value)
        {
            return value / 1000.0f;
        }

        public static Battle.BuffEffectInfo ToBuffInfo(BuffInfoProto buffProto)
        {
            Battle.BuffEffectInfo buff = new Battle.BuffEffectInfo();
            buff.guid = buffProto.Guid;
            buff.configId = buffProto.BuffConfigId;
            buff.currCDTime = buffProto.CurrCDTime;
            buff.maxCDTime = buffProto.MaxCDTime;
            buff.statckCount = buffProto.StackCount;
            buff.targetEntityGuid = buffProto.TargetEntityGuid;

            return buff;
        }
    }
}