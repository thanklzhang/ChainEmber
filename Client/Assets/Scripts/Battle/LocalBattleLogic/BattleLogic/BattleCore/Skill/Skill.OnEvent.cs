using System.Collections.Generic;

namespace Battle
{ 
    public partial class Skill
    {
       
        public virtual void OnCDEnd()
        {
            this.battle.OnSkillInfoUpdate(this);
        }

        
        //已经释放了技能效果
        public void OnFinishSkillEffect()
        {
            var entityGuid = this.releaser.guid;
            var skill = this;
            // this.battle.OnEntityFinishSkillEffect(entityGuid, skill);
        }

        //改为 after 状态
        public void OnChangeToSkillAfter()
        {
            //_Battle_Log.Log(string.Format("entity{0} OnChageToSkillAfter", this.releser.infoConfig.Name));
            this.state = ReleaseSkillState.SkillAfter;

            //currAfterReleaseTimer = GetSkillAfterTotalTime() + currBeforeReleaseTimer;
            //currAfterReleaseTimer = this.infoConfig.AfterTime / 1000.0f;
            currAfterReleaseTimer = GetSkillAfterTotalTime();

        }
    }

}