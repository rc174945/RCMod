using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;

namespace UI
{
    class SettingsPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1010f;
        protected override float Height => 630f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "SettingsPopup";
        public KeybindPopup KeybindPopup;
        public ColorPickPopup ColorPickPopup;
        public SetNamePopup SetNamePopup;
        public ImportPopup ImportPopup;
        public ExportPopup ExportPopup;
        public EditWeatherSchedulePopup EditWeatherSchedulePopup;
        private List<BaseSettingsContainer> _ignoreDefaultButtonSettings = new List<BaseSettingsContainer>();
        private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
            SetupSettingsList();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "General", "Graphics", "UI", "Keybinds", "Skins", "CustomMap", "Game", "Ability" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button"),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("General", typeof(SettingsGeneralPanel));
            _categoryPanelTypes.Add("Graphics", typeof(SettingsGraphicsPanel));
            _categoryPanelTypes.Add("UI", typeof(SettingsUIPanel));
            _categoryPanelTypes.Add("Keybinds", typeof(SettingsKeybindsPanel));
            _categoryPanelTypes.Add("Skins", typeof(SettingsSkinsPanel));
            _categoryPanelTypes.Add("CustomMap", typeof(SettingsCustomMapPanel));
            _categoryPanelTypes.Add("Game", typeof(SettingsGamePanel));
            _categoryPanelTypes.Add("Ability", typeof(SettingsAbilityPanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            KeybindPopup = ElementFactory.CreateHeadedPanel<KeybindPopup>(transform).GetComponent<KeybindPopup>();
            ColorPickPopup = ElementFactory.CreateHeadedPanel<ColorPickPopup>(transform).GetComponent<ColorPickPopup>();
            SetNamePopup = ElementFactory.CreateHeadedPanel<SetNamePopup>(transform).GetComponent<SetNamePopup>();
            ImportPopup = ElementFactory.CreateHeadedPanel<ImportPopup>(transform).GetComponent<ImportPopup>();
            ExportPopup = ElementFactory.CreateHeadedPanel<ExportPopup>(transform).GetComponent<ExportPopup>();
            EditWeatherSchedulePopup = ElementFactory.CreateHeadedPanel<EditWeatherSchedulePopup>(transform).GetComponent<EditWeatherSchedulePopup>();
            _popups.Add(KeybindPopup);
            _popups.Add(ColorPickPopup);
            _popups.Add(SetNamePopup);
            _popups.Add(ImportPopup);
            _popups.Add(ExportPopup);
            _popups.Add(EditWeatherSchedulePopup);
        }

        private void SetupSettingsList()
        {
            _saveableSettings.Add(SettingsManager.GeneralSettings);
            _saveableSettings.Add(SettingsManager.GraphicsSettings);
            _saveableSettings.Add(SettingsManager.UISettings);
            _saveableSettings.Add(SettingsManager.InputSettings);
            _saveableSettings.Add(SettingsManager.CustomSkinSettings);
            _saveableSettings.Add(SettingsManager.AbilitySettings);
            _saveableSettings.Add(SettingsManager.LegacyGameSettingsUI);
            _saveableSettings.Add(SettingsManager.WeatherSettings);
            _ignoreDefaultButtonSettings.Add(SettingsManager.CustomSkinSettings);
            _ignoreDefaultButtonSettings.Add(SettingsManager.WeatherSettings);
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Default", "Load", "Save", "Continue", "Quit" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnConfirmSetDefault()
        {
            foreach (SaveableSettingsContainer setting in _saveableSettings)
            {
                if (!_ignoreDefaultButtonSettings.Contains(setting))
                {
                    setting.SetDefault();
                    setting.Save();
                }
            }
            RebuildCategoryPanel();
            UIManager.CurrentMenu.MessagePopup.Show("Settings reset to default.");
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Save":
                    foreach (SaveableSettingsContainer setting in _saveableSettings)
                        setting.Save();
                    if (Application.loadedLevel == 0)
                        Hide();
                    else
                        GameMenu.TogglePause(false);
                    break;
                case "Load":
                    foreach (SaveableSettingsContainer setting in _saveableSettings)
                        setting.Load();
                    RebuildCategoryPanel();
                    UIManager.CurrentMenu.MessagePopup.Show("Settings loaded from file.");
                    break;
                case "Continue":
                    if (Application.loadedLevel == 0)
                        Hide();
                    else
                        GameMenu.TogglePause(false);
                    break;
                case "Default":
                    UIManager.CurrentMenu.ConfirmPopup.Show("Are you sure you want to reset to default?", () => OnConfirmSetDefault(),
                        "Reset default");
                    break;
                case "Quit":
                    foreach (SaveableSettingsContainer setting in _saveableSettings)
                        setting.Load();
                    if (Application.loadedLevel == 0)
                        Application.Quit();
                    else
                    {
                        GameMenu.TogglePause(false);
                        if (PhotonNetwork.connected)
                            PhotonNetwork.Disconnect();
                        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                        FengGameManagerMKII.instance.gameStart = false;
                        FengGameManagerMKII.instance.DestroyAllExistingCloths();
                        Destroy(GameObject.Find("MultiplayerManager"));
                        Application.LoadLevel("menu");
                    }
                    break;
            }
        }
        
        public override void Hide()
        {
            if (gameObject.activeSelf)
            {
                foreach (SaveableSettingsContainer setting in _saveableSettings)
                    setting.Apply();
                
            }
            base.Hide();
        }
    }
}
