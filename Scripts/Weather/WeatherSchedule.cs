using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Weather
{
    class WeatherSchedule
    {
        static Dictionary<string, WeatherAction> NameToWeatherAction = RCextensions.EnumToDict<WeatherAction>();
        static Dictionary<string, WeatherEffect> NameToWeatherEffect = RCextensions.EnumToDict<WeatherEffect>();
        static Dictionary<string, WeatherValueSelectType> NameToWeatherValueSelectType = RCextensions.EnumToDict<WeatherValueSelectType>();
        public List<WeatherEvent> Events = new List<WeatherEvent>();

        public WeatherSchedule()
        {
        }

        public WeatherSchedule(string csv)
        {
            DeserializeFromCSV(csv);
        }

        public string SerializeToCSV()
        {
            List<string> lines = new List<string>();
            foreach (WeatherEvent ev in Events)
            {
                List<string> items = new List<string>();
                items.Add(ev.Action.ToString());
                if (ev.Effect != WeatherEffect.None)
                    items.Add(ev.Effect.ToString());
                if (ev.ValueSelectType != WeatherValueSelectType.None)
                {
                    if (ev.Action != WeatherAction.Label)
                        items.Add(ev.ValueSelectType.ToString());
                    if (ev.ValueSelectType == WeatherValueSelectType.RandomFromList)
                    {
                        for (int i = 0; i < ev.Values.Count; i++)
                            items.Add(SerializeRandomListValue(ev.GetValueType(), ev.Values[i], ev.Weights[i]));
                    }
                    else
                    {
                        foreach (object value in ev.Values)
                            items.Add(SerializeValue(ev.GetValueType(), value));
                    }
                }
                lines.Add(string.Join(",", items.ToArray()));
            }
            return string.Join(";\n", lines.ToArray());
        }

        public string DeserializeFromCSV(string csv)
        {
            Events.Clear();
            string[] lines = csv.Split(';');
            int lineCount = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string trim = lines[i].Trim();
                    lineCount += lines[i].Split('\n').Length - 1;
                    if (trim != string.Empty && !trim.StartsWith("//"))
                        Events.Add(DeserializeLine(trim));
                }
                catch (Exception ex)
                {
                    return string.Format("Import failed at line {0}", lineCount);
                }
            }
            return "";
        }

        private string SerializeValue(WeatherValueType type, object value)
        {
            string str = "";
            switch (type)
            {
                case WeatherValueType.String:
                    str = (string)value;
                    break;
                case WeatherValueType.Float:
                    str = ((float)value).ToString();
                    break;
                case WeatherValueType.Int:
                    str = ((int)value).ToString();
                    break;
                case WeatherValueType.Bool:
                    str = Convert.ToInt32((bool)value).ToString();
                    break;
                case WeatherValueType.Color:
                    str = SerializeColor((Color)value);
                    break;
            }
            str = str.Replace(",", string.Empty);
            str = str.Replace(";", string.Empty);
            return str;
        }

        private string SerializeRandomListValue(WeatherValueType type, object value, float weight)
        {
            return SerializeValue(type, value) + "-" + weight.ToString();
        }

        private string SerializeColor(Color color)
        {
            string[] str = new string[4];
            str[0] = color.r.ToString();
            str[1] = color.g.ToString();
            str[2] = color.b.ToString();
            str[3] = color.a.ToString();
            if (color.a == 1f && color.r == color.g && color.r == color.b)
                return str[0];
            return string.Join("-", str);
        }

        private WeatherEvent DeserializeLine(string line)
        {
            WeatherEvent ev = new WeatherEvent();
            string[] items = line.Split(',');
            int index = 0;
            ev.Action = NameToWeatherAction[items[index++]];
            if (ev.SupportsWeatherEffects())
                ev.Effect = NameToWeatherEffect[items[index++]];
            if (ev.Action == WeatherAction.Label)
                ev.ValueSelectType = WeatherValueSelectType.Constant;
            else if (ev.SupportsWeatherValueSelectTypes())
                ev.ValueSelectType = NameToWeatherValueSelectType[items[index++]];
            if (ev.ValueSelectType == WeatherValueSelectType.RandomFromList)
            {
                for (int i = index; i < items.Length; i++)
                {
                    string[] weightedValue = items[i].Split('-');
                    ev.Values.Add(DeserializeValue(ev.GetValueType(), weightedValue[0]));
                    if (weightedValue.Length > 1)
                        ev.Weights.Add(float.Parse(weightedValue[1]));
                    else
                        ev.Weights.Add(1f);
                }
            }
            else
            {
                for (int i = index; i < items.Length; i++)
                    ev.Values.Add(DeserializeValue(ev.GetValueType(), items[i]));
            }
            return ev;
        }

        private object DeserializeValue(WeatherValueType type, string item)
        {
            switch (type)
            {
                case WeatherValueType.String:
                    return item;
                case WeatherValueType.Float:
                    return float.Parse(item);
                case WeatherValueType.Int:
                    return int.Parse(item);
                case WeatherValueType.Bool:
                    return Convert.ToBoolean(int.Parse(item));
                case WeatherValueType.Color:
                    return DeserializeColor(item);
            }
            return null;
        }

        private Color DeserializeColor(string item)
        {
            string[] nums = item.Split('-');
            if (nums.Length == 1)
            {
                float num = float.Parse(nums[0]);
                return new Color(num, num, num, 1f);
            }    
            return new Color(float.Parse(nums[0]), float.Parse(nums[1]), float.Parse(nums[2]), float.Parse(nums[3]));
        }
    }
}
