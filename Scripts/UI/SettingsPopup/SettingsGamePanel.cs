using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGamePanel: SettingsCategoryPanel
    {
        protected override bool CategoryPanel => true;
        protected override string DefaultCategoryPanel => "Titans";

        public void CreateGategoryDropdown(Transform panel, bool includeReset = true, float elementWidth = 260f)
        {
            ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            string[] categories = new string[] { "Titans", "PVP", "Misc", "Weather" };
            ElementFactory.CreateDropdownSetting(panel, style, _currentCategoryPanelName, "Category", categories, elementWidth: elementWidth,
                                                 onDropdownOptionSelect: () => RebuildCategoryPanel());
            if (includeReset)
            {
                GameObject group = ElementFactory.CreateHorizontalGroup(panel, 0f, TextAnchor.MiddleRight);
                ElementFactory.CreateDefaultButton(group.transform, style, "Reset to default", onClick: () => OnResetButtonClick());
                CreateHorizontalDivider(panel);
            }
        }

        protected void OnResetButtonClick()
        {
            SettingsManager.LegacyGameSettingsUI.SetDefault();
            RebuildCategoryPanel();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Titans", typeof(SettingsGameTitansPanel));
            _categoryPanelTypes.Add("PVP", typeof(SettingsGamePVPPanel));
            _categoryPanelTypes.Add("Misc", typeof(SettingsGameMiscPanel));
            _categoryPanelTypes.Add("Weather", typeof(SettingsGameWeatherPanel));
        }
    }
}
