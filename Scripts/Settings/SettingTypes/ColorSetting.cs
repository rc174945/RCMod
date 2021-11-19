using System.Collections.Generic;
using UnityEngine;
using SimpleJSONFixed;

namespace Settings
{
    class ColorSetting: TypedSetting<Color>
    {
        public float MinAlpha = 0f;

        public ColorSetting(): base(Color.white)
        { 
        }

        public ColorSetting(Color defaultValue, float minAlpha = 0f)
        {
            MinAlpha = minAlpha;
            DefaultValue = SanitizeValue(defaultValue);
            Value = DefaultValue;
        }

        protected override Color SanitizeValue(Color value)
        {
            value.r = Mathf.Clamp(value.r, 0f, 1f);
            value.g = Mathf.Clamp(value.g, 0f, 1f);
            value.b = Mathf.Clamp(value.b, 0f, 1f);
            value.a = Mathf.Clamp(value.a, MinAlpha, 1f);
            return value;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            array.Add(new JSONNumber(Value.r));
            array.Add(new JSONNumber(Value.g));
            array.Add(new JSONNumber(Value.b));
            array.Add(new JSONNumber(Value.a));
            return array;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            JSONArray array = json.AsArray;
            Value = new Color(array[0].AsFloat, array[1].AsFloat, array[2].AsFloat, array[3].AsFloat);
        }
    }
}
