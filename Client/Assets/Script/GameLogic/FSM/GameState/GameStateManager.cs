using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum GameState
{
    Null,
    UpdateResource,//更新资源
    Login,
    Lobby,
    //Room,
    //Hero,
    //Loading,
    Combat,
    //GameOver,
}

public class GameStateManager : BaseStateManager
{
    private static GameStateManager instance;

    public static GameStateManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameStateManager();
            }
            return instance;
        }


    }

    public override void Init()
    {
        this.AddState((int)(GameState.UpdateResource), new UpdateResourceState());
        this.AddState((int)(GameState.Login), new LoginState());
        this.AddState((int)(GameState.Lobby), new LobbyState());
        this.AddState((int)(GameState.Combat), new CombatState());

        base.Init();

    }

    public void ChangeState(GameState nextState, params object[] args)
    {
        base.ChangeState((int)nextState, args);
    }
}

