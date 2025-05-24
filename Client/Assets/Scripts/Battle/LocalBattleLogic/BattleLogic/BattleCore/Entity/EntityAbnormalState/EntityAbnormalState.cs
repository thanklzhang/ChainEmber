using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class EntityAbnormalState
    {
        EntityAbnormalStateType stateType;
        public int count = 0;
        public void Init(EntityAbnormalStateType stateType)
        {
            this.stateType = stateType;
        }

        public void Add()
        {
            count = count + 1;
        }

        public void Remove()
        {
            count = count - 1;
        }

        public void Release()
        {

        }

        internal bool IsActive()
        {
            return this.count > 0;
        }

    }

}
