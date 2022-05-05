using System;
using UnityEngine;

namespace Settings
{
    class AbilitySettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Ability01.json"; } }
        public ColorSetting BombColor = new ColorSetting(new Color(1f, 1f, 1f, 1f), minAlpha: 0.5f);
        public IntSetting BombRadius = new IntSetting(6, minValue: 0, maxValue: 10);
        public IntSetting BombRange = new IntSetting(3, minValue: 0, maxValue: 3);
        public IntSetting BombSpeed = new IntSetting(6, minValue: 0, maxValue: 10);
        public IntSetting BombCooldown = new IntSetting(1, minValue: 0, maxValue: 6);
        public BoolSetting ShowBombColors = new BoolSetting(false);
        public BoolSetting UseOldEffect = new BoolSetting(false);

        protected override bool Validate()
        {
            return BombRadius.Value + BombRange.Value + BombSpeed.Value + BombCooldown.Value <= 16;
        }
    }
}
