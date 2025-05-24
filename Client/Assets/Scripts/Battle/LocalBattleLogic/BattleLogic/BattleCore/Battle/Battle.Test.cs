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
    //战斗消息
    public partial class Battle
    {
        void TestCreateEntityItems()
        {
            var playerList = this.battlePlayerMgr.battlePlayerList;

            foreach (var player in playerList)
            {
                if (0 == player.playerIndex)
                {
                    
                    //gainItem
                        
                    // //tset
                    // for (int i = 0; i < 10; i++)
                    // {
                    //     var id = 100001 + i;
                    //     var _item = this.battleItemMgr.GenerateItem(id);
                    //     player.GainItem(_item);
                    // }
                    // var item = this.battleItemMgr.GenerateItem(100011);
                    // player.GainItem(item);
                    //
                    // item = this.battleItemMgr.GenerateItem(100012);
                    // player.GainItem(item);
                    //
                    // item = this.battleItemMgr.GenerateItem(100013);
                    // player.GainItem(item);
                    //
                    // item = this.battleItemMgr.GenerateItem(100014);
                    // player.GainItem(item);
                    //
                    // //
                    
                    //gain box
                    var boxConfgiId = 100;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 103;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // player.GainBoxByConfigId(boxConfgiId);


                    // boxConfgiId = 402;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 402;
                    // player.GainBoxByConfigId(boxConfgiId);

                    // boxConfgiId = 401;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 401;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 401;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 401;
                    // player.GainBoxByConfigId(boxConfgiId);

                    // for (int i = 0; i < 100; i++)
                    // {
                    //     //通用
                    //     boxConfgiId = 1;
                    //     
                    //     
                    //     // //获得道具
                    //     // boxConfgiId = 301;
                    //     //获得队友
                    //     // boxConfgiId = 401;
                    //
                    //     // //队友上限数目测试
                    //     // boxConfgiId = 403;
                    //     // player.GainBoxByConfigId(boxConfgiId);
                    //
                    //     //人口测试
                    //     //boxConfgiId = 1001;
                    //     player.GainBoxByConfigId(boxConfgiId);
                    // }


                    // for (int i = 0; i < 100; i++)
                    // {
                    //     boxConfgiId = 901;
                    //     player.GainBoxByConfigId(boxConfgiId);
                    // }

                    // player.GainBoxByConfigId(boxConfgiId);
                    // boxConfgiId = 901;
                    // player.GainBoxByConfigId(boxConfgiId);
                    // player.GainBoxByConfigId(boxConfgiId);

                    //player item test
                    // var item = battleItemMgr.GenerateItem(100101);
                    // player.GainItem(item);
                    // item = battleItemMgr.GenerateItem(100102);
                    // player.GainItem(item);

                    // //entity item test
                    // item = battleItemMgr.GenerateItem(100103);
                    // player.entity.GainItem(item);
                    // item = battleItemMgr.GenerateItem(100102);
                    // player.entity.GainItem(item);
                    // item = battleItemMgr.GenerateItem(100103);
                    // player.entity.GainItem(item);

                    // item = battleItemMgr.GenerateItem(100102);
                    // player.GainItem(item);
                }
            }
        }
    }
}