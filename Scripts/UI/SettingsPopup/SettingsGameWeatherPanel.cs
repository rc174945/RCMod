using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI
{
    class SettingsGameWeatherPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 20f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsGamePanel gamePanel = (SettingsGamePanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)gamePanel.Parent;
            gamePanel.CreateGategoryDropdown(DoublePanelLeft, false, 205f);
            ElementStyle style = new ElementStyle(titleWidth: 180f, themePanel: ThemePanel);
            WeatherSettings settings = SettingsManager.WeatherSettings;
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, new ElementStyle(titleWidth: 140f, themePanel: ThemePanel), settings.WeatherSets.GetSelectedSetIndex(), 
                "Weather set", settings.WeatherSets.GetSetNames(), elementWidth: 205f, onDropdownOptionSelect: () => Parent.RebuildCategoryPanel(),
                tooltip: "* = preset and cannot be modified or deleted. Create a new set to save custom settings.");
            GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Create", "Delete", "Rename", "Copy" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherPanelButtonClick(button));
            }
            WeatherSet set = (WeatherSet)settings.WeatherSets.GetSelectedSet();
            CreateHorizontalDivider(DoublePanelLeft);
            ElementStyle toggleStyle = new ElementStyle(titleWidth: 150f, themePanel: ThemePanel);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.UseSchedule, "Use schedule",
                tooltip: "Follow a programmed weather schedule.");
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.ScheduleLoop, "Loop schedule");
            group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherPanelButtonClick(button));
            }
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, set.Skybox, "Skybox", RCextensions.EnumToStringArray<WeatherSkybox>());
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.SkyboxColor, "Skybox color", settingsPopup.ColorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Daylight, "Daylight", settingsPopup.ColorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.AmbientLight, "Ambient light", settingsPopup.ColorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Flashlight, "Flashlight", settingsPopup.ColorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.FogColor, "Fog color", settingsPopup.ColorPickPopup);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.FogDensity, "Fog density");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Rain, "Rain");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Thunder, "Thunder");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Snow, "Snow");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Wind, "Wind");
        }

        private void OnWeatherPanelButtonClick(string name)
        {
            SettingsPopup settingsPopup = (SettingsPopup)(Parent.Parent);
            SetNamePopup setNamePopup = settingsPopup.SetNamePopup;
            WeatherSettings settings = SettingsManager.WeatherSettings;
            switch (name)
            {
                case "Create":
                    setNamePopup.Show("New set", () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Create"));
                    break;
                case "Delete":
                    if (settings.WeatherSets.CanDeleteSelectedSet())
                        UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnWeatherSetOperationFinish(name),
                            UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Rename":
                    if (settings.WeatherSets.CanEditSelectedSet())
                    {
                        string currentSetName = settings.WeatherSets.GetSelectedSet().Name.Value;
                        setNamePopup.Show(currentSetName, () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Rename"));
                    }
                    break;
                case "Copy":
                    setNamePopup.Show("New set", () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Copy"));
                    break;
                case "Edit schedule":
                    // settingsPopup.EditWeatherSchedulePopup.Show(((WeatherSet)settings.WeatherSets.GetSelectedSet()).Schedule);
                    break;
                case "Import":
                    settingsPopup.ImportPopup.Show(onSave: () => OnWeatherSetOperationFinish(name));
                    break;
                case "Export":
                    settingsPopup.ExportPopup.Show(((WeatherSet)settings.WeatherSets.GetSelectedSet()).Schedule.Value);
                    break;
            }
        }

        private void OnWeatherSetOperationFinish(string name)
        {
            SettingsPopup settingsPopup = (SettingsPopup)(Parent.Parent);
            SetNamePopup setNamePopup = settingsPopup.SetNamePopup;
            SetSettingsContainer<WeatherSet> settings = SettingsManager.WeatherSettings.WeatherSets;
            switch (name)
            {
                case "Create":
                    settings.CreateSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Delete":
                    settings.DeleteSelectedSet();
                    settings.GetSelectedSetIndex().Value = 0;
                    break;
                case "Rename":
                    settings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
                    break;
                case "Copy":
                    settings.CopySelectedSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Import":
                    ImportPopup importPopup = settingsPopup.ImportPopup;
                    ((WeatherSet)settings.GetSelectedSet()).Schedule.Value = importPopup.ImportSetting.Value;
                    break;
            }
            Parent.RebuildCategoryPanel();
        }
    }
}
