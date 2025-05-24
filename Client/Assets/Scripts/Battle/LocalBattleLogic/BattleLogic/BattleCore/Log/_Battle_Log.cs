using System;
using System.Collections.Generic;

namespace Battle
{
    public class BattleLog
    {
        static IBattleLog log;

        public static void RegisterLog(IBattleLog _log)
        {
            log = _log;
            if (null == log)
            {
                log = new DefaultBattleLog();
            }
        }

        public static void Log(string str)
        {
            log.Log(str);
        }

        public static void Log(int type, string str)
        {
            log.Log(type, str);
        }

        public static void LogZxy(string str)
        {
            log.Log(200, str);
        }
        
        
        public static void LogWarningZxy(string str)
        {
            log.LogWarning(200, str);
        }
        
        public static void LogWarning(string str)
        {
            log.LogWarning(str);
        }

        public static void LogError(string str, Exception e = null)
        {
            var resultStr = str;

            log.LogError(str);
        }

        public static void LogException(Exception e = null)
        {
            var str = "";
            if (e != null)
            {
                str = e.Message + "\n" + e.StackTrace;
            }

            log.LogError(str);
        }

        public static void LogVector3List(int type,List<Vector3> vects)
        {
            string str = "----->\n";
            for (int i = 0; i < vects.Count; i++)
            {
                var v = vects[i];
                str += v.x + "," + v.y + "," + v.z;
                str += "\n";
            }

            str += "<--------";
            log.Log(type, str);
        }
    }
}