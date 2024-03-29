﻿using UnityEngine;
using Utility;
using System.Collections.Generic;
using System.IO;
using Settings;
using SimpleJSONFixed;
using UnityEngine.UI;
using ApplicationManagers;
using System.Collections;
using System;

namespace UI
{
    class UIManager : MonoBehaviour
    {
        private static readonly string LanguageFolderPath = Application.dataPath + "/Resources/Languages";
        private static readonly string UIThemeFolderPath = Application.dataPath + "/Resources/UIThemes";
        private static Dictionary<string, JSONObject> _languages = new Dictionary<string, JSONObject>();
        private static Dictionary<string, JSONObject> _uiThemes = new Dictionary<string, JSONObject>();
        private static Dictionary<Type, string> _lastCategories = new Dictionary<Type, string>();
        private static string _currentUITheme;
        private static UIManager _instance;
        public static BaseMenu CurrentMenu;
        public static float CurrentCanvasScale = 1f;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            LoadLanguages();
            LoadUIThemes();
        }

        public static void FinishLoadAssets()
        {
            LoadEmojis();
            SetMenu(MenuType.Main);
        }

        public static void SetLastCategory(Type t, string category)
        {
            if (_lastCategories.ContainsKey(t))
                _lastCategories[t] = category;
            else
                _lastCategories.Add(t, category);
        }

        public static string GetLastcategory(Type t)
        {
            if (_lastCategories.ContainsKey(t))
                return _lastCategories[t];
            return string.Empty;
        }

        private static void LoadEmojis()
        {
            foreach (string emoji in GameMenu.AvailableEmojis)
                GameMenu.EmojiTextures.Add(emoji, AssetBundleManager.MainAssetBundle.Load("Emoji" + emoji) as Texture2D);
        }

        public static void SetMenu(MenuType menu)
        {
            _currentUITheme = SettingsManager.UISettings.UITheme.Value;
            if (CurrentMenu != null)
                Destroy(CurrentMenu);
            switch (menu)
            {
                case MenuType.Main:
                    _lastCategories.Clear();
                    CurrentMenu = ElementFactory.CreateDefaultMenu<MainMenu>().GetComponent<BaseMenu>();
                    break;
                case MenuType.Game:
                    CurrentMenu = ElementFactory.CreateDefaultMenu<GameMenu>().GetComponent<GameMenu>();
                    break;
                default:
                    break;
            }
        }

        public static string GetLocale(string category, string subCategory, string item = "", string forcedLanguage = "", string defaultValue = "")
        {
            JSONObject language = null;
            string languageName = forcedLanguage != string.Empty ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value;
            if (_languages.ContainsKey(languageName))
                language = _languages[languageName];
            string finalItem = subCategory;
            if (item != string.Empty)
                finalItem += "." + item;
            if (language == null || language[category] == null || language[category][finalItem] == null)
            {
                if (languageName == "English")
                {
                    if (defaultValue != string.Empty)
                        return defaultValue;
                    return string.Format("{0} locale error.", finalItem);
                }
                return GetLocale(category, subCategory, item, "English");
            }
            return language[category][finalItem].Value;
        }

        public static string[] GetLocaleArray(string category, string subCategory, string item = "", string forcedLanguage = "")
        {
            JSONObject language = null;
            string languageName = forcedLanguage != string.Empty ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value;
            if (_languages.ContainsKey(languageName))
                language = _languages[languageName];
            string finalItem = subCategory;
            if (item != string.Empty)
                finalItem += "." + item;
            if (language == null || language[category] == null || language[category][finalItem] == null)
            {
                if (languageName == "English")
                    return new string[] { string.Format("{0} locale error.", finalItem) };
                return GetLocaleArray(category, subCategory, item, "English");
            }
            List<string> array = new List<string>();
            foreach (JSONString data in (JSONArray)language[category][finalItem])
                array.Add(data.Value);
            return array.ToArray();
        }

        public static string GetLocaleCommon(string item)
        {
            return GetLocale("Common", item);
        }

        public static string[] GetLocaleCommonArray(string item)
        {
            return GetLocaleArray("Common", item);
        }

        public static string[] GetLanguages()
        {
            List<string> languages = new List<string>();
            foreach (string language in _languages.Keys)
            {
                if (language == "English")
                    languages.Insert(0, language);
                else
                    languages.Add(language);
            }
            return languages.ToArray();
        }

        private static void LoadLanguages()
        {
            if (!Directory.Exists(LanguageFolderPath))
            {
                Directory.CreateDirectory(LanguageFolderPath);
                Debug.Log("No language folder found, creating it.");
                return;
            }
            foreach (string file in Directory.GetFiles(LanguageFolderPath, "*.json"))
            {
                JSONObject json = (JSONObject)JSON.Parse(File.ReadAllText(file));
                if (!_languages.ContainsKey(json["Name"]))
                    _languages.Add(json["Name"].Value, json);
            }
            if (!_languages.ContainsKey(SettingsManager.GeneralSettings.Language.Value))
            {
                SettingsManager.GeneralSettings.Language.Value = "English";
                SettingsManager.GeneralSettings.Save();
            }
        }

        public static Color GetThemeColor(string panel, string category, string item, string fallbackPanel = "DefaultPanel")
        {
            JSONObject theme = null;
            if (_uiThemes.ContainsKey(_currentUITheme))
                theme = _uiThemes[_currentUITheme];
            if (theme == null || theme[panel] == null || theme[panel][category] == null || theme[panel][category][item] == null)
            {
                if (panel != fallbackPanel)
                    return GetThemeColor(fallbackPanel, category, item, fallbackPanel);
                Debug.Log(string.Format("{0} {1} {2} theme error.", panel, category, item));
                return Color.white;
            }
            try
            {
                List<float> array = new List<float>();
                foreach (JSONNumber data in (JSONArray)theme[panel][category][item])
                    array.Add(float.Parse(data.Value) / 255f);
                return new Color(array[0], array[1], array[2], array[3]);
            }
            catch
            {
                Debug.Log(string.Format("{0} {1} {2} theme error.", panel, category, item));
                return Color.white;
            }
        }

        public static ColorBlock GetThemeColorBlock(string panel, string category, string item, string fallbackPanel = "DefaultPanel")
        {
            Color normal = GetThemeColor(panel, category, item + "NormalColor", fallbackPanel);
            Color highlighted = GetThemeColor(panel, category, item + "HighlightedColor", fallbackPanel);
            Color pressed = GetThemeColor(panel, category, item + "PressedColor", fallbackPanel);
            ColorBlock block = new ColorBlock
            {
                colorMultiplier = 1f,
                fadeDuration = 0.1f,
                normalColor = normal,
                highlightedColor = highlighted,
                pressedColor = pressed,
                disabledColor = pressed
            };
            return block;
        }

        public static string[] GetUIThemes()
        {
            List<string> themes = new List<string>();
            bool foundLight = false;
            bool foundDark = false;
            foreach (string theme in _uiThemes.Keys)
            {
                if (theme == "Light")
                    foundLight = true;
                else if (theme == "Dark")
                    foundDark = true;
                else
                    themes.Add(theme);
            }
            if (foundDark)
                themes.Insert(0, "Dark");
            if (foundLight)
                themes.Insert(0, "Light");
            return themes.ToArray();
        }

        private static void LoadUIThemes()
        {
            if (!Directory.Exists(UIThemeFolderPath))
            {
                Directory.CreateDirectory(UIThemeFolderPath);
                Debug.Log("No UI theme folder found, creating it.");
                return;
            }
            foreach (string file in Directory.GetFiles(UIThemeFolderPath, "*.json"))
            {
                JSONObject json = (JSONObject)JSON.Parse(File.ReadAllText(file));
                if (!_uiThemes.ContainsKey(json["Name"]))
                    _uiThemes.Add(json["Name"].Value, json);
            }
            if (!_uiThemes.ContainsKey(SettingsManager.UISettings.UITheme.Value))
            {
                SettingsManager.UISettings.UITheme.Value = "Dark";
                SettingsManager.UISettings.Save();
            }
        }
    }

    public enum MenuType
    {
        Main,
        Customization,
        Snapshots,
        LevelEditor,
        Game
    }
}
