using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGameMiscPanel : SettingsCategoryPanel
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
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanPerWavesEnabled, "Custom titans/wave");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanPerWaves, "Titan amount", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanMaxWavesEnabled, "Custom max waves");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanMaxWaves, "Wave amount", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.EndlessRespawnEnabled, "Endless respawn");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.EndlessRespawnTime, "Respawn time", elementWidth: inputWidth);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.PunksEveryFive, "Punks every 5 waves");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.GlobalMinimapDisable, "Global minimap disable");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.PreserveKDR, "Preserve KDR",
                tooltip: "Preserve player stats when they leave and rejoin the room.");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.RacingEndless, "Endless racing",
                tooltip: "Racing round continues even if someone finishes.");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.KickShifters, "Kick shifters");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowHorses, "Allow horses");
            ElementFactory.CreateInputSetting(DoublePanelRight, new ElementStyle(titleWidth: 160f, themePanel: ThemePanel), settings.Motd, "MOTD",
                elementWidth: 200f);
        }
    }
}
