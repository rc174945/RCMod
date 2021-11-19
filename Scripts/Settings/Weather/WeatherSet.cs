using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Weather;

namespace Settings
{
    class WeatherSet : BaseSetSetting
    {
        public StringSetting Skybox = new StringSetting("Day");
        public ColorSetting SkyboxColor = new ColorSetting(new Color(0.5f, 0.5f, 0.5f));
        public ColorSetting Daylight = new ColorSetting(new Color(1f, 1f, 1f));
        public ColorSetting AmbientLight = new ColorSetting(new Color(0.494f, 0.478f, 0.447f));
        public ColorSetting Flashlight = new ColorSetting(new Color(1f, 1f, 1f, 0f));
        public FloatSetting FogDensity = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public ColorSetting FogColor = new ColorSetting(new Color(0.5f, 0.5f, 0.5f));
        public FloatSetting Rain = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Thunder = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Snow = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Wind = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public BoolSetting UseSchedule = new BoolSetting(false);
        public BoolSetting ScheduleLoop = new BoolSetting(false);
        public StringSetting Schedule = new StringSetting(string.Empty);
    }
}
