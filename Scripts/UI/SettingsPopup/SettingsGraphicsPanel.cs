using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGraphicsPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "Graphics";
            GraphicsSettings settings = SettingsManager.GraphicsSettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.OverallQuality, UIManager.GetLocale(cat, sub, "OverallQuality"),
                UIManager.GetLocaleArray(cat, sub, "OverallQualityOptions"), elementWidth: 200f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.TextureQuality, UIManager.GetLocale(cat, sub, "TextureQuality"),
                UIManager.GetLocaleArray(cat, sub, "TextureQualityOptions"), elementWidth: 200f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.VSync, UIManager.GetLocale(cat, sub, "VSync"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSCap, UIManager.GetLocale(cat, sub, "FPSCap"), elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ShowFPS, UIManager.GetLocale(cat, sub, "ShowFPS"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ExclusiveFullscreen, UIManager.GetLocale(cat, sub, "ExclusiveFullscreen"), tooltip: UIManager.GetLocale(cat, sub, "ExclusiveFullscreenTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.InterpolationEnabled, UIManager.GetLocale(cat, sub, "InterpolationEnabled"), tooltip: UIManager.GetLocale(cat, sub, "InterpolationEnabledTooltip"));
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.WeatherEffects, UIManager.GetLocale(cat, sub, "WeatherEffects"),
               UIManager.GetLocaleArray(cat, sub, "WeatherEffectsOptions"), elementWidth: 200f);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.AntiAliasing, UIManager.GetLocale(cat, sub, "AntiAliasing"),
               UIManager.GetLocaleArray(cat, sub, "AntiAliasingOptions"), elementWidth: 200f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.RenderDistance, UIManager.GetLocale(cat, sub, "RenderDistance"), elementWidth: 130f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AnimatedIntro, UIManager.GetLocale(cat, sub, "AnimatedIntro"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.WindEffectEnabled, UIManager.GetLocale(cat, sub, "WindEffectEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.WeaponTrailEnabled, UIManager.GetLocale(cat, sub, "WeaponTrailEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.BlurEnabled, UIManager.GetLocale(cat, sub, "BlurEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.MipmapEnabled, UIManager.GetLocale(cat, sub, "MipmapEnabled"), tooltip: UIManager.GetLocale(cat, sub, "MipmapEnabledTooltip"));
        }
    }
}
