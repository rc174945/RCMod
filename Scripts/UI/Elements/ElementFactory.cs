using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ApplicationManagers;
using UnityEngine.UI;
using UnityEngine.Events;
using Settings;

namespace UI
{
    class ElementFactory
    {
        public static ElementStyle CurrentElementStyle = new ElementStyle();
        public static GameObject CreateDefaultMenu<T>() where T : BaseMenu
        {
            GameObject menu = AssetBundleManager.InstantiateAsset<GameObject>("DefaultMenu");
            menu.transform.position = Vector3.zero;
            BaseMenu component = menu.AddComponent<T>();
            component.Setup();
            component.ApplyScale();
            return menu;
        }

        public static GameObject CreateDefaultPanel<T>(Transform parent, bool enabled = false) where T : BasePanel
        {
            return InstantiateAndSetupPanel<T>(parent, "DefaultPanel", enabled);
        }

        public static GameObject CreateDefaultPanel(Transform parent, Type t, bool enabled = false)
        {
            GameObject panel = InstantiateAndBind(parent, "DefaultPanel");
            ((BasePanel)panel.AddComponent(t)).Setup(parent.GetComponent<BasePanel>());
            panel.SetActive(enabled);
            return panel;
        }

        public static GameObject CreateHeadedPanel<T>(Transform parent, bool enabled = false) where T : HeadedPanel
        {
            return InstantiateAndSetupPanel<T>(parent, "HeadedPanel", enabled);
        }

        public static GameObject CreateTooltipPopup<T>(Transform parent, bool enabled = false) where T : TooltipPopup
        {
            return InstantiateAndSetupPanel<T>(parent, "TooltipPopup", enabled);
        }

        public static GameObject CreateDefaultButton(Transform parent, ElementStyle style, string title, float elementWidth = 0f, float elementHeight = 0f,
            UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "DefaultButton");
            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            LayoutElement layout = button.GetComponent<LayoutElement>();
            if (elementWidth > 0f)
                layout.preferredWidth = elementWidth;
            if (elementHeight > 0f)
                layout.preferredHeight = elementHeight;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            return button;
        }

        public static GameObject CreateCategoryButton(Transform parent, ElementStyle style, string title, UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "CategoryButton");
            Text text = button.GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "CategoryButton", "");
            return button;
        }

        public static GameObject CreateDropdownSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options,
            string tooltip = "", float elementWidth = 140f, float elementHeight = 40f, float maxScrollHeight = 300f, float? optionsWidth = null,
            UnityAction onDropdownOptionSelect = null)
        {
            GameObject dropdownSetting = InstantiateAndBind(parent, "DropdownSetting");
            DropdownSettingElement element = dropdownSetting.AddComponent<DropdownSettingElement>();
            if (optionsWidth == null)
                optionsWidth = elementWidth;
            element.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight, optionsWidth.Value,
                maxScrollHeight, onDropdownOptionSelect);
            return dropdownSetting;
        }

        public static GameObject CreateIncrementSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 33f, float elementHeight = 30f, string[] options = null,
            UnityAction onValueChanged = null)
        {
            GameObject incrementSetting = InstantiateAndBind(parent, "IncrementSetting");
            IncrementSettingElement element = incrementSetting.AddComponent<IncrementSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, options, onValueChanged);
            return incrementSetting;
        }

        public static GameObject CreateToggleSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 30f, float elementHeight = 30f)
        {
            GameObject toggleSetting = InstantiateAndBind(parent, "ToggleSetting");
            ToggleSettingElement element = toggleSetting.AddComponent<ToggleSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight);
            return toggleSetting;
        }

        public static GameObject CreateToggleGroupSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options,
           string tooltip = "", float elementWidth = 30f, float elementHeight = 30f)
        {
            GameObject toggleGroupSetting = InstantiateAndBind(parent, "ToggleGroupSetting");
            ToggleGroupSettingElement element = toggleGroupSetting.AddComponent<ToggleGroupSettingElement>();
            element.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight);
            return toggleGroupSetting;
        }

        public static GameObject CreateSliderSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 150f, float elementHeight = 16f, int decimalPlaces = 2)
        {
            GameObject sliderSetting = InstantiateAndBind(parent, "SliderSetting");
            SliderSettingElement element = sliderSetting.AddComponent<SliderSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, decimalPlaces);
            return sliderSetting;
        }

        public static GameObject CreateInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 140f, float elementHeight = 40f, bool multiLine = false,
            UnityAction onValueChanged = null, UnityAction onEndEdit = null)
        {
            GameObject inputSetting = InstantiateAndBind(parent, "InputSetting");
            InputSettingElement element = inputSetting.AddComponent<InputSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, multiLine, onValueChanged, onEndEdit);
            return inputSetting;
        }

        public static GameObject CreateSliderInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float sliderWidth = 150f, float sliderHeight = 16f, float inputWidth = 70f,
            float inputHeight = 40f, int decimalPlaces = 2)
        {
            GameObject sliderInputSetting = InstantiateAndBind(parent, "SliderInputSetting");
            SliderInputSettingElement element = sliderInputSetting.AddComponent<SliderInputSettingElement>();
            element.Setup(setting, style, title, tooltip, sliderWidth, sliderHeight, inputWidth, inputHeight, decimalPlaces);
            return sliderInputSetting;
        }

        public static GameObject CreateDefaultLabel(Transform parent, ElementStyle style, string title, FontStyle fontStyle = FontStyle.Normal, 
            TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GameObject label = InstantiateAndBind(parent, "DefaultLabel");
            Text text = label.GetComponent<Text>();
            text.fontSize = style.FontSize;
            text.text = title;
            text.fontStyle = fontStyle;
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
            text.alignment = alignment;
            if (parent.GetComponent<VerticalLayoutGroup>() != null && parent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth)
                text.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            return label;
        }

        public static GameObject CreateKeybindSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, KeybindPopup keybindPopup,
            string tooltip = "", float elementWidth = 120f, float elementHeight = 35f, int bindCount = 2)
        {
            GameObject keybindSetting = InstantiateAndBind(parent, "KeybindSetting");
            KeybindSettingElement element = keybindSetting.AddComponent<KeybindSettingElement>();
            element.Setup(setting, style, title, keybindPopup, tooltip, elementWidth, elementHeight, bindCount);
            return keybindSetting;
        }

        public static GameObject CreateColorSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, ColorPickPopup colorPickPopup,
            string tooltip = "", float elementWidth = 90f, float elementHeight = 30f)
        {
            GameObject colorSetting = InstantiateAndBind(parent, "ColorSetting");
            ColorSettingElement element = colorSetting.AddComponent<ColorSettingElement>();
            element.Setup(setting, style, title, colorPickPopup, tooltip, elementWidth, elementHeight);
            return colorSetting;
        }

        public static GameObject CreateHorizontalLine(Transform parent, ElementStyle style, float width, float height = 1f)
        {
            GameObject line = InstantiateAndBind(parent, "HorizontalLine");
            line.transform.Find("LineImage").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            line.transform.Find("LineImage").gameObject.AddComponent<HorizontalLineScaler>();
            line.transform.Find("LineImage").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "HorizontalLineColor");
            return line;
        }

        public static GameObject CreateHorizontalGroup(Transform parent, float spacing, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            GameObject group = InstantiateAndBind(parent, "HorizontalGroup");
            group.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
            group.GetComponent<HorizontalLayoutGroup>().childAlignment = alignment;
            return group;
        }

        public static GameObject InstantiateAndSetupPanel<T>(Transform parent, string asset, bool enabled = false) where T : BasePanel
        {
            GameObject panel = InstantiateAndBind(parent, asset);
            panel.AddComponent<T>().Setup(parent.GetComponent<BasePanel>());
            panel.SetActive(enabled);
            return panel;
        }

        public static GameObject InstantiateAndBind(Transform parent, string asset)
        {
            GameObject obj = AssetBundleManager.InstantiateAsset<GameObject>(asset);
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        public static void SetAnchor(GameObject obj, TextAnchor anchor, TextAnchor pivot, Vector2 offset)
        {
            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.anchorMin = transform.anchorMax = GetAnchorVector(anchor);
            transform.pivot = GetAnchorVector(pivot);
            transform.anchoredPosition = offset;
        }

        public static Vector2 GetAnchorVector(TextAnchor anchor)
        {
            Vector2 anchorVector = new Vector2(0f, 0f);
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    anchorVector = new Vector2(0f, 1f);
                    break;
                case TextAnchor.MiddleLeft:
                    anchorVector = new Vector2(0f, 0.5f);
                    break;
                case TextAnchor.LowerLeft:
                    anchorVector = new Vector2(0f, 0f);
                    break;
                case TextAnchor.UpperCenter:
                    anchorVector = new Vector2(0.5f, 1f);
                    break;
                case TextAnchor.MiddleCenter:
                    anchorVector = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.LowerCenter:
                    anchorVector = new Vector2(0.5f, 0f);
                    break;
                case TextAnchor.UpperRight:
                    anchorVector = new Vector2(1f, 1f);
                    break;
                case TextAnchor.MiddleRight:
                    anchorVector = new Vector2(1f, 0.5f);
                    break;
                case TextAnchor.LowerRight:
                    anchorVector = new Vector2(1f, 0f);
                    break;
            }
            return anchorVector;
        }
    }
}
