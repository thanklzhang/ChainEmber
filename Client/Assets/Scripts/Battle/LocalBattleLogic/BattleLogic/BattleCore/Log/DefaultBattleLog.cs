using System;

namespace Battle
{
    public class DefaultBattleLog : IBattleLog
    {
        public void Log(string str)
        {
            Console.WriteLine("battle log : " + str);
        }

        public void LogError(string str)
        {
            Console.WriteLine("battle error : " + str);
        }

        public void Log(int type, string str)
        {
            Console.WriteLine("battle log : " + str);
        }

        public void LogWarning(int type, string str)
        {
            Console.WriteLine("battle warning : " + str);
        }

        public void LogWarning(string str)
        {
            Console.WriteLine("battle warning : " + str);
        }
    }

}

