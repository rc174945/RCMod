using System;
using UnityEngine;
using Settings;

namespace GameProgress
{
    class GameProgressContainer: SaveableSettingsContainer
    {
        protected override string FolderPath { get { return Application.dataPath + "/UserData/GameProgress"; } }
        protected override string FileName { get { return "GameProgress"; } }
        protected override bool Encrypted => true;

        public AchievmentContainer Achievment = new AchievmentContainer();
        public QuestContainer Quest = new QuestContainer();
        public GameStatContainer GameStat = new GameStatContainer();

        public override void Save()
        {
            Quest.CollectRewards();
            base.Save();
        }

    }
}
