using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    class SingleplayerPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "SingleplayerPopup", "Title");
        protected override float Width => 800f;
        protected override float Height => 510f;
        protected override bool DoublePanel => true;

        private string[] _characterOptions = new string[] {"Mikasa", "Levi", "Armin", "Marco", "Jean", "Eren", "Titan_Eren", "Petra", "Sasha", "Set 1", "Set 2", "Set 3"};
        private string[] _costumeOptions = new string[] { "Costume 1", "Costume 2", "Costume 3" };

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SingleplayerGameSettings settings = SettingsManager.SingleplayerGameSettings;
            string category = "MainMenu";
            string sub = "SingleplayerPopup";
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle dropdownStyle = new ElementStyle(themePanel: ThemePanel);
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Start"),
                onClick: () => OnButtonClick("Start"));
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"),
                onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, settings.Map, UIManager.GetLocaleCommon("Map"), 
                GetMapOptions(), elementWidth: 180f, optionsWidth: 360f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, SettingsManager.WeatherSettings.WeatherSets.SelectedSetIndex,
                UIManager.GetLocale(category, sub, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
            ElementFactory.CreateToggleGroupSetting(DoublePanelLeft, dropdownStyle, settings.Difficulty, UIManager.GetLocale(category, sub, "Difficulty"),
                UIManager.GetLocaleArray(category, sub, "DifficultyOptions"));
            ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.Character, UIManager.GetLocale(category, sub, "Character"),
                _characterOptions, elementWidth: 180f);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, dropdownStyle, settings.Costume, UIManager.GetLocale(category, sub, "Costume"),
                _costumeOptions);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, dropdownStyle, settings.CameraType, 
                UIManager.GetLocale(category, sub, "Camera"), RCextensions.EnumToStringArray<CAMERA_TYPE>());
        }

        // temporary until map selection system is reworked
        private string[] GetMapOptions()
        {
            LevelInfo.Init();
            int[] mapOptionIndices = new int[] { 15, 16, 12, 13, 14, 24, 18 }; 
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
                StartSinglePlayer();
        }

        private void StartSinglePlayer()
        {
            SingleplayerGameSettings settings = SettingsManager.SingleplayerGameSettings;
            IN_GAME_MAIN_CAMERA.difficulty = settings.Difficulty.Value;
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
            IN_GAME_MAIN_CAMERA.singleCharacter = settings.Character.Value.ToUpper();
            IN_GAME_MAIN_CAMERA.cameraMode = (CAMERA_TYPE)settings.CameraType.Value;
            CheckBoxCostume.costumeSet = settings.Costume.Value + 1;
            FengGameManagerMKII.level = settings.Map.Value;
            Application.LoadLevel(LevelInfo.getInfo(settings.Map.Value).mapName);
        }
    }
}
