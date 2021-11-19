using Settings;
using UnityEngine.UI;

namespace UI
{
    class EditProfileProfilePanel: BasePanel
    {
        protected override float Width => 720f;
        protected override float Height => 520f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ProfileSettings settings = SettingsManager.ProfileSettings;
            ElementStyle style = new ElementStyle(titleWidth: 120f, themePanel: ThemePanel);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Name, UIManager.GetLocaleCommon("Name"), elementWidth: 200f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Guild, UIManager.GetLocaleCommon("Guild"), elementWidth: 200f);
        }
    }
}
