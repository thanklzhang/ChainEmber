using System.Collections.Generic;
using NetProto;
using UnityEngine;

namespace Battle
{
    public partial class SkillEffectMgr
    {
        private List<BuffEffect> willRemoveBuffList;
        private List<EffectOperate> willCreateEffectList;

        public void AddSkillEffect(int effectConfigId, SkillEffectContext context,bool isCopyContext = true)
        {
            List<int> configIds = new List<int>()
            {
                effectConfigId
            };
            AddSkillEffectGroup(configIds,context,isCopyContext);
        }

        public void AddSkillEffectGroup(List<int> configIds, SkillEffectContext context, bool isCopyContext = true)
        {
            // var battle = this.battle;
            var newContext = context;
            if (isCopyContext)
            {
                newContext = context.Copy();
            }

            if (configIds != null)
            {
                willRemoveBuffList = new List<BuffEffect>();
                willCreateEffectList = new List<EffectOperate>();
                foreach (var item in configIds)
                {
                    var id = item;
                    AddSingleSkillEffect(id, newContext);
                }

                //remove
                for (int i = 0; i < willRemoveBuffList.Count; i++)
                {
                    var removeBuff = willRemoveBuffList[i];
                    removeBuff.ForceDelete();
                }

                willRemoveBuffList.Clear();

                //add
                for (int i = 0; i < willCreateEffectList.Count; i++)
                {
                    var createEftOp = willCreateEffectList[i];
                    eftOpList.Add(createEftOp);
                }

                willCreateEffectList.Clear();
            }
        }

        private void AddSingleSkillEffect(int effectConfigId, SkillEffectContext context)
        {
            SkillEffect effect = null;
            var type = SkillEffectFactory.GetTypeByConfigId(effectConfigId);
            if (type == SkillEffectType.BuffEffect)
            {
                effect = CreateBuff(effectConfigId, context);
            }
            else
            {
                //创建实例
                effect = CreateSkillEffect(type, effectConfigId, context);

                // //填充给客户端的创建信息
                // var createInfo = effect.ToCreateEffectInfo();
                // battle?.OnCreateSkillEffect(createInfo);
            }


            // return effect;
        }

        //创建 buff 流程 （buff 有叠层 所以流程不一样）
        SkillEffect CreateBuff(int configId, SkillEffectContext context)
        {
            SkillEffect effect = null;
            var followEntityGuid = 0;
            if (context.selectEntities.Count > 0)
            {
                followEntityGuid = context.selectEntities[0].guid;
            }
            else
            {
                BattleLog.LogError(" : the this.context.selectEntities of count is 0 : in buff , config : " +
                                    configId);
                return null;
            }

            // Battle_Log.LogZxy("zxy : createBuff : followEntityGuid : " + followEntityGuid);

            var entity = battle.FindEntity(followEntityGuid);
            if (null == entity)
            {
                BattleLog.LogError("CreateBuff : the entity is not found , config : " + configId);
                return null;
            }

            BuffEffectInfo buffInfo = null;

            //var tableConfig = BattleConfigManager.Instance.GetById<IBuffEffect>(configId);
            var buff = entity.GetBuffByConfigId(configId);
            var isNeedCreateNew = false;
            var isRemoveOldBuff = false;
            if (buff != null)
            {
                var isAddLayer =
                    (BuffAddLayerType)buff.tableConfig.AddLayerType == BuffAddLayerType.AddLayerAndEffect ||
                    (BuffAddLayerType)buff.tableConfig.AddLayerType == BuffAddLayerType.AddLayerWithoutEffect;
                if (isAddLayer)
                {
                    //叠层
                    var stackCount = 1;
                    buff.AddLayer(stackCount);

                    buffInfo = buff.GetBuffInfo();
                    isNeedCreateNew = false;
                }
                else
                {
                    isNeedCreateNew = true;
                    isRemoveOldBuff = true;
                    //TODO 改为刷新 buff 属性
                }
            }
            else
            {
                isNeedCreateNew = true;
            }

            if (isNeedCreateNew)
            {
                //收集 buffInfo 同步给客户端
                var linkTargetGuid = 0;
                if (context.selectEntities.Count > 1)
                {
                    linkTargetGuid = context.selectEntities[1].guid;
                    // Battle_Log.LogZxy("zxy : createBuff : linkTargetGuid : " + linkTargetGuid);
                }

                buffInfo = BuffEffect.GenNewBuffInfo(configId, followEntityGuid,
                    linkTargetGuid);

                //需要移除之前的 buff 的情况
                if (isRemoveOldBuff)
                {
                    if (buff != null)
                    {
                        // buff.ForceDelete();
                        this.willRemoveBuffList.Add(buff);
                    }
                }

                //创建新的 buff
                effect = CreateSkillEffect(SkillEffectType.BuffEffect,
                    configId, context, buffInfo);
            }
            else
            {
                //不需要新创建 因为直接叠加了buff
                battle.OnUpdateBuffInfo(buffInfo);
            }

            return effect;
        }

        //创建技能效果
        SkillEffect CreateSkillEffect(SkillEffectType type, int effectConfigId,
            SkillEffectContext context, BuffEffectInfo buffEffectInfo = null)
        {
            SkillEffect effect = SkillEffectFactory.GetInstanceByType(type);
            
            var guid = this.GenGuid();

            if (type == SkillEffectType.BuffEffect)
            {
                var buffEffect = effect as BuffEffect;
                var stackCount = 1;
                if (buffEffectInfo.statckCount >= 1)
                {
                    stackCount = buffEffectInfo.statckCount;
                }

                buffEffect.InitLayerCount(stackCount);
            }

            effect.Init(effectConfigId, guid, context);


            // this.skillEffectDic.Add(guid, effect);
            //willCreateEffectList.Add(effect);
            willCreateEffectList.Add(new EffectOperate()
            {
                opType = EffectOperateType.Create,
                opEft = effect,
                buffInfo = buffEffectInfo
            });
            // eftOpList.Add(new EffectOperate()
            // {
            //     opType = EffectOperateType.Create,
            //     opEft = effect,
            //     buffInfo = buffEffectInfo
            // });

            return effect;
        }


        public List<EffectOperate> eftOpList = new List<EffectOperate>();
    }

    public enum EffectOperateType
    {
        Create = 0,
        Delete = 1
    }

    public class EffectOperate
    {
        public EffectOperateType opType; //0 create , 1 delete
        public SkillEffect opEft;
        public BuffEffectInfo buffInfo;
    }
}