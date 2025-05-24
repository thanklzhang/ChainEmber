using System.Collections.Generic;

namespace Battle
{
    public partial class Skill
    {
        public bool isWillDelete;

        public void SetExp(int exp)
        {
            BattleLog.Log($" {this.currExp} => {exp}");
            this.currExp = exp;
        }

        public void Upgrade()
        {
            //升级会直接换 Skill 类 ， 所以这里只是作为一个运行节点
        }

        public void SetWillDelete()
        {
            isWillDelete = true;
        }
    }
}