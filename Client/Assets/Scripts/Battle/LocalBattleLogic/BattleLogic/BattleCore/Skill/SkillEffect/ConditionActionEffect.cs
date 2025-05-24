using System.Collections.Generic;

namespace Battle
{
    public enum ConditionEffectType
    {
        //实体属性和状态
        EntityAttrState = 1,

        //实体关系
        EntityRelation = 201
    }

    public enum ConditionEntityType
    {
        SelectEntity = 0,
        Releaser = 1
    }

    public enum ConditionActionType
    {
        Null = 0,
        SkillEffect = 1
    }


    public class ConditionActionEffect : SkillEffect
    {
        public IConditionActionEffect tableConfig;
        Battle battle;
        Vector3 moveTargetPos;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IConditionActionEffect>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            //_Battle_Log.Log("MoveEffect OnStart");

            //tableConfig = ConfigManager.Instance.GetById<Config.MoveEffect>(this.configId);

            var battle = this.context.battle;
            //默认为技能释放者
            var selectEntity = this.context.selectEntities[0];

            var conditionType = (ConditionEffectType)tableConfig.Condition;
            if (conditionType == ConditionEffectType.EntityRelation)
            {
                //实体关系条件
                var relationList = tableConfig.ConditionParamIntList;
                var operate = tableConfig.Operate;
                var opValue = (EntityRelationType)tableConfig.OpIntValue;

                var type0 = (ConditionEntityType)relationList[0];
                var type1 = (ConditionEntityType)relationList[1];
                var entity0 = GetEntityByCondition(type0);
                var entity1 = GetEntityByCondition(type1);
                var relation = entity0.GetRelationWith(entity1);
                if (entity0 != null && entity1 != null)
                {
                    bool isTriggerAction = relation.Contains(opValue);

                    if (isTriggerAction)
                    {
                        //trigger action
                        TriggerAction();
                    }
                }
            }


            this.SetWillEndState();
        }


        public void TriggerAction()
        {
            //_G.Log("battle", string.Format("AreaEffect effect of guid : {0} TriggerEffect", this.guid));

            var actionType = (ConditionActionType)tableConfig.ActionType;

            if (actionType == ConditionActionType.SkillEffect)
            {
                if (tableConfig.ActionParamIntList.Count > 0)
                {
                    battle.AddSkillEffectGroup(tableConfig.ActionParamIntList, this.context);
                }
            }
        }
        
        
        public BattleEntity GetEntityByCondition(ConditionEntityType type)
        {
            if (type == ConditionEntityType.SelectEntity)
            {
                var selectEntity = this.context.selectEntities[0];
                return selectEntity;
            }
            else if (type == ConditionEntityType.Releaser)
            {
                return this.context.fromSkill.releaser;
            }

            return null;
        }

    }
}