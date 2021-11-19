using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGamePVPPanel: SettingsCategoryPanel
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
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.PointModeEnabled, "Point mode",
                tooltip: "End game after player or team reaches certain number of points.");
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.PointModeAmount, "Point amount", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.BombModeEnabled, "Bomb mode");
            ElementFactory.CreateToggleGroupSetting(DoublePanelLeft, style, settings.TeamMode, "Team mode", new string[] { "Off", "No sort", "Size lock", "Skill lock" });
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.InfectionModeEnabled, "Infection mode");
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.InfectionModeAmount, "Starting titans", elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, style, settings.BladePVP, "Blade/AHSS PVP", new string[] { "Off", "Teams", "FFA" });
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.FriendlyMode, "Friendly mode",
                tooltip: "Prevent normal AHSS/Blade PVP.");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AHSSAirReload, "AHSS air reload");
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.CannonsFriendlyFire, "Cannons friendly fire");
        }
    }
}
