namespace Settings
{
    class TitanInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "TitanInput.json"; } }
        public KeybindSetting AttackPunch = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting AttackSlam = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting AttackSlap = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackGrabFront = new KeybindSetting(new string[] { "1", "None" });
        public KeybindSetting AttackGrabBack = new KeybindSetting(new string[] { "3", "None" });
        public KeybindSetting AttackGrabNape = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting AttackBite = new KeybindSetting(new string[] { "2", "None" });
        public KeybindSetting CoverNape = new KeybindSetting(new string[] { "Z", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Sit = new KeybindSetting(new string[] { "X", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftShift", "None" });
    }
}
