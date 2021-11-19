namespace Settings
{
    class SkyboxCustomSkinSet: BaseSetSetting
    {
        public StringSetting Front = new StringSetting(string.Empty);
        public StringSetting Back = new StringSetting(string.Empty);
        public StringSetting Left = new StringSetting(string.Empty);
        public StringSetting Right = new StringSetting(string.Empty);
        public StringSetting Up = new StringSetting(string.Empty);
        public StringSetting Down = new StringSetting(string.Empty);
    }
}
