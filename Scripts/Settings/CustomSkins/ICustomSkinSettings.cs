namespace Settings
{
    interface ICustomSkinSettings: ISetSettingsContainer
    {
        BoolSetting GetSkinsLocal();
        BoolSetting GetSkinsEnabled();
    }
}
