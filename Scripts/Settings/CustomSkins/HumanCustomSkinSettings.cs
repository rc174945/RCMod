namespace Settings
{
    class HumanCustomSkinSettings:  BaseCustomSkinSettings<HumanCustomSkinSet>
    {
        public BoolSetting GasEnabled = new BoolSetting(true);
    }
}
