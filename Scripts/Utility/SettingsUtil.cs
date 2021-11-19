using Settings;
using UnityEngine;

namespace Utility
{
    class SettingsUtil
    {
        public static void SetSettingValue(BaseSetting setting, SettingType type, object value)
        {
            if (type == SettingType.Bool)
                ((BoolSetting)setting).Value = (bool)value;
            else if (type == SettingType.Color)
                ((ColorSetting)setting).Value = (Color)value;
            else if (type == SettingType.Float)
                ((FloatSetting)setting).Value = (float)value;
            else if (type == SettingType.Int)
                ((IntSetting)setting).Value = (int)value;
            else if (type == SettingType.String)
                ((StringSetting)setting).Value = (string)value;
            else
                Debug.Log("Attempting to set invalid setting value.");
        }

        public static object DeserializeValueFromJson(SettingType type, string json)
        {
            BaseSetting setting = CreateBaseSetting(type);
            if (setting == null)
                return setting;
            setting.DeserializeFromJsonString(json);
            return setting;
        }

        public static BaseSetting CreateBaseSetting(SettingType type)
        {
            switch (type)
            {
                case SettingType.Bool:
                    return new BoolSetting();
                case SettingType.Int:
                    return new IntSetting();
                case SettingType.Float:
                    return new FloatSetting();
                case SettingType.Color:
                    return new ColorSetting();
                case SettingType.String:
                    return new StringSetting();
                default:
                    return null;
            }
        }
    }
}
