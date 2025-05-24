using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;


namespace Battle
{
    //战斗技能相关
    public partial class Battle
    {
      
        internal void DeleteBuffFromEntity(int guid, int id)
        {
            this.skillEffectMgr.DeleteBuffFromEntity(guid, id);
        }

        public void AddSkillEffectGroup(List<int> configIds, SkillEffectContext context, bool isCopyContext = true)
        {
            this.skillEffectMgr.AddSkillEffectGroup(configIds, context,isCopyContext);
        }

        //添加一个技能效果
        // public void AddSkillEffect(int effectConfigId, SkillEffectContext context
        //     ,bool isCopyContext = true)
        // {
        //      this.skillEffectMgr.AddSkillEffect(effectConfigId, context,isCopyContext);
        // }


        public void DeleteSkillEffect(SkillEffect effect)
        {
            this.skillEffectMgr.DeleteSkillEffect(effect);
        }

    }
}