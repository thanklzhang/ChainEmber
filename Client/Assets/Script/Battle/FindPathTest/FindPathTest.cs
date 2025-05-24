using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = Battle.Vector3;

//正常游戏流程启动
public class FindPathTest : MonoBehaviour
{
    public static FindPathTest instance;

    PathProxy pathProxy = new PathProxy();
    Map map;

    Material lineMaterial;

    public TestEntity player0;
    //public TestEntity player1;
    //public TestEntity player2;

    public List<TestEntity> playerList;
    void Awake()
    {
        instance = this;
    }

    List<Pos> currFindNodes = new List<Pos>();
    public System.Random rand;
    // Start is called before the first frame update
    void Start()
    {
        map = new Map();

        var mapInfo = new List<List<int>>
        {
            new List<int>(){ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            new List<int>(){ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            new List<int>(){ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,},
            new List<int>(){ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
        };

        //var mapInfo = new int[][]
        //{
        //    new int[]{ 0,0,0,0,0},
        //    new int[]{ 0,1,0,0,0},
        //    new int[]{ 0,0,0,0,0},
        //    new int[]{ 0,0,0,0,0},
        //    new int[]{ 0,0,0,0,0},

        //};

        int randSeek = unchecked((int)DateTime.Now.Ticks);
        rand = new System.Random(randSeek);


        map.Init(mapInfo,new List<Vector3>(),new List<Vector3>());

        pathProxy.Init(map);


        SetTempShader();

        player0 = playerList[0];

        player0.transform.position = new UnityEngine.Vector3(3.5f, 0, 3.5f);

        for (int i = 0; i < playerList.Count; i++)
        {
            var player = playerList[i];
            var aiComp = player.GetComponent<AITest>();
            if (aiComp != null)
            {
                aiComp.Init();
            }
        }

        //playerList = new List<TestEntity>();
        //playerList.Add(player0);
        //playerList.Add(player1);
        //playerList.Add(player2);

       

        //player1.transform.position = new Vector3(2f, 0, 1f);

        //test 
        //currFindNodes = pathProxy.Find(3, 0, 3, 1);
    }

    void SetTempShader()
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        //设置参数
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //设置参数
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        //设置参数
        lineMaterial.SetInt("_ZWrite", 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnityEngine.Vector3 resultPos;
            if (TryToGetRayTargetPos(out resultPos))
            {
                this.OnPlayerClickGround(resultPos);
            }
        }
    }

    public bool TryToGetRayTargetPos(out UnityEngine.Vector3 pos)
    {
        pos = UnityEngine.Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                var currHit = hits[i];
                var tag = currHit.collider.tag;
                if (tag == "Ground")
                {
                    //Logx.Log("hit ground : " + currHit.collider.gameObject.name);
                    pos = currHit.point;
                    return true;

                    //this.OnPlayerClickGround(currHit.point);
                }
            }
        }
        return false;
    }

    void OnPlayerClickGround(UnityEngine.Vector3 resultPos)
    {
        //Logx.Log("hit ground : " + resultPos);

        var startPos = GetMapPos(player0.transform.position);
        var endPos = GetMapPos(resultPos);

        var pos0 = new Pos()
        {
            x = (int)startPos.x,
            y = (int)startPos.z
        };

        var pos1 = new Pos()
        {
            x = (int)endPos.x,
            y = (int)endPos.z
        };
        FindPath(player0, pos0, pos1, null);

    }

    public List<Battle.Pos> FindPath(TestEntity player, Pos startPos, Pos endPos, List<Pos> dynamicList)
    {
        var start = startPos;
        //if (player.isMove)
        //{
        //    start = player.currPosList[player.currIndex];
        //}
        //var currFindNodes = pathProxy.Find((int)startPos.x, (int)startPos.y, (int)endPos.x, (int)endPos.y, dynamicList);
        var currFindNodes = pathProxy.Find((int)start.x, (int)start.y, (int)endPos.x, (int)endPos.y, dynamicList);

        if (currFindNodes.Count >= 2)
        {
            //移除本身 保留终点
            currFindNodes.RemoveAt(0);
        }
        player.StartMove(currFindNodes);
        return currFindNodes;
    }

    UnityEngine.Vector3 GetMapPos(UnityEngine.Vector3 resultPos)
    {
        var intPos = new UnityEngine.Vector3((int)resultPos.x, (int)resultPos.y, (int)resultPos.z);
        return intPos;
    }

    public bool IsStaticObstacle(int x, int y)
    {
        var mapNodes = map.mapNodes;
        return PathNodeType.Obstacle == mapNodes[x][y].nodeType;
    }

    public int GetMapWidth()
    {
        var mapNodes = map.mapNodes;
        return mapNodes.Count;
    }

    public int GetMapHeight()
    {
        var mapNodes = map.mapNodes;
        return mapNodes[0].Count;
    }

    void OnRenderObject()
    {
        var mapNodes = map.mapNodes;

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        //矩阵相乘，将物体坐标转化为世界坐标
        GL.MultMatrix(transform.localToWorldMatrix);

        //draw cell
        for (int x = 0; x < mapNodes.Count; x++)
        {
            for (int y = 0; y < mapNodes[x].Count; y++)
            {
                var node = mapNodes[x][y];

                GL.Begin(GL.QUADS);
                var color = new Color(0, 0, 1);

                if (player0.currPosList.Exists((n) =>
                 {
                     return n.x == node.x && n.y == node.y;
                 }))
                {
                    color = new Color(1, 0, 0);
                }
                //else if (player1.currPosList.Exists((n) =>
                //{
                //    return n.x == node.x && n.y == node.y;
                //}))
                //{
                //    color = new Color(1, 1, 1);
                //}
                else
                {
                    if (node.nodeType == PathNodeType.Normal)
                    {
                        color = new Color(0, 1, 0);
                    }
                    else if (node.nodeType == PathNodeType.Obstacle)
                    {
                        color = new Color(0, 0, 1);
                    }
                }


                GL.Color(color);

                UnityEngine.Vector3 pos0 = new UnityEngine.Vector3(node.x, 0, node.y);
                GL.Vertex(pos0);

                pos0.x += 1;
                GL.Vertex(pos0);

                pos0.z += 1;
                GL.Vertex(pos0);

                pos0.x -= 1;
                GL.Vertex(pos0);

                GL.End();

            }
        }

        //draw lines
        GL.Begin(GL.LINES);
        var color2 = new Color(0, 0, 0);
        GL.Color(color2);


        for (int i = 0; i < mapNodes.Count + 1; i++)
        {
            var pos = new UnityEngine.Vector3(i * 1, 0, 0);
            var pos2 = new UnityEngine.Vector3(i * 1, 0, mapNodes[0].Count * 1);
            GL.Vertex(pos);
            GL.Vertex(pos2);
        }

        for (int i = 0; i < mapNodes[0].Count + 1; i++)
        {
            var pos = new UnityEngine.Vector3(0, 0, i);
            var pos2 = new UnityEngine.Vector3(mapNodes.Count, 0, i);
            GL.Vertex(pos);
            GL.Vertex(pos2);
        }

        GL.End();

        GL.PopMatrix();
    }
}
