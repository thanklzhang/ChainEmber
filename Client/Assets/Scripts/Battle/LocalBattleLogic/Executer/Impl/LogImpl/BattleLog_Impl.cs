using System;

namespace Battle
{
    public class BattleLog_Impl : IBattleLog
    {
        public void Log(string str)
        {
            Logx.Log(LogxType.Battle,str);
        }

        public void LogError(string str)
        {
            Logx.LogError(LogxType.Battle,str);
        }

        public void Log(int type, string str)
        {
            Logx.Log((LogxType)type,str);
        }

        public void LogWarning(int type, string str)
        {
            Logx.LogWarning((LogxType)type,str);
        }

        public void LogWarning(string str)
        {
            Logx.LogWarning(LogxType.Battle,str);
        }
    }

}

