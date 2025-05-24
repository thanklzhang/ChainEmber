using System.Collections.Generic;

namespace Battle
{
    public class TimeSequenceEffect : SkillEffect
    {
        public ITimeSequenceEffect tableConfig;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<ITimeSequenceEffect>(this.configId);
        }

        public float currTimer = 0.0f;
        public int currTimeIndex = 0;

        public override void OnStart()
        {
            base.OnStart();

            var battle = this.context.battle;

            currTimeIndex = 0;
            currTimer = 0;
        }

        public override void OnUpdate(float timeDelta)
        {
            currTimer += timeDelta;
            var currMaxInterval = tableConfig.IntervalTimeList[currTimeIndex] / 1000.0f;
            if (currTimer >= currMaxInterval)
            {
                TriggerAction(currTimeIndex);
                currTimer = 0;
                currTimeIndex += 1;
                
                if(currTimeIndex >= tableConfig.IntervalTimeList.Count)
                {
                    this.SetWillEndState();
                }
            }
        }


        public void TriggerAction(int index)
        {
            var effectIds = tableConfig.IntervalEffectList[index];
            SkillEffectContext context = this.context.Copy();
            battle.AddSkillEffectGroup(effectIds, context);
          
        }
    }
}