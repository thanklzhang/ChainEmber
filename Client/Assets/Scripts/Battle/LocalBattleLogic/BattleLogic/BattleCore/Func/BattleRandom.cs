using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class BattleRandom
    {
        public static float Next(float a, float b, Random rand = null)
        {
            float c = b - a;
            rand = rand == null ? new Random() : rand;
            return (float)(rand.NextDouble() * c + a);
        }

        //[a,b)
        public static int Next(int a, int b, Random rand = null)
        {
            int c = b - a;
            int dir = Math.Sign(c);
            rand = rand == null ? new Random() : rand;
            var randNum = dir * rand.Next(dir * c);
            return randNum + a;
        }

        public static int GetNextIndexByWeights(List<int> weights, Random random = null)
        {
            var sum = weights.Sum(w => w);
            var currSum = 0;
            var rand = Next(0, sum, random);
            var currIndex = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                var w = weights[i];
                currSum += w;
                currIndex = i;
                if (rand < currSum)
                {
                    break;
                }
            }

            return currIndex;
        }

        //
        public static List<int> GetRandIndexes(int a, int b, int count, Battle battle)
        {
            List<int> indexList = new List<int>();

            List<int> tempList = new List<int>();
            for (int i = a; i < b; i++)
            {
                tempList.Add(i);
            }

            for (int i = 0; i < count; i++)
            {
                if (tempList.Count <= 0)
                {
                    break;
                }

                var randIndex = GetRandInt(0, tempList.Count, battle);
                indexList.Add(tempList[randIndex]);
                tempList.RemoveAt(randIndex);
            }

            return indexList;
        }

        public static List<BattleEntity> GetRandEntityList(List<BattleEntity> list, int a, int b, int count, Battle battle)
        {
            List<BattleEntity> newList = new List<BattleEntity>();
            if (0 == list.Count)
            {
                return newList;
            }

            var indexes = BattleRandom.GetRandIndexes(a, b, count, battle);
            foreach (var index in indexes)
            {
                newList.Add(list[index]);
            }

            return newList;
        }

        public static int GetRandInt(int a, int b, Battle battle)
        {
            return GetRandInt(a, b, battle.rand);
        }

        public static int GetRandInt(int a, int b, Random rand)
        {
            return BattleRandom.Next(a, b, rand);
        }

        public static float GetRandFloat(float a, float b, Battle battle)
        {
            return GetRandFloat(a, b, battle.rand);
        }

        public static float GetRandFloat(float a, float b, Random rand)
        {
            return BattleRandom.Next(a, b, rand);
        }

        public static Vector3 GetRectRandVector3(Vector3 min, Vector3 max, Battle battle)
        {
            return GetRectRandVector3(min, max, battle.rand);
        }

        public static Vector3 GetRectRandVector3(Vector3 min, Vector3 max, Random rand)
        {
            var x = GetRandFloat(min.x, max.x, rand);
            var y = GetRandFloat(min.y, max.y, rand);
            var z = GetRandFloat(min.z, max.z, rand);

            return new Vector3(x, y, z);
        }
    }
}