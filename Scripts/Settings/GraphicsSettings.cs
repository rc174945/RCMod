using ApplicationManagers;
using System;
using UnityEngine;

namespace Settings
{
    class GraphicsSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Graphics.json"; } }
        public IntSetting OverallQuality = new IntSetting(QualitySettings.GetQualityLevel());
        public IntSetting TextureQuality = new IntSetting((int)TextureQualityLevel.High);
        public BoolSetting VSync = new BoolSetting(false);
        public IntSetting FPSCap = new IntSetting(0, minValue: 0);
        public BoolSetting ExclusiveFullscreen = new BoolSetting(false);
        public BoolSetting ShowFPS = new BoolSetting(false);
        public BoolSetting MipmapEnabled = new BoolSetting(true);
        public BoolSetting WeaponTrailEnabled = new BoolSetting(true);
        public BoolSetting WindEffectEnabled = new BoolSetting(false);
        public BoolSetting InterpolationEnabled = new BoolSetting(true);
        public IntSetting RenderDistance = new IntSetting(1500, minValue: 10, maxValue: 1000000);
        public IntSetting WeatherEffects = new IntSetting(3);
        public BoolSetting AnimatedIntro = new BoolSetting(true);
        // public IntSetting Fov = new IntSetting(50, minValue: 0, maxValue: 1000);
        public BoolSetting BlurEnabled = new BoolSetting(false);
        // public IntSetting ShadowQuality = new IntSetting((int)ShadowQualityLevel.Off);
        public IntSetting AntiAliasing = new IntSetting((int)AntiAliasingLevel.Off);

        public override void Save()
        {
            base.Save();
            FullscreenHandler.SetMainData(ExclusiveFullscreen.Value);
        }

        public override void Load()
        {
            base.Load();
            FullscreenHandler.SetMainData(ExclusiveFullscreen.Value);
        }

        public override void Apply()
        {
            QualitySettings.SetQualityLevel(OverallQuality.Value, true);
            QualitySettings.vSyncCount = Convert.ToInt32(VSync.Value);
            Application.targetFrameRate = FPSCap.Value > 0 ? FPSCap.Value : -1;
            QualitySettings.masterTextureLimit = 3 - TextureQuality.Value;
            QualitySettings.antiAliasing = AntiAliasing.Value == 0 ? 0 : (int)Mathf.Pow(2, AntiAliasing.Value);
            ApplyShadows();
            IN_GAME_MAIN_CAMERA.ApplyGraphicsSettings();
        }

        private void ApplyShadows()
        {
            /*
            if (ShadowQuality.Value == (int)ShadowQualityLevel.Off)
            {
                QualitySettings.shadowCascades = 1;
                QualitySettings.shadowDistance = 0f;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.VeryLow)
            {
                QualitySettings.shadowCascades = 1;
                QualitySettings.shadowDistance = 20f;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.Low)
            {
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowDistance = 40f;
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.Medium)
            {
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowDistance = 70f;
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.High)
            {
                QualitySettings.shadowCascades = 4;
                QualitySettings.shadowDistance = 150f;
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
            }
            */
        }
    }

    public enum TextureQualityLevel
    {
        VeryLow,
        Low,
        Medium,
        High
    }

    public enum ShadowQualityLevel
    {
        Off,
        VeryLow,
        Low,
        Medium,
        High
    }

    public enum AntiAliasingLevel
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum OverallQualityLevel
    {
        Fastest,
        Fast,
        Simple,
        Good,
        Beautiful,
        Fantastic
    }

    public enum WeatherEffectLevel
    {
        Off,
        Low,
        Medium,
        High
    }
}
