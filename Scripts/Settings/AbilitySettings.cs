using System;
using UnityEngine;

namespace Settings
{
    class AbilitySettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Ability.json"; } }
        public ColorSetting BombColor = new ColorSetting(new Color(1f, 1f, 1f, 1f), minAlpha: 0.5f);
        public IntSetting BombRadius = new IntSetting(5, minValue: 0, maxValue: 10);
        public IntSetting BombRange = new IntSetting(5, minValue: 0, maxValue: 10);
        public IntSetting BombSpeed = new IntSetting(5, minValue: 0, maxValue: 10);
        public IntSetting BombCooldown = new IntSetting(5, minValue: 0, maxValue: 10);
        public BoolSetting ShowBombColors = new BoolSetting(false);
        public BoolSetting UseOldEffect = new BoolSetting(false);
    }
}
