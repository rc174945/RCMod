using System;
using UnityEngine;

namespace Settings
{
    class LegacyGeneralSettings : BaseSettingsContainer
    {
        public BoolSetting SpecMode = new BoolSetting(false);
        public BoolSetting LiveSpectate = new BoolSetting(true);
    }
}
