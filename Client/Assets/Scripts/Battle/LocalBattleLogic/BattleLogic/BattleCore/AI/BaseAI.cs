using System;
using System.Collections.Generic;

namespace Battle
{
    public class BaseAI
    {
        protected BattleEntity entity;

        public bool isAutoAttack = true;

        // 自动攻击检测范围
        protected float autoAttackDetectRange = 10.0f; // 自动攻击的检测范围sa
        
        public void Init(BattleEntity entity)
        {
            this.entity = entity;
            this.OnInit();
        }

        public virtual void OnInit()
        {
        }

        public void SetAISkillInfo()
        {
            this.OnSetAISkillInfo();
        }

        public virtual void OnSetAISkillInfo()
        {
            
        }

        public void Update(float timeDelta)
        {
            this.OnUpdate(timeDelta);
        }

        //无路可走的时候
        public virtual void OnMoveNoWay()
        {
            
        }

        public virtual void OnUpdate(float timeDelta)
        {
        }


        public virtual void OnBeHurt(float resultDamage, Skill fromSkill)
        {
        
        }
    }
}