using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsCustomMapPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "CustomMap";
            LegacyGameSettings settings = SettingsManager.LegacyGameSettingsUI;
            ElementStyle defaultStyle = new ElementStyle(fontSize: 24, titleWidth: 200f, themePanel: ThemePanel);
            ElementStyle inputStyle = new ElementStyle(fontSize: 24, titleWidth: 120f, themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, defaultStyle, "Map script");
            ElementFactory.CreateInputSetting(DoublePanelLeft, inputStyle, settings.LevelScript, string.Empty, elementWidth: 420f, elementHeight: 300f,
                multiLine: true);
            GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 0f, TextAnchor.UpperCenter);
            ElementFactory.CreateDefaultButton(group.transform, buttonStyle, "Clear", onClick: () => OnCustomMapButtonClick("ClearMap"));
            string[] options = new string[] { "Survive", "Waves", "PVP", "Racing", "Custom" };
            ElementFactory.CreateDropdownSetting(DoublePanelRight, defaultStyle, settings.GameType, "Game mode", options,
                elementWidth: 140f);
            ElementFactory.CreateInputSetting(DoublePanelRight, defaultStyle, settings.TitanSpawnCap, "Titan cap");
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateDefaultLabel(DoublePanelRight, defaultStyle, "Logic script");
            ElementFactory.CreateInputSetting(DoublePanelRight, inputStyle, settings.LogicScript, string.Empty, elementWidth: 420f, elementHeight: 300f,
                multiLine: true);
            group = ElementFactory.CreateHorizontalGroup(DoublePanelRight, 0f, TextAnchor.UpperCenter);
            ElementFactory.CreateDefaultButton(group.transform, buttonStyle, "Clear", onClick: () => OnCustomMapButtonClick("ClearLogic"));
        }

        private void OnCustomMapButtonClick(string name)
        {
            if (name == "ClearMap")
                SettingsManager.LegacyGameSettingsUI.LevelScript.Value = string.Empty;
            else if (name == "ClearLogic")
                SettingsManager.LegacyGameSettingsUI.LogicScript.Value = string.Empty;
            Parent.RebuildCategoryPanel();
        }
    }
}
