using System;
using UnityEngine;

namespace Settings
{
    class GeneralSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "General.json"; } }
        public StringSetting Language = new StringSetting("English");
        public FloatSetting Volume = new FloatSetting(1f, minValue: 0f, maxValue: 1f);
        public FloatSetting MouseSpeed = new FloatSetting(0.5f, minValue: 0.01f, maxValue: 1f);
        public FloatSetting CameraDistance = new FloatSetting(1f, minValue: 0f, maxValue: 1f);
        public BoolSetting InvertMouse = new BoolSetting(false);
        public BoolSetting CameraTilt = new BoolSetting(true);
        public BoolSetting SnapshotsEnabled = new BoolSetting(false);
        public BoolSetting SnapshotsShowInGame = new BoolSetting(false);
        public IntSetting SnapshotsMinimumDamage = new IntSetting(0, minValue: 0);
        public BoolSetting MinimapEnabled = new BoolSetting(false);

        public override void Apply()
        {
            AudioListener.volume = Volume.Value;
            IN_GAME_MAIN_CAMERA.cameraDistance = CameraDistance.Value + 0.3f;
        }
    }
}
