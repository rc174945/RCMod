using System.Collections.Generic;
using System.Linq;
using System.Text;
using Settings;
using UnityEngine;
using Utility;

namespace Weather
{
    class WeatherEvent
    {
        public WeatherAction Action;
        public WeatherEffect Effect;
        public WeatherValueType ValueType;
        public List<string> Values = new List<string>();

        public WeatherEvent(WeatherAction action, WeatherEffect effect, WeatherValueType valueType, List<string> values)
        {
            Action = action;
            Effect = effect;
            ValueType = valueType;
            Values = values;
        }

        public object DeserializeValue()
        {
            SettingType type = GetSettingType();
            List<object> list = new List<object>();
            foreach (StringSetting serializedValue in Values.Value)
                list.Add(SettingsUtil.DeserializeValueFromJson(type, serializedValue.Value));
            WeatherValueType valueType = (WeatherValueType)ValueType.Value;
            if (valueType == WeatherValueType.Constant)
                return list[0];
            else if (valueType == WeatherValueType.RandomBetween)
            {
                if (type == SettingType.Float)
                    return Random.Range((float)list[0], (float)list[1]);
                else if (type == SettingType.Int)
                    return Random.Range((int)list[0], (int)list[1] + 1);
                else if (type == SettingType.Color)
                {
                    Color color1 = (Color)list[0];
                    Color color2 = (Color)list[1];
                    float r = Random.Range(Mathf.Min(color1.r, color2.r), Mathf.Max(color1.r, color2.r));
                    float g = Random.Range(Mathf.Min(color1.g, color2.g), Mathf.Max(color1.g, color2.g));
                    float b = Random.Range(Mathf.Min(color1.b, color2.b), Mathf.Max(color1.b, color2.b));
                    float a = Random.Range(Mathf.Min(color1.a, color2.a), Mathf.Max(color1.a, color2.a));
                    return new Color(r, g, b, a);
                }
            }
            else if (valueType == WeatherValueType.RandomFromList)
            {
                return list[Random.Range(0, list.Count)];
            }
            return null;
        }

        public SettingType GetSettingType()
        {
            switch ((WeatherAction)Action.Value)
            {
                case WeatherAction.SetDefault:
                case WeatherAction.SetDefaultAll:
                case WeatherAction.Return:
                    return SettingType.None;
                case WeatherAction.SetTargetTime:
                case WeatherAction.Wait:
                    return SettingType.Float;
                case WeatherAction.Goto:
                case WeatherAction.Label:
                    return SettingType.String;
            }
            switch ((WeatherEffect)Effect.Value)
            {
                case WeatherEffect.Daylight:
                case WeatherEffect.AmbientLight:
                case WeatherEffect.FogColor:
                case WeatherEffect.FlashlightColor:
                    return SettingType.Color;
                case WeatherEffect.FogDensity:
                case WeatherEffect.Rain:
                case WeatherEffect.Thunder:
                case WeatherEffect.Snow:
                case WeatherEffect.Wind:
                    return SettingType.Float;
                case WeatherEffect.Skybox:
                    return SettingType.Int;
                default:
                    return SettingType.None;
            }
        }

        public string[] SupportedWeatherEffects()
        {
            switch ((WeatherAction)Action.Value)
            {
                case WeatherAction.SetDefault:
                case WeatherAction.SetValue:
                case WeatherAction.SetTargetValue:
                case WeatherAction.SetTargetTime:
                    return RCextensions.EnumToStringArray<WeatherEffect>();
                default:
                    return new string[0];
            }
        }

        public string[] SupportedWeatherValueTypes()
        {
            switch (GetSettingType())
            {
                case SettingType.Float:
                case SettingType.Color:
                case SettingType.Int:
                    return RCextensions.EnumToStringArray<WeatherValueType>();
                case SettingType.Bool:
                case SettingType.String:
                    return new string[] { WeatherValueType.Constant.ToString(), WeatherValueType.RandomFromList.ToString() };
                default:
                    return new string[0];
            }
        }
    }

    public enum WeatherAction
    {
        SetDefaultAll,
        SetDefault,
        SetValue,
        SetTargetValue,
        SetTargetTime,
        Wait,
        Goto,
        Label,
        Return
    }

    public enum WeatherEffect
    {
        Daylight,
        AmbientLight,
        Skybox,
        FlashlightColor,
        FogDensity,
        FogColor,
        Rain,
        Thunder,
        Snow,
        Wind
    }

    public enum WeatherValueType
    {
        Constant,
        RandomBetween,
        RandomFromList
    }

    public enum WeatherSkybox
    {
        Day,
        Dawn,
        Dusk,
        Night,
        Cloudy,
        Storm
    }
}
