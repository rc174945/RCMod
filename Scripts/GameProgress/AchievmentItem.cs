using Settings;

namespace GameProgress
{
    class AchievmentItem : QuestItem
    {
        public StringSetting Tier = new StringSetting(string.Empty);
        public BoolSetting Active = new BoolSetting(false);
    }
}
