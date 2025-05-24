using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //单位的属性
    //原则：属性在读取或者储存的的时候都应该转换成整数，而在运行时候的时候都应该是浮点数
    public class AttrHelper
    {
        public static bool IsPermillage(EntityAttrType attrType)
        {
            if ((int)attrType >= 1000)
            {
                return true;
            }

            return false;
        }

        public static EntityAttrType GetPermillageTypeByFixedType(EntityAttrType attrType)
        {
            var type = (EntityAttrType)((int)attrType + 1000);
            return type;
        }

        public static EntityAttrType GetFixedTypeByPermillageType(EntityAttrType attrType)
        {
            var type = (EntityAttrType)((int)attrType - 1000);
            return type;
        }

        public class AttrCreateArg
        {
            //必填-----
            public int guid;
            public int attrGroupConfigId;
            public EntityAttrGroupType attrGroupType;
            //----------
            
            //计算相关的单位
            public BattleEntity releaser;
            public BattleEntity target;
            public BattleEntity teamLeader;

            //层数 ， buff 用到
            public int layerCount = 1;
            
        }

        //是否是固定浮点数
        public static bool IsFixedFloatValue(EntityAttrType attrType)
        {
            if (attrType == EntityAttrType.MoveSpeed ||
                attrType == EntityAttrType.AttackSpeed ||
                attrType == EntityAttrType.AttackRange || 
                attrType == EntityAttrType.InputDamageRate || 
                attrType == EntityAttrType.OutputDamageRate || 
                attrType == EntityAttrType.CritRate || 
                attrType == EntityAttrType.CritDamage ||
                attrType == EntityAttrType.TreatmentRate)
            {
                return true;
            }

            return false;
        }

        //运行时的值转变成DTO的值
        public static int ToDtoAttrValue(EntityAttrType attrType, float value)
        {
            if (IsFixedFloatValue(attrType))
            {
                return (int)(value * 1000);
            }

            return (int)value;
        }

        public static float GetAttrFloatValue(EntityAttrType attrType, int value)
        {
            if (IsFixedFloatValue(attrType))
            {
                return value / 1000.0f;
            }

            return value;
        }

        public static List<AttrOption> GetAttrOptions(AttrCreateArg arg,Battle battle)
        {
            var attrOptions = new List<AttrOption>();
            
            var tableConfig = BattleConfigManager.Instance.GetById<IBattleAttributeGroup>(arg.attrGroupConfigId);
            for (int i = 0; i < tableConfig.AddedValueGroup.Count; i++)
            {
                AttrOption option = new AttrOption();
                EntityAttrType attrType = (EntityAttrType)tableConfig.AddedAttrGroup[i];
                option.guid = arg.guid;
                option.entityAttrType = attrType;
                //附加属性计算
                var addedValueConfig = tableConfig.AddedValueGroup[i];
                option.addedValueType =
                    addedValueConfig.Count > 0 ? (AddedValueType)addedValueConfig[0] : (AddedValueType)0;
                option.addedParamValue = addedValueConfig.Count > 1 ? addedValueConfig[1] * arg.layerCount : 0;
                var calculateTargetType = addedValueConfig.Count > 2
                    ? (AttrCalculateTargetType)addedValueConfig[2]
                    : AttrCalculateTargetType.Releaser;
                if (calculateTargetType == AttrCalculateTargetType.Releaser)
                {
                    option.calculateTarget = arg.releaser;
                }
                else if (calculateTargetType == AttrCalculateTargetType.EffectTarget)
                {
                    option.calculateTarget = arg.target;
                }
                else if (calculateTargetType == AttrCalculateTargetType.TeamLeader)
                {
                    option.calculateTarget = arg.teamLeader;
                }

                bool isRand = false;
                List<int> randParamList = null;
                if (i < tableConfig.AddedValueRand.Count)
                {
                    randParamList = tableConfig.AddedValueRand[i];
                    if (randParamList != null && randParamList.Count >= 2)
                    {
                        isRand = true;
                    }
                }

                if (isRand)
                {
                    option.isRandValue = isRand;
                    option.minRandValue = randParamList[0];
                    option.maxRandValue = randParamList[1];
                }

                //是否是持续性改变属性
                bool isContinuous = false;
                if (i < tableConfig.IsAddedAttrGroupContinuous.Count)
                {
                    isContinuous = 1 == tableConfig.IsAddedAttrGroupContinuous[i];
                }

                option.isContinuous = isContinuous;

                //初始值
                var initValue = AttrHelper.CalculateAttrAddedValue(option,battle);
                option.initValue = initValue;

                attrOptions.Add(option);
            }

            return attrOptions;
        }


        //计算根据不同效果下的属性附加值
        public static float CalculateAttrAddedValue(AttrOption _attrOption,Battle battle)
        {
            //根据 attrOption 实时计算
            var attrOption = _attrOption;

            //持续改变类型的属性计算时 取其他属性值的时候不取带有持续改变类型的属性部分
            //比如 有一个持续改变类型的属性：增加攻击力100%的护甲 ， 那么增加护甲时候 ，由于是持续改变类型 ， 
            //那么这个攻击力取值是取‘没有持续改变类型’的数值部分 ， 比如 攻击力含有80点攻击力是‘非持续改变类型’ ， 20 是持续改变类型’
            //那么就只会取80 点攻击力
            //这种是为了防止循环叠加属性，而且也是区分这个属性是否是临时属性（非持续改变类型）的需要
            //临时属性 ： 计算时值计算当前值 即便之后环境因素改了 该值也不会改变
            //持续改变属性 ： 反之 之后环境因素改了 该值会实时随之更改
            bool isIncludeContinuous = !_attrOption.isContinuous;
            //buffAttr.calculateTarget
            var valueType = attrOption.addedValueType;

            var calculateTarget = attrOption.calculateTarget;
            //这里先做一种属性的关联 之后弄多个属性关联
            var sum = 0.0f;
            float resultValue = 0;
            var paramValue = attrOption.addedParamValue;
            if (attrOption.isRandValue)
            {
                paramValue = BattleRandom.GetRandInt(attrOption.minRandValue,
                    attrOption.maxRandValue + 1, battle);

                BattleLog.LogZxy($"zxy : rand : {paramValue} , min:{attrOption.minRandValue} ," +
                                  $"max:{attrOption.maxRandValue}");
            }

            if (valueType == AddedValueType.Fixed)
            {
                //读取的值 转化为运行时的值
                if (IsFixedFloatValue(attrOption.entityAttrType))
                {
                    sum +=  paramValue / 1000.0f;
                }
                else
                {
                    sum += paramValue;
                }
            }
            else if (valueType == AddedValueType.PhysicAttack_Permillage)
            {
                if (calculateTarget != null)
                {
                    resultValue =
                        calculateTarget.attrMgr.GetFinalAttrValue(EntityAttrType.Attack, isIncludeContinuous) *
                        (paramValue / 1000.0f);
                }

                sum += resultValue;
            }
            else if (valueType == AddedValueType.MaxHealth_Permillage)
            {
                if (calculateTarget != null)
                {
                    resultValue =
                        calculateTarget.attrMgr.GetFinalAttrValue(EntityAttrType.MaxHealth, isIncludeContinuous) *
                        (paramValue / 1000.0f);
                }

                sum += resultValue;
            }
            else if (valueType == AddedValueType.Defence_Permillage)
            {
                if (calculateTarget != null)
                {
                    resultValue =
                        calculateTarget.attrMgr.GetFinalAttrValue(EntityAttrType.Defence, isIncludeContinuous) *
                        (paramValue / 1000.0f);
                }

                sum += resultValue;
            }
            else if (valueType == AddedValueType.LostHealth_Permillage_Rate)
            {
                if (calculateTarget != null)
                {
                    var maxHealth =
                        calculateTarget.attrMgr.GetFinalAttrValue(EntityAttrType.MaxHealth, isIncludeContinuous);
                    var currHealth = calculateTarget.GetCurrHp();
                    var ratio = (maxHealth - currHealth) / (float)maxHealth;

                    resultValue = ratio * paramValue;
                }

                sum += resultValue;
            }

            return sum;
        }
    }
}