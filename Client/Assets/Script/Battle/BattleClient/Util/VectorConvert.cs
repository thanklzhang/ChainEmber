using System.Collections.Generic;

namespace Battle_Client
{
    public class VectorConvert
    {
        public static List<Battle.Vector3> ToVector3(List<float[]> _posList)
        {
            List<Battle.Vector3> pList = new List<Battle.Vector3>();
            ;
            for (int i = 0; i < _posList.Count; i++)
            {
                var p = _posList[i];

                pList.Add(ToVector3(p));
            }

            return pList;
        }

        public static Battle.Vector3 ToVector3(float[] _pos)
        {
            Battle.Vector3 pos = new Battle.Vector3(_pos[0], 0, _pos[1]);
            return pos;
        }
    }
}