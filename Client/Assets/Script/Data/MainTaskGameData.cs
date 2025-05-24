using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{

    public class MainTaskChapterData
    {
        public int id;
        public MainTaskChapterState state;
        List<MainTaskStageData> stageList;
        Dictionary<int, MainTaskStageData> stageDic;

        public void SetStageList(List<MainTaskStageData> stageList)
        {
            this.stageList = stageList;
            this.stageDic = this.stageList.ToDictionary((data) => data.id);
        }

        public MainTaskStageData GetStageById(int stageId)
        {
            if (stageDic.ContainsKey(stageId))
            {
                return stageDic[stageId];
            }
            else
            {
                return null;
            }
        }

        internal void SetStageState(int stageId, MainTaskStageState state)
        {
            var stage = GetStageById(stageId);
            if (null == stage)
            {
                Logx.LogWarning("FinishOneStage : the stage(mainTask) is not found : " + stageId);
                return;
            }
            stage.state = state;
        }

        internal List<MainTaskStageData> GetStageList()
        {
            return stageList;
        }
    }

    public class MainTaskStageData
    {
        public int id;
        public MainTaskStageState state;
    }

    public class MainTaskGameData : BaseGameData
    {
        //public int flag;//待定
        List<MainTaskChapterData> chapterList = new List<MainTaskChapterData>();
        Dictionary<int, MainTaskChapterData> chapterDic = new Dictionary<int, MainTaskChapterData>();

        public List<MainTaskChapterData> ChapterList { get => chapterList; set => chapterList = value; }
        public Dictionary<int, MainTaskChapterData> ChapterDic { get => chapterDic; }

        public void SetMainTaskData(List<MainTaskChapterData> chapterList)
        {
            //增删改查
            this.chapterList = chapterList;
            this.chapterDic = this.chapterList.ToDictionary(hero => hero.id);
            //发事件：所有主线信息同步
        }

        public void SetStageState(int chapterId, int stageId, MainTaskStageState state)
        {
            var chapter = GetChapterById(chapterId);
            if (null == chapter)
            {
                Logx.LogWarning("FinishStage : the chapter(mainTask) is not found : " + chapterId);
                return;
            }
            chapter.SetStageState(stageId, state);
            //发事件：章节中的关卡状态更新了
        }

        public MainTaskChapterData GetChapterById(int id)
        {
            if (ChapterDic.ContainsKey(id))
            {
                return ChapterDic[id];
            }
            else
            {
                return null;
            }

        }

        public int GetCurrChapterId()
        {
            int chapterId = 0;

            foreach (var chapterData in this.chapterList)
            {
                if (chapterData.state == MainTaskChapterState.NoFinish)
                {
                    chapterId = chapterData.id;
                    break;
                }
            }

            return chapterId;
        }

        public bool IsChapterUnlock(int chapterId)
        {
            var chapterData = GetChapterById(chapterId);
            return chapterData != null;
        }

        //public bool IsChapterFinish(int chapterId)
        //{
        //    var chapterData = GetChapterById(chapterId);
        //    if (chapterData != null)
        //    {
        //        return chapterData.state == MainTaskChapterState.HasFinish || chapterData.state == MainTaskChapterState.HasReceive;
        //    }
        //    return false;
        //}

        public MainTaskStageData GetStageData(int chapterId, int stageId)
        {
            MainTaskStageData resultStage = null;
            var chapterData = GetChapterById(chapterId);
            if (chapterData != null)
            {
                var stage = chapterData.GetStageById(stageId);
                if (stage != null && stage.id == stageId)
                {
                    resultStage = stage;
                }
            }

            return resultStage;
        }

        public bool IsStageUnlock(int chapterId, int stageId)
        {
            var chapterData = GetChapterById(chapterId);
            if (chapterData != null)
            {
                var stage = chapterData.GetStageById(stageId);
                if (stage != null && stage.id == stageId)
                {
                    return true;
                }
            }

            return false;
        }

        //public bool IsStageFinish(int chapterId, int stageId)
        //{
        //    var chapterData = GetChapterById(chapterId);
        //    if (chapterData != null)
        //    {
        //        var stage = chapterData.GetStageById(stageId);
        //        if (stage != null)
        //        {
        //            if (stage.state == MainTaskStageState.HasFinish || stage.state == MainTaskStageState.HasReceive)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public bool IsStageHasReceive(int chapterId, int stageId)
        //{
        //    var chapterData = GetChapterById(chapterId);
        //    if (chapterData != null)
        //    {
        //        var stage = chapterData.GetStageById(stageId);
        //        if (stage != null)
        //        {
        //            if (stage.state == MainTaskStageState.HasReceive)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        public int GetCurrStageIdByChapter(int chapterId)
        {
            int stageId = 0;
            List<MainTaskStageData> list = new List<MainTaskStageData>();
            var chapter = GetChapterById(chapterId);
            if (chapter != null)
            {
                list = chapter.GetStageList();

            }

            foreach (var item in list)
            {
                if (item.state == MainTaskStageState.NoFinish)
                {
                    stageId = item.id;
                    break;
                }
            }
            return stageId;
        }

        public List<MainTaskStageData> GetStageListByChapterId(int chapterId)
        {
            List<MainTaskStageData> list = new List<MainTaskStageData>();
            var chapter = GetChapterById(chapterId);
            if (chapter != null)
            {
                list = chapter.GetStageList();
            }
            return list;

        }
    }

}
