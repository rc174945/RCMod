using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class IntroButton: Button
    {
        private float _fadeTime = 0.1f;
        private Image _hoverImage;

        protected override void Awake()
        {
            _hoverImage = transform.Find("HoverImage").GetComponent<Image>();
            _hoverImage.canvasRenderer.SetAlpha(0f);
            transition = Transition.ColorTint;
            targetGraphic = transform.Find("Content/Label").GetComponent<Graphic>();
            if (gameObject.name.StartsWith("Settings") || gameObject.name.StartsWith("Quit") || gameObject.name.StartsWith("Profile"))
                targetGraphic.GetComponent<Text>().text = UIManager.GetLocaleCommon(gameObject.name.Replace("Button", string.Empty));
            else
                targetGraphic.GetComponent<Text>().text = UIManager.GetLocale("MainMenu", "Intro", gameObject.name);
            ColorBlock newColors = new ColorBlock();
            ColorBlock themeColor = UIManager.GetThemeColorBlock("MainMenu", "IntroButton", "");
            newColors.normalColor = themeColor.normalColor;
            newColors.highlightedColor = themeColor.highlightedColor;
            newColors.pressedColor = themeColor.pressedColor;
            newColors.colorMultiplier = 1f;
            newColors.fadeDuration = _fadeTime;
            colors = newColors;
            navigation = new Navigation { mode = Navigation.Mode.None };
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            Image icon = transform.Find("Content/Icon").GetComponent<Image>();
            if (state == SelectionState.Pressed || state == SelectionState.Highlighted)
            {
                _hoverImage.CrossFadeAlpha(1f, _fadeTime, true);
                if (state == SelectionState.Pressed)
                    icon.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "PressedColor"), _fadeTime, true, true);
                else if (state == SelectionState.Highlighted)
                    icon.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "HighlightedColor"), _fadeTime, true, true);
            }
            else if (state == SelectionState.Normal)
            {
                _hoverImage.CrossFadeAlpha(0f, _fadeTime, true);
                icon.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "NormalColor"), _fadeTime, true, true);
            }
        }
    }
}
