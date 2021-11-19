using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;

namespace UI
{
    class ColorSettingElement : BaseSettingElement
    {
        private Image _image;
        private ColorPickPopup _colorPickPopup;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Color
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, ColorPickPopup colorPickPopup, string tooltip, 
            float elementWidth, float elementHeight)
        {
            _colorPickPopup = colorPickPopup;
            GameObject button = transform.Find("ColorButton").gameObject;
            button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked());
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
            _image = button.transform.Find("Border/Image").GetComponent<Image>();
            base.Setup(setting, style, title, tooltip);
        }

        protected void OnButtonClicked()
        {
            _colorPickPopup.Show(((ColorSetting)_setting), _image);
        }

        public override void SyncElement()
        {
            _image.color = ((ColorSetting)_setting).Value;
        }
    }
}
