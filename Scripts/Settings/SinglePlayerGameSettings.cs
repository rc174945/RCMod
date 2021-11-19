namespace Settings
{
    class SingleplayerGameSettings : BaseSettingsContainer
    {
        public StringSetting Map = new StringSetting("[S]Tutorial");
        public IntSetting Difficulty = new IntSetting((int)GameDifficulty.Normal);
        public StringSetting Character = new StringSetting("Mikasa");
        public IntSetting Costume = new IntSetting(0);
        public IntSetting CameraType = new IntSetting((int)CAMERA_TYPE.ORIGINAL);
    }

    public enum GameDifficulty
    {
        Normal,
        Hard,
        Abnormal
    }
}