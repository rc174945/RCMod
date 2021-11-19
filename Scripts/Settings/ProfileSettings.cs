using UnityEngine;
using UI;

namespace Settings
{
    class ProfileSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Profile.json"; } }
        public StringSetting Name = new StringSetting("GUEST" + Random.Range(0, 100000), maxLength: 50);
        public StringSetting Guild = new StringSetting(string.Empty, maxLength: 50);

        protected override void LoadLegacy()
        {
            Name.Value = SettingsManager.MultiplayerSettings.Name.Value;
            Guild.Value = SettingsManager.MultiplayerSettings.Guild.Value;
        }
    }
}
