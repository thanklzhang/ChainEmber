using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Battle
{
    public class EntityInit
    {
        public int configId;
        public int level;
        //public int team;
        public int playerIndex;
        public Vector3 position;
        public int star;
        public Vector3 dir = Vector3.forward;

        public List<SkillInit> skillInitList = new List<SkillInit>();
        public bool isPlayerCtrl;

        // public bool isSummonEntity;

        public BattleEntityRoleType roleType;
        public float summonLastTime;
        public BattleEntity beSummonMaster;

        public BattleEntity teamLeader;

    }

    public class BattleEntityInitArg
    {
        public List<EntityInit> entityInitList = new List<EntityInit>();
    }

    public class SkillInit
    {
        public int configId;
        public int level;
    }

    // public class CreateEntityArg
    // {
    //     public int configId;
    //     public int level = 1;
    //     public int star = 1;
    //     public List<int> skillLevelList = new List<int>();
    // }
}