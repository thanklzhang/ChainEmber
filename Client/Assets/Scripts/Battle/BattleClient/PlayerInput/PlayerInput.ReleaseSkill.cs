using System.Collections.Generic;
using Battle;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class PlayerInput
    {
        //开始执行技能操作
        void HandleReleaseSkill()
        {
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();

            // var index = willReleaserSkillId;
            var skillId = willReleaserSkillId;
            
            //var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
            var releaseTargetType = (SkillReleaseTargetType)skillConfig.SkillReleaseTargeType;

            if (releaseTargetType == SkillReleaseTargetType.Point)
            {
                var isColliderGround = TryToGetRayTargetPos(out var hit);
                //确定选择技能目标
                if (isColliderGround)
                {
                    // skillDirectModule.FinishSelect();
                    // willUseItemIndex = -1;
                    int targetGuid = 0;
                    Vector3 targetPos = hit.point;

                    SuccessRelease(targetGuid,targetPos,skillId);
                }

                skillDirectModule.FinishSelect();
                willUseItemIndex = -1;
            }
            else if (releaseTargetType == SkillReleaseTargetType.Entity)
            {
                //-----------------------

                GameObject gameObject = null;
                List<int> entityGuidList;
                BattleEntity_Client battleEntity = null;
                if (TryToGetRayOnEntity(out entityGuidList))
                {
                    //遍历寻找 效率低下 之后更改
                    //只找一个
                    battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
                }
                else
                {
                    //没有目标 那么就选择最近一段距离的某个单位
                    float dis = 10.0f;
                    battleEntity = BattleEntityManager.Instance.FindNearestEntity(localEntity, dis);
                }

                if (battleEntity != null)
                {
                    //Logx.Log("battle entity not null");
                    var targetGuid = battleEntity.guid;

                    var targetPos = Vector3.right;
                    // //先排除自己
                    // if (localEntity.collider.gameObject.GetInstanceID() !=
                    //     battleEntity.collider.gameObject.GetInstanceID())
                    // {
                    //     //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                    //     SuccessRelease(targetGuid,targetPos,skillId);
                    // }
                    
                    SuccessRelease(targetGuid,targetPos,skillId);
                }


                skillDirectModule.FinishSelect();
                willUseItemIndex = -1;
            }
        }

        void SuccessRelease(int targetGuid,Vector3 targetPos,int skillId)
        {
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();
            if (willUseItemIndex > 0)
            {
                //使用道具
                var arg = new ItemUseArg_Client()
                {
                    itemIndex = willUseItemIndex,
                    releaserGuid = localEntity.guid,
                    targetGuid = targetGuid,
                    targetPos = targetPos
                };

                Logx.Log(LogxType.Battle, "Use item : itemIndex : " + arg.itemIndex);

                BattleManager.Instance.MsgSender.Send_UseItem(arg);
            }
            else
            {
                //技能
                Vector3 mousePos = Vector3.zero;
                if (TryToGetRayTargetPos(out var hit))
                {
                    mousePos = hit.point;
                }

                BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid,
                    targetPos,mousePos);
            }
        }
    }
}