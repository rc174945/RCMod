using UnityEngine;
using System;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using System.Linq;

namespace GameProgress
{
    class AchievmentHandler : QuestHandler
    {
        private AchievmentContainer _achievment;

        public AchievmentHandler(AchievmentContainer achievment): base(null)
        {
            _achievment = achievment;
            ReloadAchievments();
        }

        public void ReloadAchievments()
        {
            LoadAchievments();
            CacheActiveAchievments();
        }

        private void LoadAchievments()
        {
            ListSetting<AchievmentItem> finalAchievmentItems = new ListSetting<AchievmentItem>();
            Dictionary<string, AchievmentItem> currentAchievmentDict = new Dictionary<string, AchievmentItem>();
            foreach (AchievmentItem item in _achievment.AchievmentItems.Value)
            {
                currentAchievmentDict.Add(item.GetQuestName(), item);
            }
            AchievmentContainer defaultAchievment = new AchievmentContainer();
            defaultAchievment.DeserializeFromJsonString(((TextAsset)AssetBundleManager.MainAssetBundle.Load("AchievmentList")).text);
            foreach (AchievmentItem item in defaultAchievment.AchievmentItems.Value)
            {
                if (currentAchievmentDict.ContainsKey(item.GetQuestName()))
                {
                    AchievmentItem current = currentAchievmentDict[item.GetQuestName()];
                    item.Progress.Value = current.Progress.Value;
                }
                item.Active.Value = false;
                finalAchievmentItems.Value.Add(item);
            }
            _achievment.AchievmentItems.Copy(finalAchievmentItems);
        }

        private void CacheActiveAchievments()
        {
            _activeQuests.Clear();
            Dictionary<string, List<AchievmentItem>> achievmentCategories = new Dictionary<string, List<AchievmentItem>>();
            foreach (AchievmentItem item in _achievment.AchievmentItems.Value)
            {
                string id = item.Category.Value + item.GetConditionsHash();
                if (!achievmentCategories.ContainsKey(id))
                    achievmentCategories.Add(id, new List<AchievmentItem>());
                achievmentCategories[id].Add(item);
            }
            foreach (string category in achievmentCategories.Keys)
            {
                List<AchievmentItem> items = achievmentCategories[category].OrderBy(x => x.GetQuestName()).ToList();
                AchievmentItem activeItem = null;
                foreach (AchievmentItem item in items)
                {
                    if (item.Progress.Value < item.Amount.Value)
                    {
                        activeItem = item;
                        break;
                    }
                }
                if (activeItem == null)
                    continue;
                activeItem.Active.Value = true;
                AddActiveQuest(activeItem);
            }
        }
    }

    public enum AchievmentTier
    {
        Bronze,
        Silver,
        Gold
    }
}
