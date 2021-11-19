using System;
using UnityEngine;
using Settings;

namespace GameProgress
{
    class AchievmentContainer: BaseSettingsContainer
    {
        public ListSetting<AchievmentItem> AchievmentItems = new ListSetting<AchievmentItem>();

        public AchievmentCount GetAchievmentCount()
        {
            AchievmentCount count = new AchievmentCount();
            foreach (AchievmentItem item in AchievmentItems.Value)
            {
                if (item.Tier.Value == "Bronze")
                {
                    count.TotalBronze++;
                    if (item.Finished())
                        count.FinishedBronze++;
                }
                else if (item.Tier.Value == "Silver")
                {
                    count.TotalSilver++;
                    if (item.Finished())
                        count.FinishedSilver++;
                }
                else if (item.Tier.Value == "Gold")
                {
                    count.TotalGold++;
                    if (item.Finished())
                        count.FinishedGold++;
                }
            }
            count.TotalAll = count.TotalBronze + count.TotalSilver + count.TotalGold;
            count.FinishedAll = count.FinishedBronze + count.FinishedSilver + count.FinishedGold;
            return count;
        }
    }
}
