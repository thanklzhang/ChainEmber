using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public class PlayerBuyInfo_Client
    {
        public int hasBuyPopulation;
    }
    public partial class ClientPlayer
    {
        public PlayerBuyInfo_Client buyInfo = new PlayerBuyInfo_Client();
        public void UpdatePlayerBuyInfo(PlayerBuyInfo_Client info)
        {
            buyInfo = info;
            //send msg
            EventDispatcher.Broadcast(EventIDs.OnUpdatePlayerBuyInfo);
        }

    }
}