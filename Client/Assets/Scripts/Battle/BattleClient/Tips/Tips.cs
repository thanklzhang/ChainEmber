namespace Battle_Client
{
    public class Tips
    {
        public static void ShowSkillTipText(string str)
        {
            EventDispatcher.Broadcast(EventIDs.OnSkillTips,str);
        }
    }
}