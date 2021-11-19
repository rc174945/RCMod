using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGameTitansPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 20f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsGamePanel gamePanel = (SettingsGamePanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)gamePanel.Parent;
            gamePanel.CreateGategoryDropdown(DoublePanelLeft);
            float inputWidth = 120f;
            ElementStyle style = new ElementStyle(titleWidth: 240f, themePanel: ThemePanel);
            LegacyGameSettings settings = SettingsManager.LegacyGameSettingsUI;
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanNumberEnabled, "Custom titan number");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanNumber, "Titan amount", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanSpawnEnabled, "Custom titan spawns",
                tooltip: "Spawn rates must add up to 100.");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnNormal, "Normal", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnAberrant, "Aberrant", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnJumper, "Jumper", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnCrawler, "Crawler", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnPunk, "Punk", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanSizeEnabled, "Custom titan sizes");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSizeMin, "Minimum size", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSizeMax, "Maximum size", elementWidth: inputWidth);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, style, settings.TitanHealthMode, "Titan health", new string[] { "Off", "Fixed", "Scaled" });
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanHealthMin, "Minimum health", elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanHealthMax, "Maximum health", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.TitanArmorEnabled, "Titan armor");
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanArmor, "Armor amount", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.TitanExplodeEnabled, "Titan explode");
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanExplodeRadius, "Explode radius", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.RockThrowEnabled, "Punk rock throwing");
        }
    }
}
