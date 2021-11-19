namespace Settings
{
    class ShifterInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "ShifterInput.json"; } }
        public KeybindSetting AttackDefault = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackSpecial = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting CoverNape = new KeybindSetting(new string[] { "Z", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Sit = new KeybindSetting(new string[] { "X", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting Roar = new KeybindSetting(new string[] { "N", "None" });
    }
}
