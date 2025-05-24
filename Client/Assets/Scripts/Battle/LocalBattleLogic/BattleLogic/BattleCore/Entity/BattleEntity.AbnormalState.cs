using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Battle
{
    public partial class BattleEntity
    {
        private  EntityAbnormalStateMgr abnormalStateMgr;
        
        private void InitAbnormalState()
        {
            abnormalStateMgr = new EntityAbnormalStateMgr();
            abnormalStateMgr.Init(this);
        }

        //abnormal state
        public void AddAbnormalState(EntityAbnormalStateType type)
        {
            this.abnormalStateMgr.Add(type);

            //判断是否造成了无法移动
            if (!this.IsCanMoveByAbnormalState())
            {
                this.ChangeToIdle();
            }

            //判断是否打断当前技能
            var isBreak = this.abnormalStateMgr.IsBreakSkill(type);
            if (isBreak)
            {
                BreakSkills();
            }
        }


        public void RemoveAbnormalState(EntityAbnormalStateType type)
        {
            this.abnormalStateMgr.Remove(type);
            if (!this.IsDead())
            {
                if (!this.IsCanMoveByAbnormalState())
                {
                    this.ChangeToIdle();
                }
            }
        }

        public bool IsExistAbnormalState(EntityAbnormalStateType type)
        {
            return this.abnormalStateMgr.IsExist(type);
        }

    }
}