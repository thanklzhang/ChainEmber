using System.Collections.Generic;

namespace Battle
{
    public class ItemSkill : Skill
    {
        private BattleItem item;

        public void SetItem(BattleItem item)
        {
            this.item = item;
        }

        public override void OnCDEnd()
        {
            this.battle.OnItemInfoUpdate(item);
        }
    }
}