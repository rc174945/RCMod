using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGeneralPanel: SettingsCategoryPanel
    {
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "General";
            GeneralSettings settings = SettingsManager.GeneralSettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.Language, "Language", UIManager.GetLanguages(),
                elementWidth: 160f, onDropdownOptionSelect: () => settingsPopup.RebuildCategoryPanel(), tooltip: UIManager.GetLocaleCommon("RequireRestart"));
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.Volume, UIManager.GetLocale(cat, sub, "Volume"),
                elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.MouseSpeed, UIManager.GetLocale(cat, sub, "MouseSpeed"),
               elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraDistance, UIManager.GetLocale(cat, sub, "CameraDistance"),
               elementWidth: 135f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.InvertMouse, UIManager.GetLocale(cat, sub, "InvertMouse"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CameraTilt, UIManager.GetLocale(cat, sub, "CameraTilt"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.MinimapEnabled, UIManager.GetLocale(cat, sub, "MinimapEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsEnabled, UIManager.GetLocale(cat, sub, "SnapshotsEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsShowInGame, UIManager.GetLocale(cat, sub, "SnapshotsShowInGame"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapshotsMinimumDamage, UIManager.GetLocale(cat, sub, "SnapshotsMinimumDamage"),
                elementWidth: 100f);
        }
    }
}
