using System.Collections.Generic;

namespace Battle
{ 
    public partial class Skill
    {
        void NotifySkillTrackStart(SkillTrackStartTimeType startTimeType)
        {
            //根据 skill 获取到 track ， 并且根据触发时机来进行触发
            //type == startTimeType
            foreach (var skillTrackId in this.infoConfig.SkillTrackList)
            {
                if (skillTrackId > 0)
                {
                    var trackConfig = BattleConfigManager.Instance.GetById<ISkillTrack>(skillTrackId);
                    if ((SkillTrackStartTimeType)trackConfig.StartTimeType == startTimeType)
                    {
                        this.battle.OnNotifySkillTrackStart(this, skillTrackId);
                    }
                }
            }
        }
        
        public void NotifySkillTrackEnd(SkillTrackEndTimeType endTimeType)
        {
            //根据 skill 获取到 track ， 并且根据触发时机来进行触发
            foreach (var skillTrackId in this.infoConfig.SkillTrackList)
            {
                if (skillTrackId > 0)
                {
                    var trackConfig = BattleConfigManager.Instance.GetById<ISkillTrack>(skillTrackId);
                    if (endTimeType == SkillTrackEndTimeType.OnEntityDead)
                    {
                        //如果死亡了 那么立即删除 （这里待定 先统一死亡清除逻辑）
                        this.battle.OnNotifySkillTrackEnd(this, skillTrackId);
                    }
                    else
                    {
                        if ((SkillTrackEndTimeType)trackConfig.EndTimeType == endTimeType)
                        {
                            this.battle.OnNotifySkillTrackEnd(this, skillTrackId);
                        }
                    }
                }
            }
        }
    }

}