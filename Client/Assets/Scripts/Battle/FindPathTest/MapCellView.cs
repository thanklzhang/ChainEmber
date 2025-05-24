using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MapCellView : MonoBehaviour
{
    Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        SetTempShader();
    }

    // Update is called once per frame
    void Update()
    {

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

    Map map;
    List<Battle.Pos> currPosList = new List<Battle.Pos>();
    public void SetMap(Map map)
    {
        this.map = map;
    }
    public void SetRenderPath(List<Battle.Pos> currPosList)
    {
        this.currPosList = currPosList;
    }

    void OnRenderObject()
    {
        List<List<PathNode>> mapNodes = null;

        //TODO : 兼容本地和远端战斗
        if (Battle_Client.BattleManager.Instance.IsLocalBattle())
        {
            var battle = Battle_Client.BattleManager.Instance.GetBattle();
            var player0 = battle.FindPlayerByPlayerIndex(0);
            var entityGuid = player0.ctrlHeroGuid;
            var entity = battle.FindEntity(entityGuid);
            if (null == entity)
            {
                return;
            }

            BaseAI ai = null;//(Battle.PlayerAI)battle.FindAI(entity.guid);
            if (null == ai)
            {
                return;
            }
            SetRenderPath(entity.GetCurrPathPosList());
            mapNodes = map.mapNodes;
        }
        else
        {
            //test
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

            map.Init(mapInfo,new List<Battle.Vector3>(),new List<Battle.Vector3>());

            mapNodes = map.mapNodes;
        }
      
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

                if (currPosList.Exists((n) =>
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

                Vector3 pos0 = new Vector3(node.x, 0, node.y);
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
            var pos = new Vector3(i * 1, 0, 0);
            var pos2 = new Vector3(i * 1, 0, mapNodes[0].Count * 1);
            GL.Vertex(pos);
            GL.Vertex(pos2);
        }

        for (int i = 0; i < mapNodes[0].Count + 1; i++)
        {
            var pos = new Vector3(0, 0, i);
            var pos2 = new Vector3(mapNodes.Count, 0, i);
            GL.Vertex(pos);
            GL.Vertex(pos2);
        }

        GL.End();

        GL.PopMatrix();
    }

}
