using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public partial class EntityAttrMgr
    {
        //计算 ‘持续改变’ 的属性
        void CalculateContinuousAttr()
        {
            foreach (var groupKV in attrGroupDic)
            {
                var group = groupKV.Value;
                var attrs = group.GetAttrObjs();
                foreach (var attrKV in attrs )
                {
                    var attrObj = attrKV.Value;
                    var partDic = attrObj.attrPartDic;
                    foreach (var partKV in partDic)
                    {
                        var attrPart = partKV.Value;
                        if (attrPart.isContinuous)
                        {
                            attrPart.Calculate(this.battle);
                        }
                    }
                }
            }
        }
    }
}