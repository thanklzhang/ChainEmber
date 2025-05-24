using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public enum AIThinkType
    {
        Idle = 0,
        Skill = 1,
        Escape = 2,
    }
    
    
    public class AIAnalyseResult
    {
        public float priority;
            
        public Skill skill;
        public BattleEntity expectTarget;
        public Vector3 expectPos;

        public bool isEscape;
    }

    public partial class CpuAI : BaseAI
    {
       
    }
}