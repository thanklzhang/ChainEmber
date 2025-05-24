using System.Collections.Generic;

namespace Battle
{
    public enum CollisionGroupAffectType
    {
        //发生的效果改变千分比
        ChangeEffectPercent = 1,
    }

    //碰撞组 用于多次碰撞一个实体而有不同表现的效果
    public class CollisionGroupEffect : SkillEffect
    {
        public ICollisionGroupEffect tableConfig;
        Battle battle;
        HashSet<int> colliderEntityGuidSet;

        //用于判定的效果 id 一般选择第一个 也就是说例如 第一个是可转向技能
        //那么转向的时候会清除 collision cache 第一个 effect 转向的时候就清除
        //其他的 effect 不管
        //只是根据这个字段
        int judgeEffectId;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<ICollisionGroupEffect>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            colliderEntityGuidSet = new HashSet<int>();

            context.collisonGroupEffect = this;
            
            battle.AddSkillEffectGroup(tableConfig.SkillEffectIds, context,true);
            // foreach (var item in tableConfig.SkillEffectIds)
            // {
            //     var id = item;
            //     var battle = this.context.battle;
            //
            //     context.collisonGroupEffect = this;
            //
            //     battle.AddSkillEffect(id, context);
            // }

            judgeEffectId = tableConfig.SkillEffectIds[0];
        }

        public bool IsHasCollsion(int entityGuid)
        {
            return colliderEntityGuidSet.Contains(entityGuid);
        }

        public void OnCollisionEntity(int entityGuid)
        {
            colliderEntityGuidSet.Add(entityGuid);
        }

        public void ClearCollisionCache(int entityGuid)
        {
            if (entityGuid == judgeEffectId)
            {
                colliderEntityGuidSet.Clear();
            }
        }

        public override void OnUpdate(float timeDetla)
        {
        }

        public override void OnEnd()
        {
        }
    }
}