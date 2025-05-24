using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITest : MonoBehaviour
{
    public TestEntity entity;
    
    bool isStart;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        var randPos = GetRandPos();
        this.transform.position = this.entity.GetPosCenter(randPos);
        isStart = true;
    }

    Vector3 GetRandPos()
    {
        var r = FindPathTest.instance.rand;
        var mapWidth = FindPathTest.instance.GetMapWidth();
        var mapHeight = FindPathTest.instance.GetMapHeight();
        a = a + 1;
        var startPos = this.transform.position;
        int x = 0;
        int y = 0;
        int t = 0;
        while (true)
        {
            t += 1;
            if (t >= 10000)
            {
                Logx.LogError("perhaps die loop !!!");
                break;
            }

            var randX = Battle.BattleRandom.Next(0, mapWidth - 1, r);
            var randY = Battle.BattleRandom.Next(0, mapHeight - 1, r);

            //Logx.Log("rand x : " + randX + " randY : " + randY);
            
            var isObstacle = FindPathTest.instance.IsStaticObstacle(randX, randY);
            if (!isObstacle)
            {
                x = randX;
                y = randY;
                break;
            }
        }
        var endPos = new Vector3(x, 0, y);
        return endPos;
    }

    int a = 0;
    void StartFind()
    {
        //a = a + 1;
        //var startPos = this.transform.position;
        //int x = 0;
        //int y = 0;
        //int t = 0;
        //while (true)
        //{
        //    t += 1;
        //    if (t >= 10000)
        //    {
        //        Logx.LogError("perhaps die loop !!!");
        //        break;
        //    }
        //    int xx = 0;
        //    int yy = 0;
        //    if (0 == a % 2)
        //    {
        //        xx = 0;
        //        yy = 0;
        //    }
        //    else
        //    {
        //        xx = 4;
        //        yy = 4;
        //    }
        //    var randX = Battle.MyRandom.Next(xx, xx, r);
        //    var randY = Battle.MyRandom.Next(yy, yy, r);

        //    var isObstacle = FindPathTest.instance.IsStaticObstacle(randX, randY);
        //    if (!isObstacle)
        //    {
        //        x = randX;
        //        y = randY;
        //        break;
        //    }
        //}
        //var endPos = new Vector3(x, 0, y);


        var startPos = this.transform.position;
        var endPos = GetRandPos();

        var pos0 = new Battle.Pos()
        {
            x = (int)startPos.x,
            y = (int)startPos.z
        };

        var pos1 = new Battle.Pos()
        {
            x = (int)endPos.x,
            y = (int)endPos.z
        };
        FindPathTest.instance.FindPath(this.entity, pos0, pos1, null);
    }

    float timer;
    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            return;
        }
        if (!entity.isMove)
        {
            StartFind();
        }
    }
}
