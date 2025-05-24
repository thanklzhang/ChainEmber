using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace Battle
{
    //操作结果代码类型
    public enum ResultCodeType
    {
        Success = 0,
        AddSkillFull = 50,
        AddLeaderSkillFull = 51,
        AddTeamMemberFull = 61,
    }

    public class ResultCode
    {
        public ResultCodeType type;
        public int intArg0;
        public int intArg1;
        public int intArg2;
        public List<string> paramList;

        public static ResultCode successTemp = new ResultCode()
        {
            type = ResultCodeType.Success
        };

        public static ResultCode Success => successTemp;
    }
}