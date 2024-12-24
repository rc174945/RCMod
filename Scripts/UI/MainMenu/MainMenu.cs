using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;

namespace UI
{
    class MainMenu: BaseMenu
    {
        public BasePopup _singleplayerPopup;
        public BasePopup _multiplayerMapPopup;
        public BasePopup _settingsPopup;
        public BasePopup _toolsPopup;
        public BasePopup _multiplayerRoomListPopup;
        public BasePopup _editProfilePopup;
        public BasePopup _questsPopup;

        protected Text _multiplayerStatusLabel;

        public override void Setup()
        {
            base.Setup();
            if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
            {
                GameObject background = ElementFactory.InstantiateAndBind(transform, "MainBackground");
                background.AddComponent<IgnoreScaler>();
            }
            SetupIntroPanel();
            SetupLabels();
        }

        public void ShowMultiplayerRoomListPopup()
        {
            HideAllPopups();
            _multiplayerRoomListPopup.Show();
        }

        public void ShowMultiplayerMapPopup()
        {
            HideAllPopups();
            _multiplayerMapPopup.Show();
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _singleplayerPopup = ElementFactory.CreateHeadedPanel<SingleplayerPopup>(transform).GetComponent<BasePopup>();
            _multiplayerMapPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerMapPopup>(transform, "MultiplayerMapPopup").
                GetComponent<BasePopup>();
            _editProfilePopup = ElementFactory.CreateHeadedPanel<EditProfilePopup>(transform).GetComponent<BasePopup>();
            _settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(transform).GetComponent<BasePopup>();
            _toolsPopup = ElementFactory.CreateHeadedPanel<ToolsPopup>(transform).GetComponent<BasePopup>();
            _multiplayerRoomListPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerRoomListPopup>(transform, "MultiplayerRoomListPopup").
                GetComponent<BasePopup>();
            _questsPopup = ElementFactory.CreateHeadedPanel<QuestPopup>(transform).GetComponent<BasePopup>();
            _popups.Add(_singleplayerPopup);
            _popups.Add(_multiplayerMapPopup);
            _popups.Add(_editProfilePopup);
            _popups.Add(_settingsPopup);
            _popups.Add(_toolsPopup);
            _popups.Add(_multiplayerRoomListPopup);
            _popups.Add(_questsPopup);
        }

        private void SetupIntroPanel()
        {
            GameObject introPanel = ElementFactory.InstantiateAndBind(transform, "IntroPanel");
            ElementFactory.SetAnchor(introPanel, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(-10f, 30f));
            foreach (Transform transform in introPanel.transform.Find("Buttons"))
            {
                IntroButton introButton = transform.gameObject.AddComponent<IntroButton>();
                introButton.onClick.AddListener(() => OnIntroButtonClick(introButton.name));
            }
        }

        private void SetupLabels()
        {
            GameObject patreonButton = ElementFactory.CreateDefaultButton(transform, new ElementStyle(), "Download Aottg2",
                onClick: () => OnIntroButtonClick("Donate"), elementWidth: 240f, elementHeight: 70f);
            ElementFactory.SetAnchor(patreonButton, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-40f, -460f));
            _multiplayerStatusLabel = ElementFactory.CreateDefaultLabel(transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
            ElementFactory.SetAnchor(_multiplayerStatusLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            _multiplayerStatusLabel.color = Color.white;
            Text versionText = ElementFactory.CreateDefaultLabel(transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
            ElementFactory.SetAnchor(versionText.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
            versionText.color = Color.white;
            if (ApplicationConfig.DevelopmentMode)
                versionText.text = "RC MOD DEVELOPMENT VERSION";
            else
                versionText.text = "RC Mod Version " + ApplicationConfig.GameVersion + ".";
        }

        private void Update()
        {
            if (_multiplayerStatusLabel != null)
            {
                _multiplayerStatusLabel.text = PhotonNetwork.connectionStateDetailed.ToString();
                if (PhotonNetwork.connected)
                {
                    _multiplayerStatusLabel.text += " ping:" + PhotonNetwork.GetPing();
                }
            }
        }

        private void OnIntroButtonClick(string name)
        {
            HideAllPopups();
            switch (name)
            {
                case "SingleplayerButton":
                    _singleplayerPopup.Show();
                    break;
                case "MultiplayerButton":
                    _multiplayerMapPopup.Show();
                    break;
                case "ProfileButton":
                    _editProfilePopup.Show();
                    break;
                case "QuestsButton":
                    _questsPopup.Show();
                    break;
                case "SettingsButton":
                    _settingsPopup.Show();
                    break;
                case "ToolsButton":
                    _toolsPopup.Show();
                    break;
                case "QuitButton":
                    Application.Quit();
                    break;
                case "Donate":
                    Application.OpenURL("https://www.aottg2.com");
                    break;
            }
        }
    }
}
