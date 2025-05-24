using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Battle;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MapDataGenerateTool : MonoBehaviour
{
    public List<List<PathNode>> mapNodes;

    public Transform collisionRoot;
    // public Transform customPosRoot;
    public Transform enemyInitPosRoot;
    
    public Transform playerInitPosRoot;
    public int maxMapX = 100;
    public int maxMapZ = 100;

    Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        mapNodes = new List<List<PathNode>>();

        SetTempShader();
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

    public void ClearMap()
    {
        this.mapNodes?.Clear();
    }


    void OnDrawGizmos()
    {
        if (null == mapNodes)
        {
            mapNodes = new List<List<PathNode>>();
        }

        if (0 == mapNodes.Count)
        {
            return;
        }

        if (null == lineMaterial)
        {
            this.SetTempShader();
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

                if (node.nodeType == PathNodeType.Normal)
                {
                    color = new Color(0, 1, 0);
                }
                else if (node.nodeType == PathNodeType.Obstacle)
                {
                    color = new Color(0.3f, 0.3f, 0.3f);
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

    public void GenMapInfo()
    {
        MapSaveData mapData = new MapSaveData();
        
        //地图 list
        List<List<int>> list = new List<List<int>>();
        for (int i = 0; i < maxMapX; i++)
        {
            list.Add(new List<int>());
            for (int j = 0; j < maxMapZ; j++)
            {
                list[i].Add(0);
            }
        }

        List<Transform> collisions = collisionRoot.GetComponentsInChildren<Transform>().ToList();
        for (int i = 0; i < collisions.Count; i++)
        {
            var collision = collisions[i];
            var render = collision.GetComponent<MeshRenderer>();
            if (null == render)
            {
                continue;
            }

            var minX = (int)Mathf.Floor(collision.position.x - collision.localScale.x * 0.5f);
            var maxX = (int)Mathf.Floor(collision.position.x + collision.localScale.x * 0.5f);

            var minZ = (int)Mathf.Floor(collision.position.z - collision.localScale.z * 0.5f);
            var maxZ = (int)Mathf.Floor(collision.position.z + collision.localScale.z * 0.5f);


            for (int j = minX; j <= maxX; j++)
            {
                for (int k = minZ; k <= maxZ; k++)
                {
                    if (j >= 0 && j < maxMapX &&
                        k >= 0 && k < maxMapZ)
                    {
                        list[j][k] = 1;
                    }
                }
            }
        }

        mapData.mapList = list;
        
        // //自定义点集合
        // List<Transform> customPosList = enemyInitPosRoot.GetComponentsInChildrenExceptSelf<Transform>().ToList();
        // List<float[]>_customPosList = new List<float[]>();
        // for (int i = 0; i < customPosList.Count; i++)
        // {
        //     var pos = customPosList[i];
        //     float[] p = new float[2];
        //     p[0] = pos.position.x - 0.5f;
        //     p[1] = pos.position.z - 0.5f;
        //     
        //     _customPosList.Add(p);
        // }
        //
        // mapData.posList = _customPosList;

        mapData.enemyInitPosList = ParsePosList(enemyInitPosRoot);
       
        
        // //玩家初始位置集合
        // List<Transform> playerInitPosList = playerInitPosRoot.GetComponentsInChildrenExceptSelf<Transform>().ToList();
        // List<float[]>_playerInitPosList = new List<float[]>();
        // for (int i = 0; i < playerInitPosList.Count; i++)
        // {
        //     var pos = playerInitPosList[i];
        //     float[] p = new float[2];
        //     p[0] = pos.position.x - 0.5f;
        //     p[1] = pos.position.z - 0.5f;
        //     
        //     _playerInitPosList.Add(p);
        // }
        // mapData.playerInitPosList = _playerInitPosList;

        mapData.playerInitPosList = ParsePosList(playerInitPosRoot);
        
       
        var json = LitJson.JsonMapper.ToJson(mapData);

        var desName = GlobalConfig.buildPath + @"\Battle\BattleMap\map_info_temp";
      
        var currDesName = desName;
        for (int i = 0; i < 10000; i++)
        {
            if (File.Exists(currDesName + ".json"))
            {
                currDesName = desName + i;
            }
            else
            {
                break;
            }
        }

        var resultDesPath = currDesName + ".json";

        JsonTool.SaveJson(resultDesPath, json);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        mapNodes = new List<List<PathNode>>();

        for (int i = 0; i < list.Count; i++)
        {
            mapNodes.Add(new List<PathNode>());
            var count = list[i].Count;
            string str = "";
            for (int j = 0; j < count; j++)
            {
                mapNodes[i].Add(new PathNode()
                {
                    nodeType = (PathNodeType)list[i][j],
                    x = i,
                    y = j
                });
            }
        }
        Debug.Log("GenCollisionInfo");
    }
    
    public List<float[]> ParsePosList(Transform posRoot)
    {
        List<Transform> posTranList = posRoot.GetComponentsInChildrenExceptSelf<Transform>().ToList();
        List<float[]>_customPosList = new List<float[]>();
        for (int i = 0; i < posTranList.Count; i++)
        {
            var pos = posTranList[i];
            float[] p = new float[2];
            p[0] = pos.position.x - 0.5f;
            p[1] = pos.position.z - 0.5f;
            
            _customPosList.Add(p);
        }

        return _customPosList;
    }
}