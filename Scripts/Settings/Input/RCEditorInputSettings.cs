namespace Settings
{
    class RCEditorInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "RCEditorInput.json"; } }
        public KeybindSetting Up = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting Down = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting Slow = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting Fast = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting RotateRight = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting RotateLeft = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting RotateCCW = new KeybindSetting(new string[] { "Z", "None" });
        public KeybindSetting RotateCW = new KeybindSetting(new string[] { "C", "None" });
        public KeybindSetting RotateBack = new KeybindSetting(new string[] { "F", "None" });
        public KeybindSetting RotateForward = new KeybindSetting(new string[] { "R", "None" });
        public KeybindSetting Place = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Delete = new KeybindSetting(new string[] { "Backspace", "None" });
        public KeybindSetting Cursor = new KeybindSetting(new string[] { "X", "None" });
    }
}
