using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public interface IConfig
    {
        void Init(int id);
        int Id { get; }
    }
}