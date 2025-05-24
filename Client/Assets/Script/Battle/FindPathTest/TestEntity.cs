using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntity : MonoBehaviour
{
    public List<Pos> currPosList = new List<Pos>();
    public int currIndex;
    public bool isMove;
    public float speed = 3;

    Map map;

    //是否在碰撞等待中
    public bool isCollisionWaiting;
    float collisionWaitTimer;
    float collisionWaitMaxTime = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove)
        {
            return;
        }

        if (isCollisionWaiting)
        {
            collisionWaitTimer += Time.deltaTime;
            if (collisionWaitTimer >= collisionWaitMaxTime)
            {
                collisionWaitTimer = 0;
                isCollisionWaiting = false;
                FindPath();
            }
            return;
        }

        //按照路径走
        if (currIndex < currPosList.Count)
        {
            var targetPos = new UnityEngine.Vector3();
            var pos = currPosList[currIndex];
            targetPos = GetPosCenter(pos);

            var currPos = this.gameObject.transform.position;
            var moveVector = targetPos - this.gameObject.transform.position;

            var dir = moveVector.normalized;
            var currFramePos = currPos;
            //预测下一帧位置
            var moveDelta = dir * speed * Time.deltaTime;
            var nextFramePos = currPos + moveDelta;

            var dotValue = UnityEngine.Vector3.Dot(currFramePos - targetPos, targetPos - nextFramePos);
            if (dotValue >= 0 || moveVector.sqrMagnitude < 0.001f)
            {
                //到达
                //Logx.Log("arrive : currIndex : " + currIndex + " targetPos : " + "(" + targetPos.x + "," + targetPos.z + ")");
                this.transform.position = targetPos;
                currIndex = currIndex + 1;
            }
            else
            {
                //移动
                moveDelta = dir * speed * Time.deltaTime;
                this.gameObject.transform.position = currPos + moveDelta;


                //碰撞检测
                var playerList = FindPathTest.instance.playerList;
                var currPosCenter = currPos;
                TestEntity collisionPlayer = null;
                foreach (var player in playerList)
                {
                    if (player == this)
                    {
                        continue;
                    }

                    //判断前方射线
                    var rayLength = 0.71f;
                    var explorePos = currPosCenter + dir * rayLength;
                    float exploreEndR = 0.6f;

                    var isRayCollision = IsCollisionCircle(explorePos, 0, player.transform.position, exploreEndR);
                    if (isRayCollision)
                    {
                        collisionPlayer = player;
                        break;
                    }
                }

                var isCollision = collisionPlayer != null;
                if (isCollision)
                {
                    if (!collisionPlayer.isCollisionWaiting)
                    {
                        isCollisionWaiting = true;
                    }
                    else
                    {
                        FindPath();
                    }
                }
            }
        }
        else
        {
            isMove = false;
        }
    }

    public void FindPath()
    {
        var playerList = FindPathTest.instance.playerList;
        var currPos = this.gameObject.transform.position;
        var intCurPos = GetIntPos(currPos);

        var aIntPos = intCurPos;
        List<Pos> dynamicList = new List<Pos>();
        foreach (var player2 in playerList)
        {
            if (player2 == this)
            {
                continue;
            }

            var bPos = player2.transform.position;
            var bIntPos = new Pos()
            {
                x = (int)bPos.x,
                y = (int)bPos.z
            };

            var xLen = Math.Abs(aIntPos.x - bIntPos.x);
            var yLen = Math.Abs(aIntPos.y - bIntPos.y);

            if (xLen + yLen <= 2)
            {
                dynamicList.Add(bIntPos);
            }
        }

        var endPos = this.currPosList[this.currPosList.Count - 1];

        //Logx.Log("collision , change next path : start:" + GetPosStr(aIntPos) + " end:" +
        //    GetPosStr(endPos) + " dynamicList.Count : " + dynamicList.Count);

        FindPathTest.instance.FindPath(this, aIntPos, endPos, dynamicList);
    }


    bool IsCollisionCircle(UnityEngine.Vector3 pos0, float r0, UnityEngine.Vector3 pos1, float r1)
    {
        var centerLength = (pos0 - pos1).sqrMagnitude;
        var rLengh = (r0 + r1) * (r0 + r1);
        return centerLength < rLengh;
    }

    public UnityEngine.Vector3 GetPosCenter(int x, int y)
    {
        var v = new UnityEngine.Vector3(x, 0, y);
        float width = 0.5f;
        float height = 0.5f;
        return v + new UnityEngine.Vector3(width, 0, height);
    }

    public UnityEngine.Vector3 GetPosCenter(Pos pos)
    {
        return GetPosCenter(pos.x, pos.y);
    }

    public UnityEngine.Vector3 GetPosCenter(UnityEngine.Vector3 pos)
    {
        return GetPosCenter((int)pos.x, (int)pos.z);
    }


    public Pos GetIntPos(UnityEngine.Vector3 vec)
    {
        var x = (int)vec.x;
        //var y = (int)vec.y;
        var z = (int)vec.z;

        Pos p = new Pos()
        {
            x = x,
            y = z
        };

        return p;

    }

    public List<Pos> cachePosList = new List<Pos>();

    public string GetPosStr(UnityEngine.Vector3 pos)
    {
        return ("(" + pos.x + "," + pos.z + ")");
    }

    public string GetPosStr(Pos pos)
    {
        return ("(" + pos.x + "," + pos.y + ")");
    }

    public void StartMove(List<Pos> posList)
    {
        var str = "";
        foreach (var item in posList)
        {
            str = str + ("(" + item.x + ",") + ("" + item.y + ")");
        }
        //Logx.Log("start move poslist : " + str);
        //Logx.Log("start move : currPos : " + GetPosStr(this.transform.position));

        this.currPosList = posList;
        isMove = true;
        currIndex = 0;

    }
}
