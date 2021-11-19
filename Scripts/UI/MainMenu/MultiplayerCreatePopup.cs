using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    class MultiplayerCreatePopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "MultiplayerCreatePopup", "Title");
        protected override float Width => 800f;
        protected override float Height => 500f;
        protected override bool DoublePanel => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            MultiplayerGameSettings settings = SettingsManager.MultiplayerGameSettings;
            string cat = "MainMenu";
            string sub1 = "MultiplayerCreatePopup";
            string sub2 = "SingleplayerPopup";
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle dropdownStyle = new ElementStyle(themePanel: ThemePanel);
            ElementStyle inputStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Start"),
                onClick: () => OnButtonClick("Start"));
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"),
                onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, settings.Map, UIManager.GetLocaleCommon("Map"),
                GetMapOptions(), elementWidth: 180f, optionsWidth: 360f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, SettingsManager.WeatherSettings.WeatherSets.GetSelectedSetIndex(),
                UIManager.GetLocale(cat, sub2, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
            ElementFactory.CreateToggleGroupSetting(DoublePanelLeft, dropdownStyle, settings.Difficulty, UIManager.GetLocale(cat, sub2, "Difficulty"),
                UIManager.GetLocaleArray(cat, sub2, "DifficultyOptions"));
            ElementFactory.CreateInputSetting(DoublePanelRight, inputStyle, settings.Name, UIManager.GetLocale(cat, sub1, "ServerName"), elementWidth: 200f);
            ElementFactory.CreateInputSetting(DoublePanelRight, inputStyle, settings.Password, UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
            ElementFactory.CreateInputSetting(DoublePanelRight, inputStyle, settings.MaxPlayers, UIManager.GetLocale(cat, sub1, "MaxPlayers"), elementWidth: 200f);
            ElementFactory.CreateInputSetting(DoublePanelRight, inputStyle, settings.MaxTime, UIManager.GetLocale(cat, sub1, "MaxTime"), elementWidth: 200f);
        }

        // temporary until map selection system is reworked
        private string[] GetMapOptions()
        {
            LevelInfo.Init();
            int[] mapOptionIndices = new int[] { 0, 3, 4, 5, 17, 6, 7, 8, 9, 10, 11, 19, 20, 21, 22, 23, 25, 26 }; 
            List<string> mapOptions = new List<string>();
            foreach (int index in mapOptionIndices)
                mapOptions.Add(LevelInfo.levels[index].name);
            return mapOptions.ToArray();
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                Hide();
            else if (name == "Start")
                StartMultiplayer();
        }

        private void StartMultiplayer()
        {
            MultiplayerGameSettings settings = SettingsManager.MultiplayerGameSettings;
            string name = settings.Name.Value;
            int maxPlayers = settings.MaxPlayers.Value;
            int maxTime = settings.MaxTime.Value;
            string map = settings.Map.Value;
            string difficulty = "normal";
            if (settings.Difficulty.Value == (int)GameDifficulty.Abnormal)
                difficulty = "abnormal";
            else if (settings.Difficulty.Value == (int)GameDifficulty.Hard)
                difficulty = "hard";
            string day = "day";
            string password = settings.Password.Value;
            if (password.Length > 0)
            {
                password = new SimpleAES().Encrypt(password);
            }
            PhotonNetwork.CreateRoom(string.Concat(new object[] { name, "`", map, "`", difficulty, "`", maxTime, "`", day, "`", password, "`", UnityEngine.Random.Range(0, 0xc350) }), true, true, maxPlayers);
        }
    }
}
