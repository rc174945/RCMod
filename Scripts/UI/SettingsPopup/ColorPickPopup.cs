using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class ColorPickPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocale("SettingsPopup", "ColorPickPopup", "Title");
        protected override float Width => 450f;
        protected override float Height => 450f;
        protected override float VerticalSpacing => 20f;
        protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;
        private float PreviewWidth = 90f;
        private float PreviewHeight = 40f;
        private Image _image;
        private ColorSetting _setting;
        private Image _preview;
        private FloatSetting _red = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        private FloatSetting _green = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        private FloatSetting _blue = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        private FloatSetting _alpha = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        private List<GameObject> _sliders = new List<GameObject>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateDefaultButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            GameObject preview = ElementFactory.InstantiateAndBind(SinglePanel, "ColorPreview");
            preview.GetComponent<LayoutElement>().preferredWidth = PreviewWidth;
            preview.GetComponent<LayoutElement>().preferredHeight = PreviewHeight;
            _preview = preview.transform.Find("Image").GetComponent<Image>();
        }

        private void Update()
        {
            if (_preview != null)
            {
                _preview.color = GetColorFromSliders();
            }
        }

        public void Show(ColorSetting setting, Image image)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _setting = setting;
            _image = image;
            _red.Value = setting.Value.r;
            _green.Value = setting.Value.g;
            _blue.Value = setting.Value.b;
            _alpha.MinValue = setting.MinAlpha;
            _alpha.Value = setting.Value.a;
            _preview.color = GetColorFromSliders();
            CreateSliders();
        }

        private void CreateSliders()
        {
            foreach (GameObject obj in _sliders)
            {
                Destroy(obj);
            }
            ElementStyle style = new ElementStyle(titleWidth: 85f, themePanel: ThemePanel);
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style,_red, "Red", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _green, "Green", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _blue, "Blue", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _alpha, "Alpha", decimalPlaces: 3));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                _setting.Value = GetColorFromSliders();
                _image.color = _setting.Value;
                Hide();
            }
        }

        private Color GetColorFromSliders()
        {
            return new Color(_red.Value, _green.Value, _blue.Value, Mathf.Clamp(_alpha.Value, _setting.MinAlpha, 1f));
        }
    }
}
