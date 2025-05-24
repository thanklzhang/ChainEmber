using System.Collections.Generic;

namespace Config
{
    public static class MainTaskChapter_Tool
    {
        public static List<int> GetStageIdsByChapterId(int chapterId)
        {
            List<int> list = new List<int>();
            var chapter = Config.ConfigManager.Instance.GetById<Config.MainTaskChapter>(chapterId);
            if (chapter != null)
            {
                var stageList = chapter.StageList;
                foreach (var idStr in stageList)
                {
                    var id = idStr;
                    list.Add(id);
                }
            }
            return list;
        }
    }
}