using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseState
{
    public int state;
    
    public virtual void Init( )
    {
      
    }

    public virtual void Enter(params object[] args)
    {
        
    }

    public virtual void Exit()
    {

    }

    public virtual void Excute()
    {

    }

}
