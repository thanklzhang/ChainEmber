using System.Collections.Generic;

namespace Battle
{
    public class ListTool
    {
        //浅拷贝 List
        public static List<T> CopyList<T>(List<T> list)
        {
            if (null == list || 0 == list.Count)
            {
                return null;
            }
            
            List<T> newList = new List<T>();
            foreach (var item in list)
            {
                newList.Add(item);
            }

            return newList;
        }
    }
}