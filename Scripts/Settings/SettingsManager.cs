using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settings
{
    class SettingsManager
    {
        public static MultiplayerSettings MultiplayerSettings;
        public static ProfileSettings ProfileSettings;
        public static SingleplayerGameSettings SingleplayerGameSettings;
        public static MultiplayerGameSettings MultiplayerGameSettings;
        public static CustomSkinSettings CustomSkinSettings;
        public static GraphicsSettings GraphicsSettings;
        public static GeneralSettings GeneralSettings;
        public static UISettings UISettings;
        public static AbilitySettings AbilitySettings;
        public static InputSettings InputSettings;
        public static LegacyGameSettings LegacyGameSettings;
        public static LegacyGameSettings LegacyGameSettingsUI;
        public static LegacyGeneralSettings LegacyGeneralSettings;
        public static WeatherSettings WeatherSettings;

        public static void Init()
        {
            MultiplayerSettings = new MultiplayerSettings();
            ProfileSettings = new ProfileSettings();
            SingleplayerGameSettings = new SingleplayerGameSettings();
            MultiplayerGameSettings = new MultiplayerGameSettings();
            CustomSkinSettings = new CustomSkinSettings();
            GraphicsSettings = new GraphicsSettings();
            GeneralSettings = new GeneralSettings();
            UISettings = new UISettings();
            AbilitySettings = new AbilitySettings();
            InputSettings = new InputSettings();
            LegacyGameSettings = new LegacyGameSettings();
            LegacyGameSettingsUI = new LegacyGameSettings();
            LegacyGeneralSettings = new LegacyGeneralSettings();
            WeatherSettings = new WeatherSettings();
        }
    }
}
