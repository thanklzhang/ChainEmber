using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //最小的属性部分 只包含一个影响属性的因素
    public class EntityAttrPart
    {
        //属性相关项 对于 ‘持续改变’ 的属性会有作用
        public AttrOption attrOption;

        public int guid
        {
            get
            {
                if (attrOption != null)
                {
                    return attrOption.guid;
                }

                return 0;
            }
        }

        public bool isContinuous
        {
            get
            {
                if (attrOption != null)
                {
                    return attrOption.isContinuous;
                }

                return false;
            }
        }

        //runtime--------------
        //正常属性值
        public float value;

        public void Init(AttrOption _attrOption,Battle battle)
        {
            this.attrOption = _attrOption;
            Calculate(battle);
        }

        public void Calculate(Battle battle)
        {
            if (isContinuous)
            {
                this.value = AttrHelper.CalculateAttrAddedValue(this.attrOption,battle);
                //取不带 ‘持续改变’ 属性的属性计算
            }
            else
            {
                this.value = this.attrOption.initValue;
            }
        }

        //直接更改值，适用于等级，星级等属性的升级刷新 , buff 等使用的时候需要注意，尽量先不要使用
        public void SetValue(float value)
        {
            this.attrOption.initValue = value;
            this.value = value;
        }
    }
}